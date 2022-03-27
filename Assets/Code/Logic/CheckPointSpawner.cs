using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Code.Infrastructure.Factories;
using Code.Logic.Agents;
using Code.Logic.Interactions;
using UnityEngine;
using Zenject;

namespace Code.Logic
{
	public class CheckPointSpawner : MonoBehaviour
	{
		[SerializeField] private CinemachineSmoothPath _path;

		private ICheckPointsFactory _checkPointsFactory;
		private List<CheckPoint> _checkPoints;

		public IReadOnlyList<CheckPoint> CheckPoints => _checkPoints;

		[Inject]
		private void Construct(ICheckPointsFactory checkPointsFactory)
		{
			_checkPointsFactory = checkPointsFactory;
		}

		private void Start()
		{
			SpawnCheckPoints();
		}

		private void SpawnCheckPoints()
		{
			_checkPoints = new List<CheckPoint>();
			var checkPointCount = (int) _path.MaxUnit(CinemachinePathBase.PositionUnits.PathUnits);

			for (int i = 0; i < checkPointCount; i++)
			{
				CheckPoint checkpoint = i == checkPointCount - 1
					? _checkPointsFactory.CreateFinishCheckPoint()
					: _checkPointsFactory.CreateCheckPoint();

				checkpoint.transform.SetParent(_path.transform);
				checkpoint.transform.localPosition = _path.m_Waypoints[i].position;
				checkpoint.transform.rotation =
					_path.EvaluateOrientationAtUnit(i, CinemachinePathBase.PositionUnits.PathUnits);
				
				_checkPoints.Add(checkpoint);
			}
		}

		public CheckPoint GetCheckPointByIndex(int index) => _checkPoints[index];
	}
}