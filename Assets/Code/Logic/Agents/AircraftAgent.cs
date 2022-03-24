using Code.Infrastructure.Assets;
using Code.Logic.Balance;
using Unity.MLAgents;
using Zenject;

namespace Code.Logic.Agents
{
    public class AircraftAgent : Agent
    {
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
    }
}