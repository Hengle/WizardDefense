﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Momiji;
using UniRx;
using UnityEngine;
using Zenject;

namespace WizardDefense
{
	public class King : MonoBehaviour
	{

		[Inject (Id = "PlayerCastle")]
		private Castle _playerCastle;
		[Inject (Id = "EnemyCastle")]
		private Castle _enemyCastle;

        [SerializeField]
        private Color _color;

		[SerializeField]
		private double _sortieInterval;
		[SerializeField]
		private Formation[] _formations;

		private Formation _current;
		private int _currentFormationIndex;
		private List<Platoon> _platoons = new List<Platoon> ();

		// Use this for initialization
		void Start ()
		{
			NextPlatoon ();

			Bind ();
		}

		private void Bind ()
		{
			Observable
				.Interval (TimeSpan.FromSeconds (_sortieInterval))
				.Subscribe (_ =>
				{
					if (_current.Point.Length == _currentFormationIndex)
					{
						NextPlatoon ();
					}
					if (_currentFormationIndex == 0)
					{
						var pos = Vector3.zero.RandomX (-10, 10) + Vector3.zero.RandomZ (-10, 10);
						pos.y = 1f;
						_playerCastle.Sortie (_current, _currentFormationIndex, _platoons.Last (), pos, color: _color);
					}
					else
					{
						_playerCastle.Sortie (_current, _currentFormationIndex, _platoons.Last (), color: _color);
					}
					_currentFormationIndex += 1;
				})
				.AddTo (this);
		}

		// Update is called once per frame
		void Update ()
		{

		}

		private void NextPlatoon ()
		{
			var nextFormation = _formations.RandomValue ();
			AddPlatoon (nextFormation, _platoons.Count);
			_current = nextFormation;
			_currentFormationIndex = 0;
		}

		private void AddPlatoon (Formation formation, int platoonsCount)
		{
			_platoons.Add (new Platoon (formation, platoonsCount));
		}
	}
}