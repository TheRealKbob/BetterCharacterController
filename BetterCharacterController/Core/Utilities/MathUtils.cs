using UnityEngine;
using System.Collections;

namespace BetterCharacterController
{

	public class MathUtils
	{
	
		public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
		{
			Vector3 a = target - current;
			float magnitude = a.magnitude;
			return current + a / magnitude * maxDistanceDelta;
		}
		
	}

}