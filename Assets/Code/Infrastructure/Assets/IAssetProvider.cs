using UnityEngine;

namespace Code.Infrastructure.Assets
{
    public interface IAssetProvider
    {
        T Load<T>(string path) where T : Object;
    }
}