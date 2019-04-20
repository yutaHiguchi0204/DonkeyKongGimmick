using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class HotAirBalloon : MonoBehaviour
{
	private const float MOVE_DISTANCE_ACCELERATION = 0.002f;
	private const float MOVE_DISTANCE_MAX = 0.05f;
	private const float GRAVITY = 0.015f;

	private float _moveDistance = 0f;

	private ReactiveProperty<Vector3> _balloonPos = new ReactiveProperty<Vector3>(Vector3.zero);

	private void Start()
	{
		_balloonPos.Value = new Vector3(transform.position.x, transform.position.y, 0f);

		this.UpdateAsObservable()
			.Subscribe(_ =>
			{
				if (Input.GetKey(CommonState.MOVE_LEFT_KEY))
				{
					if (_moveDistance > -MOVE_DISTANCE_MAX)
					{
						_moveDistance -= MOVE_DISTANCE_ACCELERATION;
					}
				}
				else if (Input.GetKey(CommonState.MOVE_RIGHT_KEY))
				{ 
					if (_moveDistance < MOVE_DISTANCE_MAX)
					{
						_moveDistance += MOVE_DISTANCE_ACCELERATION;
					}
				}
				else
				{
					if (_moveDistance > 0f)
					{
						_moveDistance -= MOVE_DISTANCE_ACCELERATION;
					}
					else if (_moveDistance < 0f)
					{
						_moveDistance += MOVE_DISTANCE_ACCELERATION;
					}
				}

				_balloonPos.Value += new Vector3(_moveDistance, -GRAVITY, 0f);
			});

		Bind();
	}

	private void Bind()
	{
		_balloonPos
			.Subscribe(pos =>
			{
				transform.position = pos;
			});
	}

	// 浮く
	public void Float()
	{
		_balloonPos.Value += Vector3.up * (GRAVITY * 2);
	}
}
