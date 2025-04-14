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
	public bool isPressed;
	private bool prevPressedState;
	private bool isFastMoveDetected = false;
	private bool prevFastMoveState;
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


		if (Vector3.Distance(buttonTop.position, buttonLowerLimit.position) < upperLowerDiff * threshHold)
			isPressed = true;
		else
			isPressed = false;
		
		
		if ((isPressed && prevPressedState != isPressed) ||
		    (isFastMoveDetected && !isPressed && prevFastMoveState != isFastMoveDetected))
			Pressed();
		if ((!isPressed && prevPressedState != isPressed) ||
		    (!isFastMoveDetected && isPressed && prevFastMoveState != isFastMoveDetected))
			Released();
	}

	void Pressed()
	{
		pressedSound.pitch = 1;
		pressedSound.Play();
		
		prevPressedState = isPressed;
		onPressed.Invoke();
		onPressedColor.Invoke(color);
	}

	void Released()
	{
		prevPressedState = isPressed;
		onReleased.Invoke();
		onReleasedObject.Invoke(gameObject);
	}

	public void FastMoveEntered()
	{
		if (fastmoveColliderCount == 0)
		{
			prevFastMoveState = isFastMoveDetected;
			isFastMoveDetected = true;
		}

		fastmoveColliderCount++;
	}

	public void FastMoveExited()
	{
		if (fastmoveColliderCount == 1)
		{
			prevFastMoveState = isFastMoveDetected;
			isFastMoveDetected = false;
		}

		fastmoveColliderCount--;
	}

	private void OnDisable()
	{
		isFastMoveDetected = false;
		prevFastMoveState = false;
		isPressed = false;
		prevPressedState = false;
		buttonTop.transform.localPosition = new Vector3(0, buttonUpperLimit.position.y, 0);
	}
}
