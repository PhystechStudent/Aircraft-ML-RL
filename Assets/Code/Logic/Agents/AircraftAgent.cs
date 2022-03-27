using System;
using System.Collections.Generic;
using Code.Logic.Interactions;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Zenject;

namespace Code.Logic.Agents
{
	public class AircraftAgent : Agent
	{
		[SerializeField] private AircraftMovement _movement;
		[SerializeField] private AircraftEffects _effects;
		[SerializeField] private AircraftInteraction _interaction;

		private Dictionary<int, AgentDecision> _decisions;

		private CheckPointSpawner _checkPointSpawner;
		
		private int _nextCheckPointIndex;
		
		protected IReadOnlyDictionary<int, AgentDecision> Decisions => _decisions;

		[Inject]
		private void Construct(CheckPointSpawner checkPointSpawner)
		{
			_checkPointSpawner = checkPointSpawner;
		}
		
		private void Start()
		{
			_decisions = new Dictionary<int, AgentDecision>
			{
				{0, AgentDecision.None},
				{1, AgentDecision.Up},
				{2, AgentDecision.Down}
			};

			//_interaction.CheckPointTriggered += OnCheckPointTriggered;
		}


		private void OnDestroy()
		{
			//_interaction.CheckPointTriggered -= OnCheckPointTriggered;
		}

		private void OnCheckPointTriggered(CheckPoint checkPoint)
		{
			if(checkPoint != _checkPointSpawner.CheckPoints[_nextCheckPointIndex]) return;

			CollectCheckPoint();
		}

		private void CollectCheckPoint()
		{
			_nextCheckPointIndex = (_nextCheckPointIndex + 1) % _checkPointSpawner.CheckPoints.Count;
			
			if (Config.GameMode == GameMode.Training)
			{
				AddReward(0.5f);
			}
		}

		public override void OnActionReceived(ActionBuffers actions)
		{
			ActionSegment<int> discreteActions = actions.DiscreteActions;

			Dictionary<ActionType, int> actionsTypes = new()
			{
				{ActionType.Pitch, discreteActions[0]},
				{ActionType.Yaw, discreteActions[1]},
				{ActionType.Boost, discreteActions[2]},
			};

			int pitch = actionsTypes[ActionType.Pitch];
			int yaw = actionsTypes[ActionType.Yaw];
			bool boost = actionsTypes[ActionType.Boost] == 1;

			if (pitch == 2) pitch = -1;
			if (yaw == 2) yaw = -1;

			_movement.Move(boost);
			_movement.Rotate(pitch, yaw);
			_effects.EmitTrail(boost);
		}
	}

	public enum AgentDecision
	{
		Up = 1,
		None = 0,
		Down = 2
	}

	public enum ActionType
	{
		Pitch,
		Yaw,
		Boost
	}
}