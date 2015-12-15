using UnityEngine;
using System.Collections;

public enum CameraStyleType
{
	FIRST,
	THIRD
}

public class PlayerCamera : MonoBehaviour
{
	
	private CameraStyleType cameraType = CameraStyleType.THIRD;
	public CameraStyleType CameraType = CameraStyleType.THIRD;
	
	public float LookSpeed = 10;
	
	public Transform ThirdPersonHorizontalAnchor;
	public Transform ThirdPersonVerticalAnchor;
	
	public GameObject CameraTarget;
	
	private Vector3 inputVector = Vector3.zero;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if( CameraTarget == null ) return;
		
		inputVector.x = Input.GetAxis( "Mouse X" );
		inputVector.y = Input.GetAxis( "Mouse Y" );
		
		if( cameraType != CameraType )
			SetCameraType( CameraType );
			
		switch( cameraType )
		{
			case CameraStyleType.FIRST:
				updateFirstPersonCamera();
			break;
			
			case CameraStyleType.THIRD:
				updateThirdPersonCamera();
			break;
		}
				
	}
	
	public void SetCameraType( CameraStyleType type )
	{	
		if( type == cameraType ) return;
		
		cameraType = type;
		
		if( type != CameraType )
			CameraType = type;
	}
	
	private void updateFirstPersonCamera ()
	{
		
	}

	private void updateThirdPersonCamera ()
	{
		CameraTarget.transform.Rotate( new Vector3(0, inputVector.x * LookSpeed, 0) );
		ThirdPersonVerticalAnchor.Rotate( new Vector3(-inputVector.y * LookSpeed, 0, 0) );		
	}
}
