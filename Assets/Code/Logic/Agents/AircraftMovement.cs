using System.Collections;
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

		private IAssetProvider _assetProvider;
		private AircraftData _data;

		private float _smoothPitchChange;
		private float _smoothYawChange;
		private float _smoothRollChange;
		private float _roll;
		private bool _isFrozen;

		[Inject]
		private void Construct(IAssetProvider assetProvider)
		{
			_assetProvider = assetProvider;
		}

		private void Start()
		{
			_data = _assetProvider.Load<AircraftData>(AssetPath.AircraftData);
			_interaction.AgentCollided += OnAgentCollided;
		}

		private void OnDestroy() => _interaction.AgentCollided -= OnAgentCollided;

		private void OnAgentCollided() => StartCoroutine(Freeze());

		private IEnumerator Freeze()
		{
			_isFrozen = true;
			_rigidbody.Sleep();
			
			yield return new WaitForSeconds(3f);
			
			_rigidbody.WakeUp();
			_isFrozen = false;
		}

		public void Move(bool isBoost)
		{
			if (_isFrozen) return;
			
			float boostModifier = isBoost ? _data.BoostMultiplier : 1f;

			_rigidbody.AddForce(transform.forward * _data.Thrust * boostModifier, ForceMode.Force);
		}

		public void Rotate(int pitch, int yaw)
		{
			if (_isFrozen) return;

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