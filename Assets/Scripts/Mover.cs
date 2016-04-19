using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public float speed;
	public Vector3 directionTo;

	private Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody> ();
//		rb.velocity = directionTo * speed;
	}

	public void SetDirectionAndSpeed(Vector3 direction,float speed)
	{
		rb.velocity = direction * speed;
		//Debug.Log (direction + " " + speed);
	}

//	void OnDisable() {
//		speed = 0.0f;
//		directionTo = Vector3.zero;
////		rb.velocity = Vector3.zero;
//		SetDirectionAndSpeed (directionTo, speed);
//	}

}
