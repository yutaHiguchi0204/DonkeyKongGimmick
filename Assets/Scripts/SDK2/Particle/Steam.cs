using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : MonoBehaviour
{
	private void OnParticleCollision(GameObject other)
	{
		Debug.Log(other);
		HotAirBalloon balloon = other.GetComponent<HotAirBalloon>();

		if (balloon != null)
		{
			balloon.Float();
		}
	}
}
