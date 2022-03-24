using UnityEngine;

namespace Code.Logic.Balance
{
    [CreateAssetMenu(fileName = "New Aircraft Data", menuName = "Data / Aircraft Data")]
    public class AircraftData : ScriptableObject
    {
        public float Thrust = 100000f;
        public float PitchSpeed = 100f;
        public float YawSpeed = 100f;
        public float RollSpeed = 100f;
        public float BoostMultiplier = 2f;
        public float MaxPitchAngle = 45f;
        public float MaxRollAngle = 45f;
    }
}