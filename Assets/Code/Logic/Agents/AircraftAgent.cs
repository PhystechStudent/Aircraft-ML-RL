using System.Collections;
using System.Collections.Generic;
using Code.Infrastructure.Assets;
using Code.Logic.Balance;
using Code.Logic.Interactions;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Zenject;

namespace Code.Logic.Agents
{
    public class AircraftAgent : Agent
    {
        private const string CheckpointRadiusKey = "checkpoint_radius";

        [SerializeField] private AircraftMovement _movement;
        [SerializeField] private AircraftEffects _effects;
        [SerializeField] private AircraftInteraction _interaction;
        [SerializeField] private CheckPointSpawner _checkPointSpawner;
        [SerializeField] private int _order;
        [SerializeField] private int _stepTimeout = 300;

        private IAssetProvider _assetProvider;
        private AgentRewardData _rewardData;

        private int _nextCheckPointIndex;
        private float _nextStepTimeout;


        [Inject]
        private void Construct(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public override void Initialize()
        {
            if (Config.GameMode == GameMode.Game)
                MaxStep = 0;

            _rewardData = _assetProvider.Load<AgentRewardData>(AssetPath.RewardData);

            _interaction.CheckPointTriggered += OnCheckPointTriggered;
            _interaction.EnvironmentCollided += OnEnvironmentCollided;
        }


        private void OnDestroy()
        {
            _interaction.CheckPointTriggered -= OnCheckPointTriggered;
            _interaction.EnvironmentCollided -= OnEnvironmentCollided;
        }

        private void OnCheckPointTriggered(CheckPoint checkPoint)
        {
            if (checkPoint != _checkPointSpawner.CheckPoints[_nextCheckPointIndex]) return;

            CollectCheckPoint();
        }

        private void OnEnvironmentCollided()
        {
            if (Config.GameMode == GameMode.Training)
            {
                AddReward(_rewardData.CollisionReward);
                EndEpisode();
            }
            else StartCoroutine(ResetPositionWithDelay());
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            if (_movement.IsFrozen) return;

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

            if (Config.GameMode == GameMode.Game) return;

            GiveReward();
            //CheckDistanceToCheckPoint();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            //Aircraft velocity
            sensor.AddObservation(transform.InverseTransformDirection(_movement.Velocity));

            //Distance to next checkpoint
            sensor.AddObservation(GetDistanceToNextCheckPoint());

            //Next checkpoint orientation
            Vector3 nextCheckPointOrientation = _checkPointSpawner.CheckPoints[_nextCheckPointIndex].transform.forward;
            sensor.AddObservation(nextCheckPointOrientation);
        }

        public override void OnEpisodeBegin()
        {
            _movement.ResetVelocity();
            _movement.ResetPosition(_nextCheckPointIndex, _order);
            _effects.StopEmitTrail();

            if (Config.GameMode == GameMode.Training)
                _nextStepTimeout = StepCount + _stepTimeout;
        }

        private IEnumerator ResetPositionWithDelay()
        {
            yield return new WaitForSeconds(2f);
            _movement.ResetPosition(_nextCheckPointIndex, _order);
        }

        private void GiveReward()
        {
            AddReward(_rewardData.BaseReward / MaxStep);

            if (StepCount > _nextStepTimeout)
            {
                AddReward(_rewardData.NegativeReward);
                EndEpisode();
            }
        }

        private void CheckDistanceToCheckPoint()
        {
            Vector3 distanceToCheckPoint = GetDistanceToNextCheckPoint();

            if (distanceToCheckPoint.magnitude <
                Academy.Instance.EnvironmentParameters.GetWithDefault(CheckpointRadiusKey, 0f))
                CollectCheckPoint();
        }


        private void CollectCheckPoint()
        {
            _nextCheckPointIndex = (_nextCheckPointIndex + 1) % _checkPointSpawner.CheckPoints.Count;

            if (Config.GameMode == GameMode.Training)
            {
                AddReward(_rewardData.PositiveReward);
                _nextStepTimeout = StepCount + _stepTimeout;
            }
        }

        private Vector3 GetDistanceToNextCheckPoint()
        {
            Vector3 distance = _checkPointSpawner.CheckPoints[_nextCheckPointIndex].transform.position -
                               transform.position;
            Vector3 localDistance = transform.InverseTransformDirection(distance);

            return localDistance;
        }
    }
}