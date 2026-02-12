using UnityEngine;
using Zenject;

public class EnemyInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Enemy>().FromComponentOnRoot().AsSingle();
        Container.Bind<EnemyUI>().FromComponentOnRoot().AsSingle();
        Container.Bind<EnemyVision>().FromComponentOnRoot().AsSingle();

        Container.Bind<Animator>().FromComponentOnRoot().AsSingle();

        Container.Bind<Collider>().FromComponentOnRoot().AsSingle();
        
    }
}