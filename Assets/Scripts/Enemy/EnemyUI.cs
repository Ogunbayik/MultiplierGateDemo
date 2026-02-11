using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [Header("Test")]
    public Enemy _enemy;
    [Header("UI References")]
    [SerializeField] private Image _healthFill;


    private void Start() => Initialize();
    private void Initialize() => _healthFill.fillAmount = 1;
    private void OnEnable() => _enemy.OnEnemyHealthChanged += Enemy_OnEnemyHealthChanged;
    private void OnDisable() => _enemy.OnEnemyHealthChanged -= Enemy_OnEnemyHealthChanged;   
    private void Enemy_OnEnemyHealthChanged() => UpdateFillAmount();
    public void UpdateFillAmount()
    {
        float percentage = _enemy.GetHealthPercentage();
        _healthFill.fillAmount = percentage;
    }
}
