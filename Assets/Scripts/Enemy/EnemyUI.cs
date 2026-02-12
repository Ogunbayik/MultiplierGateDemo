using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EnemyUI : MonoBehaviour
{
    private Enemy _enemy;

    [Header("UI References")]
    [SerializeField] private Image _healthFill;

    [Inject]
    public void Construct(Enemy enemy) => _enemy = enemy;
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
