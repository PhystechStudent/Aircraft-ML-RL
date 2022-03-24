using Code.Infrastructure.Assets;
using Code.Logic.Balance;
using UnityEngine;
using Zenject;

namespace Code.Logic.Agents
{
    public class AgentMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        private IAssetProvider _assetProvider;
        private AircraftData _data;
        
        [Inject]
        private void Construct(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        private void Start()
        {
            _data = _assetProvider.Load<AircraftData>(AssetPath.AircraftData);
        }
                
        public void Movement()
        {
            float boostModifier = 1f;
            
            _rigidbody.AddForce(transform.forward * _data.Thrust * boostModifier, ForceMode.Force);
        }

        public void Rotate()
        {
            
        }
    }
}