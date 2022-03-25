using UnityEngine;

namespace Code.Infrastructure.Factories
{
    public interface ICheckPointsFactory
    {
        GameObject CreateCheckPoint();
        GameObject CreateFinishCheckPoint();
    }
}