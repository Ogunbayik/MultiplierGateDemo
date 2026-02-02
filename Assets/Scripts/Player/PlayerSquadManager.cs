using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class PlayerSquadManager : MonoBehaviour
{
    private Soldier.Pool _soldierPool;

    private List<Soldier> _activeSoldierList = new List<Soldier>();
    private PlayerDataSO _data;

    public int _initialCount;

    private Vector3 _desiredPosition;

    [Inject]
    public void Construct(Soldier.Pool soldierPool,PlayerDataSO data)
    {
        _soldierPool = soldierPool;
        _data = data;
    }

    private void Start() => InitialSquad();
    private void InitialSquad()
    {
        for (int i = 0; i < _initialCount; i++)
        {
            var soldier = _soldierPool.Spawn(_soldierPool);
            _activeSoldierList.Add(soldier);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            IncreaseSoldier(1);
        }

        if (Input.GetKeyDown(KeyCode.Q))
            RelocateSoldiers();
    }
    private void IncreaseSoldier(int soldierCount)
    {
        for (int i = 0; i < soldierCount; i++)
        {
            var soldier = _soldierPool.Spawn(_soldierPool);
            _activeSoldierList.Add(soldier);
        }
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

                _activeSoldierList[i].transform.DOMove(_desiredPosition, _data.TravelDuration);
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
                    _activeSoldierList[i].transform.position = Vector3.zero;
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

                _activeSoldierList[i].transform.DOMove(_desiredPosition, _data.TravelDuration);
            }
        }
    }
}
