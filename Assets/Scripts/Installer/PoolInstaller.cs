using UnityEngine;
using Zenject;

public class PoolInstaller : MonoInstaller
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject _cactusPrefab;
    [Header("Managers")]
    [SerializeField] private SpawnManager _spawnManager;
    public override void InstallBindings()
    {
        Container.BindMemoryPool<Enemy,Enemy.Pool>()
            .WithInitialSize(20)
            .FromComponentInNewPrefab(_cactusPrefab)
            .UnderTransformGroup(GameConstant.PoolGroups.ENEMIES);

        Container.Bind<SpawnManager>().FromInstance(_spawnManager).AsSingle();
    }
}