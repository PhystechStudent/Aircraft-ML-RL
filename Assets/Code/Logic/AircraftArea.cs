using Code.Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Code.Logic
{
    public class AircraftArea : MonoBehaviour
    {
        
        private ICheckPointsFactory _checkPointsFactory;

        [Inject]
        private void Construct(ICheckPointsFactory checkPointsFactory)
        {
            _checkPointsFactory = checkPointsFactory;
        }
        
        
    }
}