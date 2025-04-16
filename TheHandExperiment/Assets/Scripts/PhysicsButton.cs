using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class ChangeColorEvent : UnityEvent<ChangeColor.ColorType> { }
[System.Serializable]
public class ChangeObjectEvent : UnityEvent<GameObject> { }
public class PhysicsButton : MonoBehaviour
{
	public Rigidbody buttonTopRigid;
	public Transform buttonTop;
	public Transform buttonLowerLimit;
	public Transform buttonUpperLimit;
	public float threshHold;
	public float force = 10;
	private float upperLowerDiff;
	public bool isPressed = false;
	private bool prevPressedState = false;
	private bool isFastMoveDetected = false;
	private bool prevFastMoveState = false;
	private bool isActivated = false;
	private bool prevActivatedState = false;
	
	private int fastmoveColliderCount = 0;
	public AudioSource pressedSound;
	public Collider[] CollidersToIgnore;
	public UnityEvent onPressed;
	public UnityEvent onReleased;
	public ChangeColorEvent onPressedColor;
	public ChangeObjectEvent onReleasedObject;
	
	[SerializeField] public 
    ChangeColor.ColorType color; // Utilisation de l'enum ColorType

	[SerializeField]
	GameObject cube; // Référence au cube à changer
	

	[SerializeField]
	Material red;
	
	[SerializeField]
	Material yellow;
	
	[SerializeField]
	Material gray;

	private Renderer buttonRenderer;


	// Start is called before the first frame update
	void Start()
	{
		Collider localCollider = GetComponent<Collider>();
		if (localCollider != null)
		{
			Physics.IgnoreCollision(localCollider, buttonTop.GetComponentInChildren<Collider>());

			foreach (Collider singleCollider in CollidersToIgnore)
			{
				Physics.IgnoreCollision(localCollider, singleCollider);
			}
		}

		if (transform.eulerAngles != Vector3.zero)
		{
			Vector3 savedAngle = transform.eulerAngles;
			transform.eulerAngles = Vector3.zero;
			upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;
			transform.eulerAngles = savedAngle;
		}
		else
			upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;

		buttonRenderer = buttonTop.gameObject.GetComponent<Renderer>();
	}

	// Update is called once per frame
	void Update()
	{
		buttonTop.transform.localPosition = new Vector3(0, buttonTop.transform.localPosition.y, 0);
		buttonTop.transform.localEulerAngles = new Vector3(0, 0, 0);
		if (buttonTop.localPosition.y >= 0)
			buttonTop.transform.position = new Vector3(buttonUpperLimit.position.x, buttonUpperLimit.position.y, buttonUpperLimit.position.z);
		else
			buttonTopRigid.AddForce(buttonTop.transform.up * (force * Time.deltaTime));

		if (buttonTop.localPosition.y <= buttonLowerLimit.localPosition.y)
			buttonTop.transform.position = new Vector3(buttonLowerLimit.position.x, buttonLowerLimit.position.y, buttonLowerLimit.position.z);



		buttonRenderer.material = gray;
		prevPressedState = isPressed;
		prevFastMoveState = isFastMoveDetected;
		prevActivatedState = isActivated;

		if (Vector3.Distance(buttonTop.position, buttonLowerLimit.position) < upperLowerDiff * threshHold)
		{
			isPressed = true;
		}
		else
		{
			isPressed = false;
		}


		if (
			// (isPressed && !prevPressedState) 
		 //    ||
			(isFastMoveDetected /*&& !isPressed*/ && !prevFastMoveState)
		    )
		{
			// JustActivated();
			isActivated = true;
		}
		
		if (
			// (!isPressed && prevPressedState 
	  //               && !isFastMoveDetected) 
		 //    ||
			(!isFastMoveDetected && prevFastMoveState )) 
		                            // && !isPressed))
		{
			// JustReleased();
			isActivated = false;
		}
		
		if(isActivated && isActivated != prevActivatedState)
			JustActivated();
		if(!isActivated && isActivated != prevActivatedState)
			JustReleased();
	}

	void JustActivated()
	{
		buttonRenderer.material = yellow;
		pressedSound.pitch = 1;
		pressedSound.Play();
		
		onPressed.Invoke();
		onPressedColor.Invoke(color);
	}

	void JustReleased()
	{
		buttonRenderer.material = red;
		// onReleased.Invoke();
		// onReleasedObject.Invoke(gameObject);
	}

	public void FastMoveEntered()
	{
		if (fastmoveColliderCount == 0)
		{
			isFastMoveDetected = true;
			Debug.Log("true");
		}

		fastmoveColliderCount++;
	}

	public void FastMoveExited()
	{
		if (fastmoveColliderCount == 1)
		{
			isFastMoveDetected = false;
			Debug.Log("false");
			
		}

		fastmoveColliderCount--;
	}

	private void OnDisable()
	{
		isFastMoveDetected = false;
		prevFastMoveState = false;
		isPressed = false;
		prevPressedState = false;
		isActivated = false;
		prevActivatedState = false;
		buttonTop.transform.localPosition = new Vector3(0, buttonUpperLimit.position.y, 0);
	}
}
