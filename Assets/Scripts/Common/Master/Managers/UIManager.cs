using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : IManager
{
	[SerializeField]
	private List<IPanel> _panel = new List<IPanel>();

	public override void Initialize()
	{
		foreach (IPanel panel in _panel)
		{
			panel.Initialize();
		}
	}
}
