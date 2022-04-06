using System.Collections;
using Cinemachine;
using Code.Infrastructure.Assets;
using Code.Logic.Balance;
using UnityEngine;
using Zenject;

namespace Code.Logic.Agents
{
    public class AircraftMovement : MonoBehaviour
    {
        private const float SmoothDelta = 2f;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private AircraftInteraction _interaction;
        [SerializeField] private CheckPointSpawner _checkPointSpawner;
        [SerializeField] private CinemachineSmoothPath _racePath;

        private IAssetProvider _assetProvider;

        private AircraftData _data;

        private float _smoothPitchChange;
        private float _smoothYawChange;
        private float _smoothRollChange;
        private float _roll;

        public bool IsFrozen { get; private set; }

        public Vector3 Velocity => _rigidbody.velocity;

        [Inject]
        private void Construct(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        private void Start()
        {
            _data = _assetProvider.Load<AircraftData>(AssetPath.AircraftData);
            _interaction.EnvironmentCollided += OnEnvironmentCollided;
        }

        private void OnDestroy() => _interaction.EnvironmentCollided -= OnEnvironmentCollided;

        private void OnEnvironmentCollided()
        {
            if (Config.GameMode == GameMode.Training) return;

            StartCoroutine(Freeze());
        }

        private IEnumerator Freeze()
        {
            IsFrozen = true;
            _rigidbody.Sleep();

            yield return new WaitForSeconds(3f);

            _rigidbody.WakeUp();
            IsFrozen = false;
        }

        public void ResetPosition(int checkPointIndex, int order)
        {
            int previousCheckPointIndex = checkPointIndex - 1;
            if (previousCheckPointIndex == -1)
                previousCheckPointIndex = _checkPointSpawner.CheckPoints.Count - 1;

            float startPoint =
                _racePath.FromPathNativeUnits(previousCheckPointIndex, CinemachinePathBase.PositionUnits.PathUnits);

            Vector3 startPosition = _racePath.EvaluatePosition(startPoint);
            Quaternion startRotation = _racePath.EvaluateOrientation(startPoint);
            Vector3 positionOffset = Vector3.right * (order - 4 / 2f) * Random.Range(9f, 10f);

            transform.position = startPosition + startRotation * positionOffset;
            transform.rotation = startRotation;
        }

        public void ResetVelocity()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void Move(bool isBoost)
        {
            if (IsFrozen) return;

            float boostModifier = isBoost ? _data.BoostMultiplier : 1f;

            _rigidbody.AddForce(transform.forward * _data.Thrust * boostModifier, ForceMode.Force);
        }

        public void Rotate(int pitch, int yaw)
        {
            if (IsFrozen) return;

            Vector3 rotation = transform.rotation.eulerAngles;

            float rollAngle = rotation.z > 180f ? rotation.z - 360f : rotation.z;

            if (yaw == 0)
                _roll = -rollAngle / _data.MaxRollAngle;
            else _roll = -yaw;

            _smoothPitchChange = Mathf.MoveTowards(_smoothPitchChange, pitch, SmoothDelta * Time.fixedDeltaTime);
            _smoothYawChange = Mathf.MoveTowards(_smoothYawChange, yaw, SmoothDelta * Time.fixedDeltaTime);
            _smoothRollChange = Mathf.MoveTowards(_smoothRollChange, _roll, SmoothDelta * Time.fixedDeltaTime);

            float newPitch = rotation.x + _smoothPitchChange * Time.fixedDeltaTime * _data.PitchSpeed;
            if (newPitch > 180f) newPitch -= 360f;
            newPitch = Mathf.Clamp(newPitch, -_data.MaxPitchAngle, _data.MaxPitchAngle);

            float newYaw = rotation.y + _smoothYawChange * Time.fixedDeltaTime * _data.YawSpeed;

            float newRoll = rotation.z + _smoothRollChange * Time.fixedDeltaTime * _data.RollSpeed;
            if (newRoll > 180f) newRoll -= 360f;
            newRoll = Mathf.Clamp(newRoll, -_data.MaxRollAngle, _data.MaxRollAngle);

            transform.rotation = Quaternion.Euler(newPitch, newYaw, newRoll);
        }
    }
}