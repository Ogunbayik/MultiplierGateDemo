using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Gate : MonoBehaviour
{
    public enum GateType
    {
        Additional,
        Multiplication
    }

    private SignalBus _signalBus;

    [Header("Gate Settings")]
    [SerializeField] private GateType _type;
    [SerializeField] private int _gateValue;
    [SerializeField] private int _maxCount;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;

    public Func<GateType,int, int> CalculateValue;

    private Predicate<int> CanIncrease;

    private bool _isTriggered = false;

    private int _currentValue;

    public GateType Type => _type;
    private void Start()
    {
        _currentValue = _gateValue;
        CalculateValue = (type, value) => type == GateType.Additional ? _currentValue + value : _currentValue * value;
        CanIncrease = (value) => value < _maxCount;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameConstant.Tag.PLAYER) && !_isTriggered)
        {
            _isTriggered = true;
            _signalBus.Fire(new GameSignal.PlayerTriggeredGateSignal(this));
            Destroy(gameObject, GameConstant.Timers.GATE_DESTROY_TIME);
        }

        if(other.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
        {
            bullet.ReturnToPool();

            if (CanIncrease(_currentValue))
                IncreaseValue();
        }
    }
    private void IncreaseValue() => _currentValue++;
}
