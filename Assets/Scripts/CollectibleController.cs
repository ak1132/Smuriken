using UnityEngine;
using System.Collections;

public class CollectibleController : MonoBehaviour {
	public float collectibleSpeed;
	public Transform collectibleLocation;
	public int value;
	public GameObject particle;

	private Mover collectibleMover;
//	private Rigidbody rb;
	private GameController gameController;
	private TrailRenderer trail;
	//also create a reference to gameManager

	// Use this for initialization
	void Awake () {
		collectibleMover = gameObject.GetComponent<Mover> ();
		gameController = (GameController)FindObjectOfType (typeof(GameController));
//		rb = gameObject.GetComponent<Rigidbody> ();
		trail = gameObject.GetComponentInChildren<TrailRenderer> ();
	}

	void Start()
	{
		//Reset (collectibleLocation, collectibleSpeed);
//		ParticleSystem.ColorOverLifetimeModule col = particle.GetComponent<ParticleSystem>().colorOverLifetime;
//		col.enabled = true;
//		Gradient grad = new Gradient();
//		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.blue, 1.0f) }, 
//			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.75f, 1.0f) } );
//		col.color = new ParticleSystem.MinMaxGradient (grad);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void Reset(Transform location, float speed)
	{
//		collectibleSpeed = speed;
//		collectibleLocation = location;

		//Do more stuff if required
		trail.Clear();
		transform.position = location.position;
//		Debug.Log ("Inside Reset" + transform.position);

		if(location.position.y >= 0)
			collectibleMover.SetDirectionAndSpeed (Vector3.down, speed);
		else
			collectibleMover.SetDirectionAndSpeed (Vector3.up, speed);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag ("Player")) 
		{
			//Do some stuff with game manager
			Vector3 prevPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
			gameController.IncrementPlayerScore(value);
			gameController.PlayCollectibleSound ();
			transform.position = collectibleLocation.position;
			Instantiate (particle, prevPosition, Quaternion.identity);
			gameObject.SetActive(false);
		}
	}

	void OnDisable()
	{
		collectibleMover.SetDirectionAndSpeed (Vector3.zero, collectibleSpeed);
		gameController.DecreaseActiveCollectible ();
//		Debug.Log ("B4 disabling:" + transform.position);
	}
}
