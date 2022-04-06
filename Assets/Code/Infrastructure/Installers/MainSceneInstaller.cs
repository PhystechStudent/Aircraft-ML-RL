using Code.Infrastructure.Assets;
using Code.Infrastructure.Factories;
using Code.Infrastructure.InputService;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindInputService();
            BindCheckPointFactory();
            BindAssetProvider();
        }


        private void BindInputService() => Container.Bind<IInputService>().To<GamepadInputService>().AsSingle();

        private void BindCheckPointFactory() =>
            Container.Bind<ICheckPointsFactory>().To<CheckPointsFactory>().AsSingle();


        private void BindAssetProvider() => Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
    }
}