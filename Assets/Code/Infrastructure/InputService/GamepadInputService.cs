namespace Code.Infrastructure.InputService
{
    public class GamepadInputService : IInputService
    {
        public float Pitch => _playerControls.PlayerMovement.Pitch.ReadValue<float>();
        public float Yaw => _playerControls.PlayerMovement.Yaw.ReadValue<float>();
        public float Roll => _playerControls.PlayerMovement.Roll.ReadValue<float>();
        public bool Boost => _playerControls.PlayerMovement.Boost.ReadValue<bool>();
        

        private readonly PlayerControls _playerControls;

        public GamepadInputService()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }

        ~GamepadInputService()
        {
            
        }
    }
}