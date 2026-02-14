using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Gate : MonoBehaviour, IDamageable
{
    public event Action<int> OnGateValueChanged;
    public enum GateType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    private MeshRenderer _pillarRenderer;
    private MeshRenderer _panelRenderer;

    private SignalBus _signalBus;

    private GateUI _gateUI;

    [Header("Gate Settings")]
    [SerializeField] private GateType _type;
    [SerializeField] private int _initialValue;
    [SerializeField] private int _maxValue;
    [SerializeField] private float _movementSpeed;
    [Header("Pillar Colors")]
    [SerializeField] private Color _pillarIncreaseColor;
    [SerializeField] private Color _pillarDecreaseColor;
    [Header("Panel Colors")]
    [SerializeField] private Color _panelIncreaseColor;
    [SerializeField] private Color _panelDecreaseColor;

    [Inject]
    public void Construct(
        SignalBus signalBus,
        [Inject(Id = GameConstant.GatePartTypes.GATE_PILLAR)] MeshRenderer pillarRenderer,
        [Inject(Id = GameConstant.GatePartTypes.GATE_PANEL)] MeshRenderer panelRenderer,
        GateUI gateUI)
    {
        _signalBus = signalBus;
        _pillarRenderer = pillarRenderer;
        _panelRenderer = panelRenderer;
        _gateUI = gateUI;
    }

    private Predicate<int> CanIncrease;

    private bool _isTriggered = false;

    private int _currentValue;
    public int CurrentValue => _currentValue;
    public GateType Type => _type;
    private void Start() => Initialize();
    private void Initialize()
    {
        _currentValue = _initialValue;
        CanIncrease = (value) => value < _maxValue;
        SetGateColor();
        _gateUI.UpdateValueText(_currentValue);
    }
    private void Update()
    {
        transform.Translate(Vector3.back * _movementSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameConstant.Tag.PLAYER) && !_isTriggered)
        {
            _isTriggered = true;
            _signalBus.Fire(new GameSignal.PlayerTriggeredGateSignal(this));
            Destroy(gameObject, GameConstant.Timers.GATE_DESTROY_TIME);
        }
    }
    private void SetGateType(GateType type) => _type = type;
    private void SetGateColor()
    {
        if (_type == GateType.Addition || _type == GateType.Multiplication)
        {
            if (_pillarRenderer.material.color == _pillarIncreaseColor)
                return;

            _pillarRenderer.material.color = _pillarIncreaseColor;
            _panelRenderer.material.color = _panelIncreaseColor;
        }
        else
        {
            if (_pillarRenderer.material.color == _pillarDecreaseColor)
                return;

            _pillarRenderer.material.color = _pillarDecreaseColor;
            _panelRenderer.material.color = _panelDecreaseColor;
        }
    }
    public void TakeDamage(int damageAmount) => IncreaseValue();
    private void IncreaseValue()
    {
        if (_type == GateType.Multiplication || _type == GateType.Division)
            return;

        if (CanIncrease(_currentValue))
        {
            _currentValue++;

            if (_currentValue > 0 && _type == GateType.Subtraction)
            {
                SetGateType(GateType.Addition);
                SetGateColor();
            }

            OnGateValueChanged?.Invoke(_currentValue);
        }
    }

}
