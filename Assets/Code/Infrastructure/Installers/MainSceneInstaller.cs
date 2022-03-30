using Cinemachine;
using Code.Infrastructure.Assets;
using Code.Infrastructure.Factories;
using Code.Infrastructure.InputService;
using Code.Logic;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private CheckPointSpawner _checkPointSpawner;
        [SerializeField] private CinemachineSmoothPath _path;

        public override void InstallBindings()
        {
            BindInputService();
            BindCheckPointFactory();
            BindAssetProvider();
            BindCheckPointSpawner();
            BindCinemachineSmoothPath();
        }


        private void BindInputService() => Container.Bind<IInputService>().To<GamepadInputService>().AsSingle();

        private void BindCheckPointFactory() =>
            Container.Bind<ICheckPointsFactory>().To<CheckPointsFactory>().AsSingle();


        private void BindAssetProvider() => Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();

        private void BindCheckPointSpawner() =>
            Container.Bind<CheckPointSpawner>().FromInstance(_checkPointSpawner).AsSingle();

        private void BindCinemachineSmoothPath() =>
            Container.Bind<CinemachineSmoothPath>().FromInstance(_path).AsSingle();
    }
}