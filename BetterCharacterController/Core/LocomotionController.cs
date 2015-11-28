using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class LocomotionController
	{

		private BetterCharacterController controller;
		private GroundController groundController;

		private Transform transform;

		private Vector3 moveForce = Vector3.zero;

		#region Clamping Properties
		private GameObject clampedTo;
		private Vector3 lastGroundPosition = Vector3.zero;
		#endregion

		#region Physics Simulation
		private Vector3 velocity = Vector3.zero;
		public Vector3 Velocity{ get{ return velocity; } }
		private Vector3 lastPosition;
		#endregion

		#region Pointers
		public float Radius{ get{ return controller.Radius; } }
		public LayerMask EnvironmentLayer{ get{ return controller.EnvironmentLayer; } }
		public Vector3 Position{ get{ return transform.position; } }
		public Vector3 Up{ get{ return transform.up; } }
		#endregion

		private bool isClamping = false;
		public bool ClampingEnabled{ get{ return controller.EnableGroundClamping; } }

		private const int MAX_PUSHBACK_DEPTH = 2;

		public LocomotionController( BetterCharacterController controller )
		{
			this.controller = controller;
			transform = controller.transform;
			groundController = new GroundController( this, EnvironmentLayer );
		}

		public void UpdatePhase( int phase )
		{
			switch( phase )
			{
				case 1:
				updateGroundClamping();
				groundController.Probe();
				transform.position += moveForce * Time.deltaTime;
				break;

				case 2:
				recursivePushback( 0, MAX_PUSHBACK_DEPTH );
				groundController.Probe();
				velocity = ( transform.position - lastPosition ) / Time.deltaTime;
				lastPosition = transform.position;
				if( controller.EnableGroundClamping && clampedTo != null )
					lastGroundPosition = clampedTo.transform.position;
				break;

				default:
				Debug.LogError( "Updating a locomotion phase that doesn't exist are we? Tsk Tsk..." );
				break;
			}
		}

		#region Movement Functions
		public void ApplyForce( Vector3 force )
		{
			if( force.x != 0 || force.z != 0 )
				moveForce = addHorizontalForce( force.x, force.z );
			
			if( force.y > 0 )
				moveForce += addPositiveVerticalForce( force.y );

			if( force.y < 0 )
				moveForce -= addNegativeVerticalForce( force.y );
		}

		public void ApplyFriction()
		{
			moveForce = new Vector3( moveForce.x * controller.Friction, moveForce.y, moveForce.z * controller.Friction );
		}

		private Vector3 addHorizontalForce( float x, float z )
		{
			Vector3 forward = transform.forward;
			Vector3 right = transform.right;
			Vector3 local = Vector3.zero;
			local += right * x;
			local += forward * z;
			
			Vector3 m = Vector3.MoveTowards( moveForce, local.normalized * controller.Speed, controller.Acceleration * Time.deltaTime );
			return new Vector3( m.x, moveForce.y, m.z );
		}

		private Vector3 addPositiveVerticalForce( float y )
		{
			return Up * Mathf.Sqrt( 2 * controller.JumpHeight * controller.Gravity );
		}
		
		private Vector3 addNegativeVerticalForce( float y )
		{
			return new Vector3 ( 0, controller.Gravity * Time.deltaTime, 0 );
		}
		#endregion

		public bool MaintainingGround()
		{
			
			bool maintainingGround = groundController.IsGrounded( true );
			if( maintainingGround )
			{
				clampedTo = groundController.CurrentGround.GroundObject;
				clampToGround();
			}
			else
				isClamping = false;
			
			return maintainingGround;
			
		}

		public bool AcquiringGround()
		{
			
			bool acquiredGround = groundController.IsGrounded( false );
			if( acquiredGround )
			{
				moveForce = new Vector3( moveForce.x, 0, moveForce.z );
				isClamping = true;
				clampedTo = groundController.CurrentGround.GroundObject;
				Vector3 cp = new Vector3( transform.position.x, groundController.CurrentGround.ControllerPoint.y, transform.position.z );
				transform.position = cp;
			}
			
			return acquiredGround;
		}

		private void updateGroundClamping()
		{
			if( !controller.EnableGroundClamping || clampedTo == null ) return;
			transform.position += clampedTo.transform.position - lastGroundPosition;
		}

		private void clampToGround()
		{
			transform.position = new Vector3( transform.position.x, groundController.CurrentGround.ControllerPoint.y, transform.position.z );
		}

		private void recursivePushback ( int depth, int maxDepth )
		{
			bool contact = false;
			foreach( Collider c in Physics.OverlapSphere( transform.position, controller.Radius, controller.EnvironmentLayer ) )
			{
				if( c.isTrigger ) continue;

				contact = true;
				Vector3 contactPoint = Vector3.zero;
				if (c is BoxCollider)
				{
					contactPoint = ClosestPointOn((BoxCollider)c, transform.position);
				}
				else if (c is SphereCollider)
				{
					contactPoint = ClosestPointOn((SphereCollider)c, transform.position);
				}
				DebugDraw.DrawMarker( contactPoint, 0.5f, Color.cyan, 0.1f, false );
				if( c.gameObject == clampedTo )
					controller.GroundAngle = Mathf.Abs( 90 - Vector3.Angle( transform.up, contactPoint ) );
				Vector3 v = transform.position - contactPoint;
				transform.position += Vector3.ClampMagnitude( v, Mathf.Clamp( controller.Radius - v.magnitude, 0, controller.Radius ) );
			}
			
			if ( depth < maxDepth && contact )
			{
				recursivePushback(depth + 1, maxDepth);
			}
		}

		//TEMP
		Vector3 ClosestPointOn(BoxCollider collider, Vector3 to)
		{
			if (collider.transform.rotation == Quaternion.identity)
			{
				return collider.ClosestPointOnBounds(to);
			}
			
			return closestPointOnOBB(collider, to);
		}
		
		Vector3 ClosestPointOn(SphereCollider collider, Vector3 to)
		{
			Vector3 p;
			
			p = to - collider.transform.position;
			p.Normalize();
			
			p *= collider.radius * collider.transform.localScale.x;
			p += collider.transform.position;
			
			return p;
		}
		
		Vector3 closestPointOnOBB(BoxCollider collider, Vector3 to)
		{
			// Cache the collider transform
			var ct = collider.transform;
			
			// Firstly, transform the point into the space of the collider
			var local = ct.InverseTransformPoint(to);
			
			// Now, shift it to be in the center of the box
			local -= collider.center;
			
			// Inverse scale it by the colliders scale
			var localNorm =
				new Vector3(
					Mathf.Clamp(local.x, -collider.size.x * 0.5f, collider.size.x * 0.5f),
					Mathf.Clamp(local.y, -collider.size.y * 0.5f, collider.size.y * 0.5f),
					Mathf.Clamp(local.z, -collider.size.z * 0.5f, collider.size.z * 0.5f)
					);
			
			// Now we undo our transformations
			localNorm += collider.center;
			
			// Return resulting point
			return ct.TransformPoint(localNorm);
		}

	}

}