using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TakeBanana : IOperation
{
	private float SPAWN_INTERVAL_TIME = 2f;

	[SerializeField]
	private int _takeNum = 15;

	[SerializeField]
	private Item _itemPrefab;

	[SerializeField]
	private BananaPanel _bananaPanel;

	[SerializeField]
	private List<Vector2> _spawnPos = new List<Vector2>();

	IDisposable _bonusDisposable;

	public override void Initialize()
	{
		_bananaPanel.SetBanana(_takeNum);

		_bonusDisposable = Observable.Interval(TimeSpan.FromSeconds(Item.DISAPPEAR_START_TIME + SPAWN_INTERVAL_TIME))
			.Subscribe(time =>
			{
				int posID = UnityEngine.Random.Range(0, _spawnPos.Count);
				Item banana = Instantiate(_itemPrefab, _spawnPos[posID], Quaternion.identity);
				banana.SetBonus(ItemName.ItemNameList.BananaGreen, _bananaPanel, true, true);
			});

		Bind();
	}

	private void Bind()
	{
		_bananaPanel.BananaNum
			.Where(num => num <= 0)
			.Subscribe(num =>
			{
				Item bonusCoin = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
				bonusCoin.Initialize(ItemName.ItemNameList.BonusCoin, true);
				_bonusDisposable.Dispose();
			});
	}
}
