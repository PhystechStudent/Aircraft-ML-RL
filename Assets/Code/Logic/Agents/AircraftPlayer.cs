using System;
using Code.Infrastructure.InputService;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Zenject;

namespace Code.Logic.Agents
{
    public class AircraftPlayer : AircraftAgent
    {
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
            
            // Dictionary<ActionType, int> actionsTypes = new()
            // {
            //     {ActionType.Pitch, discreteActions[0]},
            //     {ActionType.Yaw, discreteActions[1]},
            //     {ActionType.Boost, discreteActions[2]},
            // };
            // if (actionsTypes == null) throw new ArgumentNullException(nameof(actionsTypes));

            int pitch = Mathf.RoundToInt(_inputService.Pitch);
            int yaw = Mathf.RoundToInt(_inputService.Yaw);
            int boost = Convert.ToInt32(_inputService.Boost);

            if (pitch == -1) pitch = 2;
            if (yaw == -1) yaw = 2;

            discreteActions[0] = pitch;
            discreteActions[1] = yaw;
            discreteActions[2] = boost;

            // actionsTypes[ActionType.Pitch] = pitch;
            // actionsTypes[ActionType.Yaw] = yaw;
            // actionsTypes[ActionType.Boost] = boost;
        }
    }
}