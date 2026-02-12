using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IDamageable, IPoolable<IMemoryPool>
{
    private IMemoryPool _pool;

    public event Action OnEnemyHealthChanged;
    public event Action OnEnemyDead;

    private EnemyVision _vision;
    private Collider _collider;

    [Header("Enemy Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private int _maxHealth;

    private int _currentHealth;
    public int CurrentHealth => _currentHealth;

    private Transform _target;

    private bool _isDead;
    private bool _isChasing;

    [Inject]
    public void Construct(EnemyVision vision, Collider collider)
    {
        _vision = vision;
        _collider = collider;
    }
    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
        ResetEnemyState();
    }
    public void OnDespawned()
    {
        _pool = null;
    }
    public void ReturnToPool() => _pool.Despawn(this);
    private void ResetEnemyState()
    {
        _currentHealth = _maxHealth;
        _collider.enabled = true;
        _isDead = false;
        _isChasing = false;
    }
    private void OnEnable() => _vision.OnPlayerDetected += Vision_OnPlayerDetected;
    private void OnDisable() => _vision.OnPlayerDetected -= Vision_OnPlayerDetected;
    private void Vision_OnPlayerDetected(Transform player)
    {
        SetTarget(player);
        _isChasing = true;
    }
    private void Update() => EvaluateBehaviour();
    private void EvaluateBehaviour()
    {
        if (_isDead)
            return;

        if (_isChasing)
            HandleChase(_target);
        else
            HandleMovement();
    }
    private void HandleMovement() => transform.Translate(Vector3.back * _movementSpeed * Time.deltaTime);
    private void HandleChase(Transform target) => transform.position = Vector3.MoveTowards(transform.position, target.position, _chaseSpeed * Time.deltaTime);
    private void SetTarget(Transform target) => _target = target;
    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        OnEnemyHealthChanged?.Invoke();

        if (_currentHealth <= 0)
            Die();
    }        
    private void Die()
    {
        _currentHealth = 0;
        _isDead = true;
        _collider.enabled = false;
        Debug.Log("Enemy is dead");
        OnEnemyDead?.Invoke();
    }
    public float GetHealthPercentage() => (float)_currentHealth / _maxHealth;

    public class Pool : MonoPoolableMemoryPool<IMemoryPool, Enemy> { }
}
