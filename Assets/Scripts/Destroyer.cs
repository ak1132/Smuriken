using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	void OnTriggerExit(Collider collider)
	{
		collider.gameObject.SetActive (false);
	}
}
