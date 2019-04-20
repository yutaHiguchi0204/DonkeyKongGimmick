using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	[SerializeField]
	private List<IManager> _managers = new List<IManager>();

	private void Awake()
	{
		foreach (IManager manager in _managers)
		{
			manager.Initialize();
		}
	}
}
