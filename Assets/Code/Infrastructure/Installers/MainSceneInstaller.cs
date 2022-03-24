using Code.Infrastructure.InputService;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindInputService();
        }

        private void BindInputService() => Container.Bind<IInputService>().To<GamepadInputService>().AsSingle();
    }
}