using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BetterCharacterControllerFramework
{
	
	public enum ControllerStateType
	{
		NONE,
		IDLE,
		MOVING,
		FALLING,
		JUMPING,
		SLIDING	
	}
	
	
	public class ControllerStateMachine
	{		
		private ControllerState state = new ControllerState(null, null, null);	
		private Dictionary<ControllerStateType, ControllerState> stateMap = new Dictionary<ControllerStateType, ControllerState>();
		
		public ControllerStateType CurrentState
		{
			get
			{
				return state.ID;
			}
			
			set
			{
				state.ExitState();
				state = getState( value );
				state.EnterState();
			}
		}
		
		public ControllerStateMachine( CharacterMotor controller, LocomotionController locomotion )
		{
			addState( ControllerStateType.IDLE, new ControllerIdleState( this, controller, locomotion ) );
			addState( ControllerStateType.MOVING, new ControllerMovingState( this, controller, locomotion ) );
			addState( ControllerStateType.FALLING, new ControllerFallingState( this, controller, locomotion ) );
			addState( ControllerStateType.JUMPING, new ControllerJumpingState( this, controller, locomotion ) );
			addState( ControllerStateType.SLIDING, new ControllerSlideState( this, controller, locomotion ) );
			
			CurrentState = ControllerStateType.IDLE;
		}

		public void DoUpdate()
		{
			state.OnUpdate();
		}
		
		private void addState( ControllerStateType t, ControllerState s )
		{
			stateMap.Add( t, s );
			s.ID = t;
		}
		
		private ControllerState getState( ControllerStateType type )
		{
			return stateMap[ type ];
		}

	}
	
	public class ControllerState
	{
		protected ControllerStateMachine stateMachine;
		protected CharacterMotor controller;
		protected LocomotionController locomotion;

		public ControllerStateType ID;
	
		public ControllerState( ControllerStateMachine stateMachine, CharacterMotor controller, LocomotionController locomotion )
		{
			this.stateMachine = stateMachine;
			this.controller = controller;
			this.locomotion = locomotion;
		}
		
		public virtual void EnterState(){}
		public virtual void OnUpdate(){}
		public virtual void ExitState(){}
		
	}
	
}