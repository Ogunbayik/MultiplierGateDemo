using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class GateUI : MonoBehaviour
{
    private Gate _gate;

    [Inject]
    public void Construct(Gate gate) => _gate = gate;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _gateValueText;
    private void Start() => UpdateValueText(_gate.CurrentValue);
    private void OnEnable() => _gate.OnGateValueChanged += Gate_OnGateValueChanged;
    private void OnDisable() => _gate.OnGateValueChanged -= Gate_OnGateValueChanged;
    private void Gate_OnGateValueChanged(int currentValue) => UpdateValueText(currentValue);
    public void UpdateValueText(int value) => _gateValueText.text = value.ToString();
}
