using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class GroundController
	{
	
		private LocomotionController controller;
		private LayerMask environmentLayer;

		public GroundController( LocomotionController controller, LayerMask environmentLayer )
		{
			this.controller = controller;
			this.environmentLayer = environmentLayer;
		}

	}
}
