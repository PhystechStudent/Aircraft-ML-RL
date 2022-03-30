using UnityEngine;

namespace Code.Logic.Balance
{
    [CreateAssetMenu(fileName = "New Agent Reward", menuName = "Data / Agent Reward Data")]
    public class AgentRewardData : ScriptableObject
    {
        public float BaseReward;
        public float PositiveReward;
        public float NegativeReward;
        public float CollisionReward;
    }
}