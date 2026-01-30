using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerAttacked;

    private IInputService _input;

    private PlayerDataSO _data;

    private Vector3 _movementDirection;

    private bool _canAttack = true;

    public PlayerDataSO Data => _data;
    [Inject]
    public void Construct(IInputService input, PlayerDataSO data)
    {
        _input = input;
        _data = data;
    }
    void Update()
    {
        HandleMovement();
        ApplyConstraints();

        if (_input.IsPressedAttack() && _canAttack)
            Attack();
    }
    private void Attack()
    {
        OnPlayerAttacked?.Invoke();
        _canAttack = false;
    }
    private void HandleMovement()
    {
        var horizontal = _input.GetHorizontal();
        var vertical = _input.GetVertical();

        _movementDirection.Set(horizontal, 0f, vertical);
        if (_movementDirection.sqrMagnitude >= 1f)
            _movementDirection.Normalize();

        transform.Translate(_movementDirection * _data.MovementSpeed * Time.deltaTime);
    }
    private void ApplyConstraints()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, _data.MinXConstraint, _data.MaxXConstraint);
        position.z = Mathf.Clamp(position.z, _data.MinZConstraint, _data.MaxZConstraint);

        transform.position = position;
    }
    public void SetAttackStatus(bool canAttack) => _canAttack = canAttack;
}
