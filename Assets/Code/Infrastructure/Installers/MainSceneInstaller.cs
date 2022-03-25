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
        }
        
        private void BindInputService() => Container.Bind<IInputService>().To<GamepadInputService>().AsSingle();
        private void BindCheckPointFactory() => Container.Bind<ICheckPointsFactory>().To<CheckPointsFactory>().AsSingle();
    }
}