using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {
	public float obstacleDefaultSpeed;
	public Transform obstacleLocation;
	public GameObject destroyParticle;

	private Mover obstacleMover;
//	private Rigidbody rb;
	private GameController gameController;
//	private TrailRenderer trail;
	//also create a reference to gameManager

	// Use this for initialization
	void Awake () {
		obstacleMover = gameObject.GetComponent<Mover> ();
		gameController = (GameController)FindObjectOfType (typeof(GameController));
//		rb = gameObject.GetComponent<Rigidbody> ();
//		trail = gameObject.GetComponentInChildren<TrailRenderer> ();
	}

	void Start()
	{
//		Reset (obstacleLocation, obstacleDefaultSpeed);
	}

	// Update is called once per frame
	void Update () {

	}

	public void Reset(Transform location, float speed)
	{
		//Do more stuff if required
//		trail.Clear();
		transform.position = location.position;
//		Debug.Log ("Inside Reset" + transform.position);

		if(location.position.x >= 0)
			obstacleMover.SetDirectionAndSpeed (Vector3.left, speed);
		else
			obstacleMover.SetDirectionAndSpeed (Vector3.right, speed);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag ("Player")) 
		{
			//Do some stuff with game manager
			Instantiate (destroyParticle, collider.transform.position, Quaternion.identity);
			gameController.PlayGameOverSound ();
			collider.gameObject.SetActive(false);
			transform.position = obstacleLocation.position;
			gameController.GameOverHandler ();
			gameObject.SetActive(false);
		}
	}

	void OnDisable()
	{
		obstacleMover.SetDirectionAndSpeed (Vector3.zero, obstacleDefaultSpeed);
		gameController.DecreaseActiveObstacle ();
	}
}
