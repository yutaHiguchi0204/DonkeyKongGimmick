using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class PlayerLife : PanelRoot
{
	private const int START_PLAYER_LIFE = 5;

    [SerializeField]
    private TextMeshProUGUI _lifeText;

    [SerializeField]
    private ReactiveProperty<int> _life = new ReactiveProperty<int>(START_PLAYER_LIFE);

	public override void Initialize()
	{
		base.Initialize();

		Bind();
	}

	private void Bind()
	{
		_life
			.Subscribe(life =>
			{
				_lifeText.text = life.ToString();
			});
	}

	public void AddLife()
    {
        _life.Value++;
    }

	public void SubLife()
	{
		_life.Value--;
	}
}
