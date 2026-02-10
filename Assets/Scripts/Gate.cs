using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Gate : MonoBehaviour
{
    public enum GateType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    private SignalBus _signalBus;

    [Header("Gate Settings")]
    [SerializeField] private GateType _type;
    [SerializeField] private int _initialValue;
    [SerializeField] private int _maxValue;

    [Inject]
    public void Construct(SignalBus signalBus) => _signalBus = signalBus;

    private Predicate<int> CanIncrease;

    private bool _isTriggered = false;

    private int _currentValue;
    public int CurrentValue => _currentValue;
    public GateType Type => _type;
    private void Start()
    {
        _currentValue = _initialValue;
        CanIncrease = (value) => value < _maxValue;
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

            if (_type == GateType.Multiplication || _type == GateType.Division)
                return;

            //Only Additional Gate Can updated!
            if (CanIncrease(_currentValue))
            {
                IncreaseValue();

                if (_currentValue > 0 && _type == GateType.Subtraction)
                    SetGateType(GateType.Addition);
            }
        }
    }
    private void SetGateType(GateType type) => _type = type;
    private void IncreaseValue() => _currentValue++;
    
}
