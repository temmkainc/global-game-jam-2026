using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [field: SerializeField] public Player Player { get; set; }
    [field: SerializeField] public PlayerSight PlayerSight { get; set; }
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(Player).AsSingle().NonLazy();
        Container.Bind<PlayerSight>().FromInstance(PlayerSight).AsSingle().NonLazy();
        Container.Bind<PlayerStateManager>().AsSingle().NonLazy();
        Container.Bind<MaskStateManager>().AsSingle().NonLazy();
    }
}