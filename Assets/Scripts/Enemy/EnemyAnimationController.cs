using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [Header("Test")]
    public Enemy _enemy;

    private void OnEnable() => _enemy.OnEnemyDead += Enemy_OnEnemyDead;   
    private void OnDisable() => _enemy.OnEnemyDead -= Enemy_OnEnemyDead;   
    private void Enemy_OnEnemyDead()
    {
        Debug.Log("Playing enemy dead animation");
    }
}
