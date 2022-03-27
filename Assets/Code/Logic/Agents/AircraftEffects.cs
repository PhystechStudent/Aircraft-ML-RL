using System.Collections;
using UnityEngine;

namespace Code.Logic.Agents
{
	public class AircraftEffects : MonoBehaviour
	{
		[SerializeField] private AircraftInteraction _interaction;
		[SerializeField] private TrailRenderer _trail;
		[SerializeField] private ParticleSystem _explodeParticle;
		[SerializeField] private GameObject _mesh;

		private void Start() => _interaction.AgentCollided += OnAgentCollided;

		private void OnDestroy() => _interaction.AgentCollided -= OnAgentCollided;

		private void OnAgentCollided()
		{
			if (Config.GameMode == GameMode.Training) return;

			StopEmitTrail();
			PlayExplodeParticle();
			StartCoroutine(HideMesh());
		}

		public void EmitTrail(bool boost)
		{
			if (boost && _trail.emitting == false) _trail.Clear();
			_trail.emitting = boost;
		}

		private void StopEmitTrail() => _trail.emitting = false;

		private void PlayExplodeParticle() => _explodeParticle.Play();

		private IEnumerator HideMesh()
		{
			_mesh.SetActive(false);
			yield return new WaitForSeconds(2f);
			_mesh.SetActive(true);
		}
	}
}