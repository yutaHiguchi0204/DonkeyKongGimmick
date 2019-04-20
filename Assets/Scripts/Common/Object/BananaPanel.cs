using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using TMPro;

public class BananaPanel : PanelRoot
{
	private const int BANANA_MAX_NUM = 100;

	[SerializeField]
	private TextMeshProUGUI _bananaNumText;

	[SerializeField]
	private bool _isBonusPanel = false;

	private ReactiveProperty<int> _bananaNum = new ReactiveProperty<int>(0);
	public IReadOnlyReactiveProperty<int> BananaNum
	{
		get
		{
			return _bananaNum;
		}
	}

	public override void Initialize()
	{
		base.Initialize();

		Bind();
	}

	private void Bind()
	{
		_bananaNum
			.Subscribe(num =>
			{
				_bananaNumText.text = _bananaNum.ToString();
			});
	}

	protected override void GetAnimation(Image item2D)
	{
		item2D.rectTransform.DOMove(transform.position, MOVE_TIME)
			.OnComplete(() =>
			{
				Destroy(item2D.gameObject);
				if (_isBonusPanel)
				{
					SubBanana();
					return;
				}
				AddBanana();
			});
	}

	public void AddBanana()
	{
		_bananaNum.Value++;
	}

	public void SubBanana()
	{
		_bananaNum.Value--;
	}

	public void SetBanana(int num)
	{
		_bananaNum.Value = num; 
	}
}
