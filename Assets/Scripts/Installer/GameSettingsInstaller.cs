using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [Header("Data Settings")]
    [SerializeField] private PlayerDataSO _playerData;
    public override void InstallBindings()
    {
        Container.Bind<PlayerDataSO>().FromInstance(_playerData).AsSingle();
    }
}