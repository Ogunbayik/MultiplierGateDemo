using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerAttacked;

    private IInputService _input;

    private PlayerSquadManager _squadManager;

    private PlayerDataSO _data;

    private Vector3 _movementDirection;

    private bool _canAttack = true;
    private bool _isReloading = false;

    public PlayerDataSO Data => _data;
    [Inject]
    public void Construct(IInputService input,PlayerSquadManager squadManager, PlayerDataSO data)
    {
        _input = input;
        _squadManager = squadManager;
        _data = data;
    }
    void Update()
    {
        if (_input.IsPressedAttack() && _canAttack)
            Attack();

        if (_canAttack && !_isReloading)
        {
            HandleMovement();
            ApplyConstraints();
        }
    }
    private void Attack() => OnPlayerAttacked?.Invoke();
    private void HandleMovement()
    {
        var horizontal = _input.GetHorizontal();
        var vertical = _input.GetVertical();
        var currentSpeed = _movementDirection.sqrMagnitude;
        var squad = _squadManager.ActiveSoldiers;

        _movementDirection.Set(horizontal, 0f, vertical);
        if (currentSpeed >= 1f)
            _movementDirection.Normalize();

        foreach (var soldier in squad)
        {
            soldier.AnimatorController.PlayMoveAnimation(currentSpeed);

            if (currentSpeed > 0.01f)
                soldier.AnimatorController.PlayBlendMoveAnimation(horizontal, vertical);
        }

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
    public void SetReloadStatus(bool isActive) => _isReloading = isActive;
}
