using System;
using Code.Logic.Interactions;
using UnityEngine;

namespace Code.Logic.Agents
{
    public class AircraftInteraction : MonoBehaviour
    {
        public event Action EnvironmentCollided;
        public event Action<CheckPoint> CheckPointTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CheckPoint checkPoint))
                CheckPointTriggered?.Invoke(checkPoint);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<AircraftAgent>()) return;

            EnvironmentCollided?.Invoke();
        }
    }
}