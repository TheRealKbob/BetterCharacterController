using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class MathUtils
	{
	
		public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
		{
			Vector3 a = target - current;
			float magnitude = a.magnitude;
			return current + a / magnitude * maxDistanceDelta;
		}

		public static float AngleInPlane(Transform from, Vector3 to, Vector3 planeNormal)
		{
			Vector3 dir = to - from.position;
			
			Vector3 p1 = MathUtils.Project(dir, planeNormal);
			Vector3 p2 = MathUtils.Project(from.forward, planeNormal);
			
			return Vector3.Angle(p1, p2);
		}
		
		public static Vector3 Project(Vector3 v, Vector3 onto)
		{
			return v - (Vector3.Dot(v, onto) / Vector3.Dot(onto, onto)) * onto;
		}
		
		public static Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
		{
			return vector - (Vector3.Dot(vector, planeNormal) * planeNormal);
		}
		
		public static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point)
		{
			Vector3 linePointToPoint = point - linePoint;	
			float t = Vector3.Dot(linePointToPoint, lineVec);
			return linePoint + lineVec * t;
		}
		
	}

}