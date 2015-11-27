using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class BetterCharacterController : MonoBehaviour
	{
	
		public Vector3 DebugMove = Vector3.zero;
	
		private const int MAX_PUSHBACK_DEPTH = 2;
	
		[SerializeField]
		private float gravityScale = 1;
		public float GravityScale{ get { return gravityScale; } }
		private float gravitationalAcceleration = 9.81f;
		private float gravity{  get { return gravityScale * gravitationalAcceleration; } }
		
		[SerializeField]
		private float speed = 1f;
		public float Speed{ get{ return speed; } }
		
		[SerializeField]
		private float acceleration = 30f;
		public float Acceleration{ get{ return acceleration; } }
		
		[SerializeField]
		private float jumpHeight = 1f;
		public float JumpHeight{ get{ return jumpHeight; } }
		
		[SerializeField]
		private LayerMask environmentLayer;
		public LayerMask EnvironmentLayer{ get { return environmentLayer; } }
		
		public float Radius = 0.5f;
		
		public Vector3 Position{ get{ return transform.position; } }
		public Vector3 Up{ get{ return transform.up; } }
		
		[SerializeField]
		private Vector3 velocity = Vector3.zero;
		public Vector3 Velocity{ get{ return velocity; } }
		
		private ControllerStateMachine stateMachine;

		private GroundController groundController;
		
		public Vector3 moveVector = Vector3.zero;
		private Vector3 lastPosition;
		private Vector3 lastGroundPosition = Vector3.zero;

		private bool isClamping = false;
		private GameObject clampedTo = null;

		void Awake ()
		{
			stateMachine = new ControllerStateMachine( this );
			groundController = new GroundController ( this, environmentLayer );
		}
		
		void Start()
		{
			lastPosition = Position;
		}

		void FixedUpdate ()
		{
		
			if( isClamping && clampedTo != null )
				updateGroundClamping();
			
			groundController.Probe();
			
			transform.position += moveVector * Time.deltaTime;
			
			stateMachine.DoUpdate();
			
			recursivePushback(0, MAX_PUSHBACK_DEPTH);
			
			groundController.Probe();
			
			velocity = ( transform.position - lastPosition ) / Time.deltaTime;
			lastPosition = transform.position;
			if( isClamping )
			{
				lastGroundPosition = clampedTo.transform.position;
			}
		}

		private void updateGroundClamping ()
		{
			transform.position += clampedTo.transform.position - lastGroundPosition;
		}
		
		private void clampToGround()
		{
			transform.position = new Vector3( transform.position.x, groundController.CurrentGround.ControllerPoint.y, transform.position.z );
		}

		private void recursivePushback ( int depth, int maxDepth )
		{
			bool contact = false;
			foreach( Collider c in Physics.OverlapSphere( Position, Radius, EnvironmentLayer ) )
			{
				if( c.isTrigger || c.gameObject == groundController.CurrentGround.GroundObject )
					continue;
					
				contact = true;
				Vector3 contactPoint = c.ClosestPointOnBounds( Position );
				DebugDraw.DrawMarker( contactPoint, 0.25f, Color.cyan, 0.1f, false );
				Vector3 v = Position - contactPoint;
				transform.position += Vector3.ClampMagnitude( v, Mathf.Clamp( Radius - v.magnitude, 0, Radius ) );
			}
			
			if ( depth < maxDepth && contact )
			{
				recursivePushback(depth + 1, maxDepth);
			}
		}		
		
		public bool MaintainingGround()
		{
		
			bool maintainingGround = groundController.IsGrounded( true );
			if( maintainingGround )
			{
				clampedTo = groundController.CurrentGround.GroundObject;
				clampToGround();
			}
			else
			{
				isClamping = false;
			}		
		
			return maintainingGround;
		}

		public bool AcquiringGround()
		{
			
			bool acquiredGround = groundController.IsGrounded( false );
			if( acquiredGround )
			{
				moveVector = new Vector3( moveVector.x, 0, moveVector.z );
				isClamping = true;
				clampedTo = groundController.CurrentGround.GroundObject;
				Vector3 cp = new Vector3( transform.position.x, groundController.CurrentGround.ControllerPoint.y, transform.position.z );
				transform.position = cp;
			}
			
			return acquiredGround;
		}

		public void Move( Vector2 move ){ Move( move.x, move.y ); }
		public void Move( Vector3 move ){ Move( move.x, move.z ); }
		public void Move( float x, float z )
		{
			Vector3 forward = transform.forward;
			Vector3 right = transform.right;
			Vector3 local = Vector3.zero;
			
			if( x != 0 )
				local += right * x;
			if( z != 0 )
				local += forward * z;
				
			Vector3 m = Vector3.MoveTowards( moveVector, local.normalized * Speed, Acceleration * Time.deltaTime );
			moveVector = new Vector3( m.x, moveVector.y, m.z );
		}

		public void AddGravity()
		{
			moveVector -= new Vector3 ( 0, gravity * Time.deltaTime, 0 );
		}
		
		public void AddJump()
		{
			isClamping = false;
			moveVector += Up * Mathf.Sqrt( 2 * jumpHeight * gravity );
		}
		
		public void ClearForce()
		{
			moveVector = Vector3.zero;
		}

	}

}