using Code.Infrastructure.Assets;
using Code.Logic.Interactions;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
    public class CheckPointsFactory : ICheckPointsFactory
    {
        private readonly IAssetProvider _assetProvider;

        private CheckPoint _checkPointPrefab;
        private FinishCheckPoint _finishCheckPointPrefab;

        public CheckPointsFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            Load();
        }

        private void Load()
        {
            _checkPointPrefab = _assetProvider.Load<CheckPoint>(AssetPath.CheckPointPrefab);
            _finishCheckPointPrefab = _assetProvider.Load<FinishCheckPoint>(AssetPath.FinishCheckPointPrefab);
        }

        public CheckPoint CreateCheckPoint() => Object.Instantiate(_checkPointPrefab);

        public FinishCheckPoint CreateFinishCheckPoint() => Object.Instantiate(_finishCheckPointPrefab);
    }
}