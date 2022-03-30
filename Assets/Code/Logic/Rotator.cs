using UnityEngine;

namespace Code.Logic
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private void Update() => Rotate();

        private void Rotate() => transform.Rotate(Vector3.forward * (_speed * Time.deltaTime), Space.Self);
    }
}