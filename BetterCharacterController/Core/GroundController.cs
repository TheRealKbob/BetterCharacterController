using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class GroundController
	{
	
		private LocomotionController controller;
		private LayerMask environmentLayer;
		
		private Ground ground;
		public Ground CurrentGround{ get{ return ground; } }

		private float probeHeight = 0;

		public GroundController( LocomotionController controller, LayerMask environmentLayer )
		{
			this.controller = controller;
			this.environmentLayer = environmentLayer;
		}

		public bool IsGrounded( bool isClamping )
		{
		
			Probe();
		
			if( ground != null )
			{			
				float tolerance = ( isClamping && controller.ClampingEnabled ) ? 0.3f : 0;
				float groundDist = Vector3.Distance( controller.Position, ground.Point );
				float tSize = controller.Radius + tolerance + 0.1f;
				//Debug.Log( "Ground dist: " + groundDist + ", tsize: " + tSize );
				if( groundDist <= tSize )
				{
					return true;
				}
			}
			return false;
		}
		
		public void Clear()
		{
			ground = null;
		}
		
		public void Probe()
		{
			
			Clear();
			
			probeHeight = controller.Position.y + controller.Radius;
			Vector3 o = new Vector3( controller.Position.x, probeHeight, controller.Position.z );
			
			DebugDraw.DrawMarker( o, 0.25f, Color.green, 0, false );
			
			float smallerRadius = controller.Radius * 0.98f;
			
			RaycastHit hit;
			if( Physics.SphereCast( o, smallerRadius, -controller.Up, out hit, Mathf.Infinity, environmentLayer ) )
			{
				Vector3 p = MathUtils.MoveTowards( hit.point, controller.Position, controller.Radius );
				float groundAngle = Vector3.Angle( hit.normal, Vector3.up );
												
				ground = new Ground( hit.point, p, Vector3.right + hit.normal, hit.distance, groundAngle, hit.collider.gameObject );
				DebugDraw.DrawMarker( hit.point, 0.25f, Color.red, 0, false );
				DebugDraw.DrawMarker( p, 0.25f, Color.blue, 0, false );
			}
			
		}
		
	}
	
	public class Ground
	{
		private Vector3 point;
		public Vector3 Point{ get{ return point; } }
		
		private Vector3 controllerPoint;
		public Vector3 ControllerPoint{ get{ return controllerPoint; } }
		
		private Vector3 normal;
		public Vector3 Normal{ get{ return normal; } }
		
		private float distance;
		public float Distance{ get{ return distance; } }

		private float angle;
		public float Angle{ get{ return angle; } }

		private GameObject groundObject;
		public GameObject GroundObject{ get{ return groundObject; } }
		
		public float DistanceToGround( Vector3 position )
		{
			return Vector3.Distance( position, controllerPoint );
		}
		
		public Ground( Vector3 point, Vector3 controllerPoint, Vector3 normal, float distance, float angle, GameObject groundObject )
		{
			this.point = point;
			this.controllerPoint = controllerPoint;
			this.normal = normal;
			this.distance = distance;
			this.angle = angle;
			this.groundObject = groundObject;
		}
	}

}
