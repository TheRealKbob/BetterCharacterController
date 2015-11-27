using UnityEngine;
using System.Collections;

namespace BetterCharacterController
{

	public class BetterCharacterController : MonoBehaviour
	{
	
		public Vector3 DebugMove = Vector3.zero;

		[SerializeField]
		private float gravityScale = 1;
		public float GravityScale{ get { return gravityScale; } }
		private float gravitationalAcceleration = 9.81f;
		private float gravity{  get { return gravityScale * gravitationalAcceleration; } }

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
		
			moveVector += DebugMove * Time.deltaTime;
			
			groundController.Probe();
			
			transform.position += moveVector * Time.deltaTime;
			
			stateMachine.DoUpdate();
			
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

		public void AddGravity()
		{
			moveVector -= new Vector3 ( 0, gravity * Time.deltaTime, 0 );
		}

	}

}