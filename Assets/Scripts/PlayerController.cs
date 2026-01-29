using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    private IInputService _input;

    private PlayerDataSO _data;

    [Inject]
    public void Construct(IInputService input, PlayerDataSO data)
    {
        _input = input;
        _data = data;
    }
    private Vector3 _movementDirection;
    void Update()
    {
        HandleMovement();
        ApplyConstraints();
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
}
