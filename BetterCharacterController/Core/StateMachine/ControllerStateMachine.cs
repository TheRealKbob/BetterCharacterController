using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BetterCharacterController
{
	
	public enum ControllerStateType
	{
		NONE,
		IDLE,
		MOVING,
		FALLING,
		JUMPING		
	}
	
	
	public class ControllerStateMachine
	{
		
		private BetterCharacterController controller;
		
		private ControllerState state = new ControllerState();
		
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
		
		public ControllerStateMachine( BetterCharacterController controller )
		{
		
			this.controller = controller;
		
			addState( ControllerStateType.IDLE, new ControllerIdleState( this, controller ) );
			addState( ControllerStateType.MOVING, new ControllerMovingState( this, controller ) );
			addState( ControllerStateType.FALLING, new ControllerFallingState( this, controller ) );
			addState( ControllerStateType.JUMPING, new ControllerJumpingState( this, controller ) );
			
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
		
		public ControllerStateType ID;
	
		public ControllerState(){}
		
		public virtual void EnterState(){}
		public virtual void OnUpdate(){}
		public virtual void ExitState(){}
		
	}
	
}