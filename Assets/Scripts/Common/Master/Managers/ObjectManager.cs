using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : IManager
{
	[SerializeField]
	private List<IObject> _objects = new List<IObject>();

	public override void Initialize()
	{
		foreach (IObject obj in _objects)
		{
			obj.Initialize();
		}
	}
}
