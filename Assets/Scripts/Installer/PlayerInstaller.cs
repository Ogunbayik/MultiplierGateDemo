using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [Header("Bullet Pool Settings")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _bulletSize;
    [SerializeField] private Transform _bulletRoot;
    [Header("Soldier Pool Settings")]
    [SerializeField] private GameObject _soldierPrefab;
    [SerializeField] private int _soldierSize;
    [SerializeField] private Transform _soldierRoot;
    public override void InstallBindings()
    {
        Container.Bind<PlayerController>().FromComponentOnRoot().AsSingle();
        Container.Bind<PlayerSquadManager>().FromComponentOnRoot().AsSingle();

        Container.BindMemoryPool<Bullet, Bullet.Pool>()
           .WithInitialSize(_bulletSize)
           .FromComponentInNewPrefab(_bulletPrefab)
           .UnderTransform(_bulletRoot)
           .NonLazy();

        Container.BindMemoryPool<Soldier, Soldier.Pool>()
            .WithInitialSize(_soldierSize)
            .FromComponentInNewPrefab(_soldierPrefab)
            .UnderTransform(_soldierRoot)
            .NonLazy();
    }
}