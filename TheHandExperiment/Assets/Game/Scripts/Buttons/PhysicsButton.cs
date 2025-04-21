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
	public AudioSource pressedSound;
	public Collider[] CollidersToIgnore;
	public UnityEvent onPressed;
	public UnityEvent onReleased;
	public ChangeColorEvent onPressedColor;
	public ChangeObjectEvent onReleasedObject;

	[SerializeField]
	public
	ChangeColor.ColorType color; // Utilisation de l'enum ColorType

	[SerializeField]
	GameObject cube; // Référence au cube à changer

	// Initialise les collisions à ignorer et calcule la hauteur du bouton
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

	// Met à jour la position du bouton et détecte s'il est pressé ou relâché
	void Update()
	{
		buttonTop.transform.localPosition = new Vector3(0, buttonTop.transform.localPosition.y, 0);
		buttonTop.transform.localEulerAngles = new Vector3(0, 0, 0);
		if (buttonTop.localPosition.y >= 0)
			buttonTop.transform.position = new Vector3(buttonUpperLimit.position.x, buttonUpperLimit.position.y, buttonUpperLimit.position.z);
		else
			buttonTopRigid.AddForce(buttonTop.transform.up * force * Time.deltaTime);

		if (buttonTop.localPosition.y <= buttonLowerLimit.localPosition.y)
			buttonTop.transform.position = new Vector3(buttonLowerLimit.position.x, buttonLowerLimit.position.y, buttonLowerLimit.position.z);


		if (Vector3.Distance(buttonTop.position, buttonLowerLimit.position) < upperLowerDiff * threshHold)
			isPressed = true;
		else
			isPressed = false;

		if (isPressed && prevPressedState != isPressed)
			Pressed();
		if (!isPressed && prevPressedState != isPressed)
			Released();
	}

	// Gère l'événement lorsqu'on presse le bouton
	void Pressed()
	{
		prevPressedState = isPressed;
		onPressed.Invoke();
		onPressedColor.Invoke(color);

		pressedSound.pitch = 1;
		pressedSound.Play();
	}

	// Gère l'événement lorsqu'on relâche le bouton
	void Released()
	{
		prevPressedState = isPressed;
		onReleased.Invoke();
		onReleasedObject.Invoke(gameObject);
	}
}
