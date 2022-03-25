using Code.Infrastructure.Assets;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
    public class CheckPointsFactory : ICheckPointsFactory
    {
        private readonly IAssetProvider _assetProvider;

        private GameObject _checkPointPrefab;
        private GameObject _finishCheckPointPrefab;

        public CheckPointsFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            Load();
        }

        private void Load()
        {
            _checkPointPrefab = _assetProvider.Load<GameObject>(AssetPath.CheckPointPrefab);
            _finishCheckPointPrefab = _assetProvider.Load<GameObject>(AssetPath.FinishCheckPointPrefab);
        }

        public GameObject CreateCheckPoint() => Object.Instantiate(_checkPointPrefab);

        public GameObject CreateFinishCheckPoint() => Object.Instantiate(_finishCheckPointPrefab);
    }
}