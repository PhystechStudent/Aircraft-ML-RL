using System;
using Code.Infrastructure.InputService;
using UnityEngine;

namespace Code.Logic.Agents
{
    public class TestInput : MonoBehaviour
    {
        private IInputService _inputService;

        private void OnEnable()
        {
            _inputService = new GamepadInputService();
        }

        private void Update()
        {
            Debug.Log(_inputService.Pitch);
        }
    }
}