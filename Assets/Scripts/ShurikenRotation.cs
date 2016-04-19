using UnityEngine;
using System.Collections;

public class ShurikenRotation : MonoBehaviour {

	public float rotationFactor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		Quaternion rotationAngle = transform.rotation;
//		rotationAngle.z = 
//		transform.Rotate (0.0f,0.0f,Mathf.Repeat (Time.deltaTime * rotationFactor, 90.0f));
		transform.Rotate(-Vector3.forward,Time.deltaTime * rotationFactor);
	}
}
