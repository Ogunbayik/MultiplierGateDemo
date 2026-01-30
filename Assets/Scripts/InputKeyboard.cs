using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyboard : IInputService
{
    public float GetHorizontal() => Input.GetAxis(GameConstant.PlayerInput.HORIZONTAL);
    public float GetVertical() => Input.GetAxis(GameConstant.PlayerInput.VERTICAL);
    public bool IsPressedAttack() => Input.GetKeyDown(KeyCode.Space);
}
