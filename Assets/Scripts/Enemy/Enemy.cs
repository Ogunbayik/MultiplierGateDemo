using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public event Action OnEnemyHealthChanged;
    public event Action OnEnemyDead;

    [Header("Enemy Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _checkRadius;

    private int _currentHealth;
    public int CurrentHealth => _currentHealth;

    private bool _isDead;
    private void Start()
    {
        _currentHealth = _maxHealth;
    }
    private void Update()
    {
        if (!_isDead)
            HandleMovement();
    }

    private void HandleMovement()
    {
        transform.Translate(Vector3.back * _movementSpeed * Time.deltaTime);
        //Enemy için check Radius eklenecek ve alana girince direkt player'a doðru kovalamaya baþlayacak ve hýzý biraz artacak
    }
    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        OnEnemyHealthChanged?.Invoke();

        if(_currentHealth <= 0)
        {
            OnEnemyDead?.Invoke();
            _currentHealth = 0;
            _isDead = true;
        }
    }        
    public float GetHealthPercentage() => (float)_currentHealth / _maxHealth;
}
