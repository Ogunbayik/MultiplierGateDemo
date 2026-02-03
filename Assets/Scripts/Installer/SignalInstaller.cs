using UnityEngine;
using Zenject;

public class SignalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<GameSignal.PlayerTriggeredGateSignal>();
    }
}