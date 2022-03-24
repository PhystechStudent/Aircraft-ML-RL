using UnityEngine;

namespace Code.Logic.Agents
{
    public class AircraftEffects : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;

        public void EmitTrail(bool boost)
        {
            if (boost && _trail.emitting == false) _trail.Clear();
            _trail.emitting = boost;
        }
    }
}