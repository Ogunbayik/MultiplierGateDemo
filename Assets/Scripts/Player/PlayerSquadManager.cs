using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using static Gate;

public class PlayerSquadManager : MonoBehaviour
{
    private SignalBus _signalBus;

    private Soldier.Pool _soldierPool;

    private List<Soldier> _activeSoldierList = new List<Soldier>();
    private PlayerDataSO _data;

    public int _initialCount;
    public int _maxSoldierCount;

    private Vector3 _desiredPosition;


    [Inject]
    public void Construct(Soldier.Pool soldierPool,PlayerDataSO data, SignalBus signalBus)
    {
        _soldierPool = soldierPool;
        _data = data;
        _signalBus = signalBus;
    }

    private void Start() => InitialSquad();
    private void InitialSquad()
    {
        for (int i = 0; i < _initialCount; i++)
        {
            var soldier = _soldierPool.Spawn(_soldierPool);
            _activeSoldierList.Add(soldier);
        }

        RelocateSoldiers();
    }
    private void OnEnable()
    {
        _signalBus.Subscribe<GameSignal.PlayerTriggeredGateSignal>(OnPlayerTriggerGate);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<GameSignal.PlayerTriggeredGateSignal>(OnPlayerTriggerGate);
    }
    private void OnPlayerTriggerGate(GameSignal.PlayerTriggeredGateSignal signal) => UpdateSoldierSequence(signal).Forget();
    private async UniTaskVoid UpdateSoldierSequence(GameSignal.PlayerTriggeredGateSignal signal)
    {
        CancellationToken token = this.GetCancellationTokenOnDestroy();
        //Ýlk önce duman efekti eklenecek..
        Debug.Log("Smoke effect is activated!.. Effect time is 0.5f");
        await UniTask.Delay(2000, cancellationToken: token);
        //Sonra asker sayýsý hesaplanacak..
        var targetCount = CalculateValue(signal.Gate.Type, signal.Gate.CurrentValue);
        UpdateSoldierCount(targetCount);
        await UniTask.Delay(2000, cancellationToken: token);
        //En sonda da formasyon deðiþecek..
        RelocateSoldiers();
    }
    private void UpdateSoldierCount(int targetAmount)
    {
        ResetSquadList();

        for (int i = 0; i < targetAmount; i++)
            CreateSoldier();
    }
    private int CalculateValue(GateType gateType, int gateValue)
    {
        int currentCount = _activeSoldierList.Count;
        int result = 0;

        switch(gateType)
        {
            case GateType.Addition:
                result = currentCount + gateValue;
                break;
            case GateType.Subtraction:
                result = currentCount + gateValue;
                break;
            case GateType.Multiplication:
                result = currentCount * gateValue;
                break;
            case GateType.Division:
                result = currentCount / gateValue;
                break;
        }

        return Mathf.Clamp(result, GameConstant.Setting.MINIMUM_SOLDIER_COUNT, _maxSoldierCount);
    }
    private void ResetSquadList()
    {
        foreach (var soldier in _activeSoldierList)
            soldier.ReturnToPool();

        _activeSoldierList.Clear();
    }
    private void CreateSoldier()
    {
        var soldier = _soldierPool.Spawn(_soldierPool);
        soldier.transform.localPosition = Vector3.zero;
        _activeSoldierList.Add(soldier);
    }
    private void RelocateSoldiers()
    {
        if(_activeSoldierList.Count % 2 == 0)
        {
            //Çift Sayýda Asker Var
            for (int i = 0; i < _activeSoldierList.Count; i++)
            {
                bool isRight = i % 2 == 0;
                var offsetZ = Mathf.Floor(i / 2f);

                if (isRight)
                    _desiredPosition = new Vector3(_data.BackRowSpacing, 0f, -offsetZ);
                else
                    _desiredPosition = new Vector3(-_data.BackRowSpacing, 0f, -offsetZ);

                _activeSoldierList[i].transform.DOLocalMove(_desiredPosition, _data.TravelDuration);
            }
        }
        else
        {
            //Tek sayýda asker var
            for (int i = 0; i < _activeSoldierList.Count; i++)
            {
                //Ýlk Soldier
                if (i == 0)
                {
                    _activeSoldierList[i].transform.localPosition = Vector3.zero;
                    continue;
                }

                bool isRight = (i % 2) == 0;
                var offsetZ = Mathf.Floor((i - 1) / 2f);

                if(isRight)
                {
                    //Çift ise pozitif tarafta
                    if (offsetZ == 0)
                        _desiredPosition = new Vector3(_data.FrontRowSpacing, 0f, 0f);
                    else
                        _desiredPosition = new Vector3(_data.BackRowSpacing, 0f, -offsetZ);
                }
                else
                {
                    //Tek ise negatif tarafta
                    if (offsetZ == 0)
                        _desiredPosition = new Vector3(-_data.FrontRowSpacing, 0f, 0f);
                    else
                        _desiredPosition = new Vector3(-_data.BackRowSpacing, 0f, -offsetZ);
                }

                _activeSoldierList[i].transform.DOLocalMove(_desiredPosition, _data.TravelDuration);
            }
        }
    }
}
