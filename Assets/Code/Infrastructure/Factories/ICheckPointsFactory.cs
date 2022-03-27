using Code.Logic.Interactions;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
    public interface ICheckPointsFactory
    {
        CheckPoint CreateCheckPoint();
        FinishCheckPoint CreateFinishCheckPoint();
    }
}