using UnityEngine;
using Zenject;

public class GateInstaller : MonoInstaller
{
    [Header("Visual References")]
    [SerializeField] private MeshRenderer _panelRenderer;
    [SerializeField] private MeshRenderer _pillarRenderer;
    public override void InstallBindings()
    {
        Container.Bind<Gate>().FromComponentOnRoot().AsSingle();
        Container.Bind<GateUI>().FromComponentOnRoot().AsSingle();

        Container.Bind<MeshRenderer>()
            .WithId(GameConstant.GatePartTypes.GATE_PANEL)
            .FromInstance(_panelRenderer)
            .AsCached();

        Container.Bind<MeshRenderer>()
            .WithId(GameConstant.GatePartTypes.GATE_PILLAR)
            .FromInstance(_pillarRenderer)
            .AsCached();
    }
}