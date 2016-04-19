using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	public float speed;
	public float xMax;
	public float yMax;
//	public float stationaryTime;
	public float threshold;

	Vector2 startPos;
	Vector2 direction;
	Vector2 endDirection;
	bool directionChosen = false;
	bool canMovePlayer = true;
	Vector3 oldPlayerPosition;
	Vector3 targetPlayerPosition;
//	float incrementTime = 0.0f;
//	bool fingerLifted = true;
	bool playerMoved = false;
	bool gamePaused = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && canMovePlayer && !gamePaused) 
		{
			oldPlayerPosition = transform.position;
			Touch touch = Input.GetTouch (0);
			//Handle finger movements based on TouchPhase
			switch (touch.phase) {
			case TouchPhase.Began:
				startPos = touch.position;
				directionChosen = false;
				break;

			case TouchPhase.Stationary:
				//set a default of 0.3 seconds for no change in finger
//				incrementTime += touch.deltaTime;
//				if (incrementTime > stationaryTime) {
//					directionChosen = true;
//					canMovePlayer = false;
//					incrementTime = 0.0f;
//					SetTargetPosition ();
//				}
				break;

			case TouchPhase.Moved:
				direction = touch.position - startPos;
//				incrementTime += touch.deltaTime;
//				Debug.Log("incrementTime " + incrementTime);
//				fingerLifted = false;

				if (direction.magnitude > threshold && !playerMoved) {
					directionChosen = true;
//					canMovePlayer = false;
//					incrementTime = 0.0f;
					SetTargetPosition ();
					//so that after moving player do not move again on finger lift
					playerMoved = true;
				}
//				directionChosen = true;
//				canMovePlayer = false;
//				SetTargetPosition ();
				break;

			case TouchPhase.Ended:
				endDirection = touch.position - startPos;
////				Debug.Log ("direction.magnitude " + direction.magnitude);
////					if (playerMoved == false) {
				if(endDirection.magnitude > threshold*6 && !playerMoved)
				{
//					incrementTime = 0.0f;
					directionChosen = true;
					canMovePlayer = false;
					SetTargetPositionEnd ();
//						fingerLifted = true;
				} 
				playerMoved = false;
				break;
			}
		}

		if(directionChosen)
		{

			transform.position = Vector3.MoveTowards (transform.position, targetPlayerPosition, speed * Time.deltaTime);
			if (Vector3.Equals (transform.position, targetPlayerPosition)) 
			{
				directionChosen = false;
				canMovePlayer = true;
			}
		}

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		transform.Translate (horizontal * speed * Time.deltaTime, 
			vertical * speed * Time.deltaTime, 
			0.0f);
		
		transform.position = new Vector3(Mathf.Clamp (transform.position.x, -xMax, xMax),
			Mathf.Clamp (transform.position.y, -yMax, yMax),
			0.0f);

	}

	void SetTargetPosition()
	{
		if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
			if (direction.x > 0.0f && oldPlayerPosition.x != xMax)
				targetPlayerPosition = oldPlayerPosition + Vector3.right;
			else if (direction.x <= 0.0f && oldPlayerPosition.x != -xMax)
				targetPlayerPosition = oldPlayerPosition + Vector3.left;
			else
				targetPlayerPosition = oldPlayerPosition;
		} else 
		{
			if (direction.y > 0.0f && oldPlayerPosition.y != yMax)
				targetPlayerPosition = oldPlayerPosition + Vector3.up;
			else if (direction.y <= 0.0f && oldPlayerPosition.y != -yMax)
				targetPlayerPosition = oldPlayerPosition + Vector3.down;
			else
				targetPlayerPosition = oldPlayerPosition;
		}
				
	}

	void SetTargetPositionEnd()
	{
		if (Mathf.Abs (endDirection.x) > Mathf.Abs (endDirection.y)) {
			if (endDirection.x > 0.0f && oldPlayerPosition.x != xMax)
				targetPlayerPosition = oldPlayerPosition + Vector3.right;
			else if (endDirection.x <= 0.0f && oldPlayerPosition.x != -xMax)
				targetPlayerPosition = oldPlayerPosition + Vector3.left;
			else
				targetPlayerPosition = oldPlayerPosition;
		} else 
		{
			if (endDirection.y > 0.0f && oldPlayerPosition.y != yMax)
				targetPlayerPosition = oldPlayerPosition + Vector3.up;
			else if (endDirection.y <= 0.0f && oldPlayerPosition.y != -yMax)
				targetPlayerPosition = oldPlayerPosition + Vector3.down;
			else
				targetPlayerPosition = oldPlayerPosition;
		}

	}

	public void TogglePlayerMovement()
	{
		gamePaused = gamePaused == true ? false : true;
	}
}
