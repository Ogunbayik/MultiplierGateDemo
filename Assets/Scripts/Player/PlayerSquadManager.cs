using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSquadManager : MonoBehaviour
{
    private Soldier.Pool _soldierPool;

    private List<Soldier> _activeSoldierList = new List<Soldier>();

    private int _initialCount = 2;
    private int _currentSquadCount;

    [Inject]
    public void Construct(Soldier.Pool soldierPool) => _soldierPool = soldierPool;

    private void Start() => InitialSquad();
    private void InitialSquad()
    {
        _currentSquadCount = _initialCount;
        for (int i = 0; i < _currentSquadCount; i++)
        {
            var soldier = _soldierPool.Spawn(_soldierPool);
            _activeSoldierList.Add(soldier);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            IncreaseSoldier(1);
    }
    private void IncreaseSoldier(int soldierCount)
    {
        for (int i = 0; i < soldierCount; i++)
        {
            var soldier = _soldierPool.Spawn(_soldierPool);
            _activeSoldierList.Add(soldier);
        }
    }
}
