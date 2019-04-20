using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationManager : IManager
{
	[SerializeField]
	private List<IOperation> _operations = new List<IOperation>();

	public override void Initialize()
	{
		foreach (IOperation operation in _operations)
		{
			operation.Initialize();
		}
	}
}
