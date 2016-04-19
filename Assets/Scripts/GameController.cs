using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static int timesPlayed = 1;

	public Transform[] collectibleSpawnPoints;
	public Transform[] obstacleSpawnPoints;
	public GameObject[] collectibles;
	public GameObject[] obstacles;
	public int maxLevels;
	public float collectibleSpeed;
	public float obstacleSpeed;
	public float deltaSpeed;
	public float deltaTime;
	public int playerScore;
	public int transitionScore;
	public int maxActiveCollectibles;
	public int maxActiveObstacles;
	public float timeBetweenLevels;
	public float maxCollectibleTime;
	public float minCollectibleTime;
	public float maxObstacleTime;
	public float minObstacleTime;
	public GameObject bg;
	public Color[] colours;
	public AudioClip clipGameOver;
	public AudioClip clipCollectible;
	public AudioClip clipLevelUp;
	public AudioClip clipGameRestart;
	public AudioSource bgAudioSource;
	public Text highScoreText;
	public Text currentScoreText;
//	public bool[] cPath;
//	public bool[] oPath;


	private int activeCollectibles;
	private int activeObstacles;
	private int level;
	[SerializeField] private int highScore;
	[SerializeField]private bool adWatched;
	private bool keepPlaying;
	private bool isCollectibleSelection;
	private bool isObstacleSelection;
	private Renderer bgRenderer;
	private PlayerData playerData;
	private AudioSource audioSource;
	private bool isMusicEnabled = true;
	private UIManager uiManager;
	private GoogleAdManager googleAd;
	private int prevRandomPattern;

	void OnEnable()
	{
		playerData = new PlayerData ();
		if (playerData.Load ()) {
			highScore = playerData.data.HighScore;
			adWatched = playerData.data.AdWatched;
		} else {
			playerData.data.HighScore = 0;
			playerData.data.AdWatched = false;
			playerData.Save ();
			highScore = 0;
			adWatched = false;
		}
		//Change adWatched back to false after rewarding the player in the game
		playerData.data.AdWatched = false;
	}

	// Use this for initialization
	void Start () {
		activeObstacles = 0;
		prevRandomPattern = -1;
		activeCollectibles = 0;
		playerScore = 0;
		level = 1;
		keepPlaying = true;
		isCollectibleSelection = false;
		isObstacleSelection = false;
		maxActiveCollectibles = adWatched == true ? 2 : 1;
		if (Camera.main.aspect <= 0.57f)
			Camera.main.orthographicSize = 5.5f;
		else if (Camera.main.aspect <= 0.63f)
			Camera.main.orthographicSize = 5f;
		else
			Camera.main.orthographicSize = 4.5f;
		bgRenderer = bg.GetComponent<Renderer> ();
		audioSource = GetComponent<AudioSource> ();
		uiManager = GetComponent<UIManager> ();
		googleAd = GetComponent<GoogleAdManager> ();

//		if (PlayerPrefs.HasKey ("Sound")) {
//			isMusicEnabled = PlayerPrefs.GetInt ("Sound") == 1 ? true : false;
//			if (!isMusicEnabled)
//				DisableSound ();
//		} else 
//		{
//			PlayerPrefs.SetInt ("Sound", 1);
//			isMusicEnabled = true;
//		}

//		playerData = new PlayerData ();
//		if (playerData.Load ()) {
//			highScore = playerData.data.HighScore;
//			adWatched = playerData.data.AdWatched;
//		} else {
//			playerData.data.HighScore = 0;
//			playerData.data.AdWatched = false;
//			playerData.Save ();
//			highScore = 0;
//			adWatched = false;
//		}
//		//Change adWatched back to false after rewarding the player in the game
//		playerData.data.AdWatched = false;

//		Debug.Log ("Collectibles Length:" + collectibles.Length);
//		for (int i = 0; i < 3; i++) {
//			cPath [i] = true;
//			oPath [i] = true;
//		}
		StartCoroutine (Play ());
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			Application.Quit ();
		}
	}

	IEnumerator Play()
	{
		//Select a random spawn Point
		//Select a random collectible and put it on the spawn point selected
		//same for obstacles
		//repeat until the no of obstacles and collectibles reach the limit
		//repeat everything until the player progresses to next level
		while (keepPlaying )
		{
//			Debug.Log ("Level:"+level);
//			Invoke ("DisplayLevel", 1.0f);
			uiManager.StartLevelUpAnimation(level);
			if (level == 1) {
				yield return new WaitForSeconds (timeBetweenLevels - 2.0f);
			} else {
				yield return new WaitForSeconds (timeBetweenLevels);
			}
			while (playerScore < level * transitionScore)
			{
				//Debug.Log ("Player Score:" + playerScore + " level:" + level);
//				Invoke("SelectRandomPositionandCollectible",
//					Random.Range(minCollectibleTime,maxCollectibleTime));
//				Debug.Log("isCollectibleSelection"+isCollectibleSelection +" isObstacleSelection"+isObstacleSelection);
				if (!isCollectibleSelection) 
				{
					isCollectibleSelection = true;
					StartCoroutine (SelectRandomPositionandCollectible ());
				}
				if (!isObstacleSelection) 
				{
					isObstacleSelection = true;
//					StartCoroutine (SelectRandomPositionandObstacle ());
					StartCoroutine(SelectRandomPositionAndObstacles());
				}
				yield return new WaitForSeconds(0.1f);
			}

			level += 1;
//			PlayLevelUpSound ();
			SetLevelSpeedAndTime ();
			StartCoroutine (ChangeColor(3.0f));
		}
	}

	public void GameOverHandler()
	{
		timesPlayed += 1;
		keepPlaying = false;
		highScoreText.text = "BEST SCORE: " + highScore.ToString();
		currentScoreText.text = playerScore.ToString ();
//		Debug.Log ("TimesPlayed "+ timesPlayed);
		if (playerScore > highScore) {
			playerData.data.HighScore = playerScore;
			highScoreText.text = "NEW RECORD!";
		}
		playerData.Save ();
//		yield return new WaitForSeconds (2.0f);
		Time.timeScale = 0.5f;
		uiManager.GameOverAnimation();
		if (timesPlayed % 4 == 0)
			googleAd.DisplayInterstitialAd ();
//		Invoke("ResetLevel",1.0f);
//		Time.timeScale = 0.5f;
////		Time.timeScale = 0;
////		ResetTimeScale(2.0f);
//		//Remove this line after debugging
//		googleAd.HideBannerAd ();
//		if (timesPlayed % 3 == 0) {
//			if (!unityAd.ShowDefaultAd ())
//				googleAd.DisplayInterstitialAd ();
//		}
//		googleAd.DisplayBannerAd ();
	}

	public void OnAdCompletion()
	{
		playerData.data.AdWatched = true;
		playerData.Save ();
	}

	public void ResetLevel()
	{
		Time.timeScale = 1;
		SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().name);
	}

	public void DecreaseActiveCollectible()
	{
		activeCollectibles -= 1;
	}

	public void DecreaseActiveObstacle()
	{
		activeObstacles -= 1;
	}

	IEnumerator SelectRandomPositionandObstacle()
	{
		int maxNum = 1;
		if (level == 1)
			maxNum = Random.Range (0, 2);
		else if (level == 3)
			maxNum = Random.Range (1, 3);
		else if (level >= 4)
			maxNum = 2;
		int prevRandPath = -1;
		while (activeObstacles < maxActiveObstacles && maxNum > 0)
		{
			int randPath = Random.Range (0, 3);
			if (randPath != prevRandPath) 
			{
				prevRandPath = randPath;
				for (int i = 0; i < obstacles.Length; i++) 
				{
					if (obstacles [i].activeSelf == false) 
					{
						obstacles [i].SetActive (true);
						obstacles [i].GetComponent<ObstacleController> ().Reset(
							obstacleSpawnPoints [Random.Range (2 * randPath, 2 * randPath + 2)],
							obstacleSpeed);
						break;
					}
				}
				maxNum -= 1;
				activeObstacles += 1;
			}
		}
		yield return new WaitForSeconds (Random.Range(minObstacleTime,maxObstacleTime));
		isObstacleSelection = false;
	}

	IEnumerator SelectRandomPositionAndObstacles()
	{
		if (activeObstacles < maxActiveObstacles)
		{
			int lev = level % maxLevels;
			int pattern = 0;
			switch (lev) 
			{
			case 1:
				pattern = Random.Range (0, 3);
				SetObstacleValues (Random.Range (2 * pattern, 2 * pattern + 2), obstacleSpeed * 2f);
				break;
			case 2:
				pattern = Random.Range (0, 3);
				if (pattern == prevRandomPattern)
					pattern = (pattern + 2) % 3;
				SetObstacleValues (2 * pattern, obstacleSpeed * 2.2f);
				SetObstacleValues (2 * pattern + 1, obstacleSpeed * 2);
				yield return new WaitForSeconds (deltaTime);
				prevRandomPattern = pattern;
				break;
			case 3:
				pattern = Random.Range (0, 4);
				switch (pattern)
				{
				case 0:
					SetObstacleValues (0, obstacleSpeed * 1.75f);
					SetObstacleValues (3, obstacleSpeed * 1.75f);
					break;
				case 1:
					SetObstacleValues (1, obstacleSpeed * 1.75f);
					SetObstacleValues (2, obstacleSpeed * 1.75f);
					break;
				case 2:
					SetObstacleValues (3, obstacleSpeed * 1.75f);
					SetObstacleValues (4, obstacleSpeed * 1.75f);
					break;
				case 3:
					SetObstacleValues (2, obstacleSpeed * 1.75f);
					SetObstacleValues (5, obstacleSpeed * 1.75f);
					break;
				}
				break;
			case 4:
				pattern = Random.Range (0, 4);
				switch (pattern) {
				case 0:
					SetObstacleValues (0, obstacleSpeed * 1.7f);
					yield return new WaitForSeconds (0.4f);
					SetObstacleValues (2, obstacleSpeed * 1.9f);
					break;
				case 1:
					SetObstacleValues (3, obstacleSpeed * 1.7f);
					yield return new WaitForSeconds (0.4f);
					SetObstacleValues (1, obstacleSpeed * 1.9f);
					break;
				case 2:
					SetObstacleValues (5, obstacleSpeed * 1.7f);
					yield return new WaitForSeconds (0.4f);
					SetObstacleValues (3, obstacleSpeed * 1.9f);
					break;
				case 3:
					SetObstacleValues (2, obstacleSpeed * 1.7f);
					yield return new WaitForSeconds (0.4f);
					SetObstacleValues (4, obstacleSpeed * 1.9f);
					break;
				}
//				yield return new WaitForSeconds (deltaTime);
				break;
			case 5:
				pattern = Random.Range (0, 3);
				switch (pattern) {
				case 0:
					SetObstacleValues (0, obstacleSpeed * 1.7f);
					yield return new WaitForSeconds (0.4f);
					SetObstacleValues (4, obstacleSpeed * 1.9f);
					break;
				case 1:
					SetObstacleValues (1, obstacleSpeed * 1.7f);
					yield return new WaitForSeconds (0.4f);
					SetObstacleValues (5, obstacleSpeed * 1.9f);
					break;
				case 2:
					SetObstacleValues (2, obstacleSpeed * 2f);
					SetObstacleValues (3, obstacleSpeed * 2f);
					break;
				}
//				yield return new WaitForSeconds (deltaTime);
				break;
			case 6:
				pattern = Random.Range (0, 4);
				switch (pattern) {
				case 0:
					SetObstacleValues (0, obstacleSpeed * 1.7f);
					SetObstacleValues (4, obstacleSpeed * 1.7f);
					yield return new WaitForSeconds (1f);
					SetObstacleValues (2, obstacleSpeed * 2.3f);
					break;
				case 1:
					SetObstacleValues (1, obstacleSpeed * 1.7f);
					SetObstacleValues (5, obstacleSpeed * 1.7f);
					yield return new WaitForSeconds (1f);
					SetObstacleValues (3, obstacleSpeed * 2.3f);
					break;
				case 2:
					SetObstacleValues (2, obstacleSpeed * 1.7f);
					yield return new WaitForSeconds (1f);
					SetObstacleValues (0, obstacleSpeed * 2.3f);
					SetObstacleValues (4, obstacleSpeed * 2.3f);
					break;
				case 3:
					SetObstacleValues (3, obstacleSpeed * 1.7f);
					yield return new WaitForSeconds (1f);
					SetObstacleValues (1, obstacleSpeed * 2.3f);
					SetObstacleValues (5, obstacleSpeed * 2.3f);
					break;
				}
				yield return new WaitForSeconds (deltaTime);
				break;
			case 7:
				pattern = Random.Range (0, 4);
				switch (pattern) {
				case 0:
					SetObstacleValues (0, obstacleSpeed * 1.4f);
					SetObstacleValues (2, obstacleSpeed * 1.4f);
					SetObstacleValues (5, obstacleSpeed * 1.4f);
					break;
				case 1:
					SetObstacleValues (1, obstacleSpeed * 1.4f);
					SetObstacleValues (2, obstacleSpeed * 1.4f);
					SetObstacleValues (4, obstacleSpeed * 1.4f);
					break;
				case 2:
					SetObstacleValues (1, obstacleSpeed * 1.4f);
					SetObstacleValues (3, obstacleSpeed * 1.4f);
					SetObstacleValues (4, obstacleSpeed * 1.4f);
					break;
				case 3:
					SetObstacleValues (0, obstacleSpeed * 1.4f);
					SetObstacleValues (3, obstacleSpeed * 1.4f);
					SetObstacleValues (5, obstacleSpeed * 1.4f);
					break;
				}
				yield return new WaitForSeconds (deltaTime * 1.5f);
				break;
			case 8:
				pattern = Random.Range (0, 4);
				switch (pattern) {
				case 0:
					SetObstacleValues (2, obstacleSpeed * 2f);
					yield return new WaitForSeconds (0.55f);
					SetObstacleValues (0, obstacleSpeed * 2f);
					SetObstacleValues (4, obstacleSpeed * 2f);
					yield return new WaitForSeconds (0.55f);
					SetObstacleValues (2, obstacleSpeed * 2f);
					break;
				case 1:
					SetObstacleValues (3, obstacleSpeed * 2f);
					yield return new WaitForSeconds (0.55f);
					SetObstacleValues (1, obstacleSpeed * 2f);
					SetObstacleValues (5, obstacleSpeed * 2f);
					yield return new WaitForSeconds (0.55f);
					SetObstacleValues (3, obstacleSpeed * 2f);
					break;
				case 2:
					SetObstacleValues (0, obstacleSpeed * 2f);
					SetObstacleValues (4, obstacleSpeed * 2f);
					yield return new WaitForSeconds (0.55f);
					SetObstacleValues (2, obstacleSpeed * 2f);
					yield return new WaitForSeconds (0.55f);
					SetObstacleValues (0, obstacleSpeed * 2f);
					SetObstacleValues (4, obstacleSpeed * 2f);
					break;
				case 3:
					SetObstacleValues (1, obstacleSpeed * 2f);
					SetObstacleValues (5, obstacleSpeed * 2f);
					yield return new WaitForSeconds (0.55f);
					SetObstacleValues (3, obstacleSpeed * 2f);
					yield return new WaitForSeconds (0.55f);
					SetObstacleValues (1, obstacleSpeed * 2f);
					SetObstacleValues (5, obstacleSpeed * 2f);
					break;
				}
				yield return new WaitForSeconds (deltaTime*4);
				break;
			case 9:
				break;
			case 0:
				pattern = Random.Range (0, 4);
				switch (pattern) {
				case 0:
					SetObstacleValues (4, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (2, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (0, obstacleSpeed * 2.2f);
					SetObstacleValues (4, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (2, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (4, obstacleSpeed * 2.2f);
					break;
				case 1:
					SetObstacleValues (5, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (3, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (1, obstacleSpeed * 2.2f);
					SetObstacleValues (5, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (3, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (5, obstacleSpeed * 2.2f);
					break;
				case 2:
					SetObstacleValues (0, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (2, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (0, obstacleSpeed * 2.2f);
					SetObstacleValues (4, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (2, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (0, obstacleSpeed * 2.2f);
					break;
				case 3:
					SetObstacleValues (1, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (3, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (1, obstacleSpeed * 2.2f);
					SetObstacleValues (5, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (3, obstacleSpeed * 2.2f);
					yield return new WaitForSeconds (0.5f);
					SetObstacleValues (1, obstacleSpeed * 2.2f);
					break;
				}
				yield return new WaitForSeconds (deltaTime*5);
				break;

			}
		}
		yield return new WaitForSeconds (Random.Range(minObstacleTime,maxObstacleTime));
		isObstacleSelection = false;
	}

	void SetObstacleValues(int position, float speed)
	{
		if (activeObstacles < maxActiveObstacles)
		{
			for (int i = 0; i < obstacles.Length; i++) {
				if (obstacles [i].activeSelf == false) {
					obstacles [i].SetActive (true);
					obstacles [i].GetComponent<ObstacleController> ().Reset (
						obstacleSpawnPoints [position],
						speed);
					break;
				}
			}
			activeObstacles += 1;
		}
	}

	public void IncrementPlayerScore(int value)
	{
		playerScore += value;
		uiManager.UpdateScore (playerScore);
		//Do more stuff
	}

	public void SetLevelSpeedAndTime()
	{
		int lev = level % maxLevels;
		if (lev == 2) {
			maxActiveObstacles += 2;
			maxObstacleTime -= deltaTime / 2;
			minObstacleTime -= deltaTime / 2;
		} else if (lev == 3) {
			maxObstacleTime += deltaTime / 2;
			minObstacleTime += deltaTime / 2;
		} else if (lev == 4) {
			collectibleSpeed += deltaSpeed;
			maxCollectibleTime -= 2 * deltaTime;
			maxObstacleTime -= deltaTime;
		} else if (lev == 5) {
			maxActiveObstacles += 2;
			minObstacleTime -= deltaTime / 2;
			maxObstacleTime -= deltaTime / 2;
		} else if (lev == 6) {
			minObstacleTime += deltaTime / 2;
			maxObstacleTime += deltaTime;
		} else if (lev == 8) {
			collectibleSpeed += deltaSpeed;
			maxCollectibleTime -= deltaTime;
			minCollectibleTime -= deltaTime;
		} else if (lev == 1) {
			//reset the entire game with increased obstacle speed
			maxActiveObstacles = 2;
			maxCollectibleTime = 4.0f;
			minCollectibleTime = 1.0f;
			maxObstacleTime = 2.0f;
			minObstacleTime = 1.0f;
			collectibleSpeed = 2.0f;
			obstacleSpeed += deltaSpeed / 2;
		}
	}

	IEnumerator SelectRandomPositionandCollectible()
	{
		//isCollectibleSelection = true;
		int maxNum = 1;
		if (adWatched)
			maxNum = 2;
		int prevRandPath = -1;
		if (activeCollectibles == 0)
		{
			while (activeCollectibles < maxActiveCollectibles && maxNum > 0) {
				int randPath = Random.Range (0, 3);
				int randCollectible = Random.Range (0, collectibles.Length);

				if (randPath != prevRandPath && !collectibles [randCollectible].activeSelf) {
					prevRandPath = randPath;
					collectibles [randCollectible].SetActive (true);
					collectibles [randCollectible].GetComponent<CollectibleController> ().Reset (
						collectibleSpawnPoints [Random.Range (2 * randPath, 2 * randPath + 2)],
						collectibleSpeed);
				
					maxNum -= 1;
					activeCollectibles += 1;
					yield return new WaitForSeconds (Random.Range (minCollectibleTime, maxCollectibleTime));
					//				Debug.Log("maxNum:"+maxNum+" activeCollectibles:" + activeCollectibles);
				}
			}
		}
		yield return new WaitForSeconds (2f);
		isCollectibleSelection = false;
	}

	IEnumerator ChangeColor(float maxTime)
	{
		yield return new WaitForSeconds(1.0f);
		int tempColor = (level - 1) % colours.Length;
		float prevTime = Time.time + maxTime;
		while (Time.time <= prevTime && bgRenderer.material.color != colours [tempColor])
		{
			bgRenderer.material.color = Color.Lerp (
				bgRenderer.material.color,
				colours [tempColor],
				.1f);
			yield return new WaitForSeconds(0.1f);
		}
//		Debug.Log ("Returning");
	}
	//Device id 6PKVLNU4PJYDCIY5

	public void PlayCollectibleSound()
	{
		if (isMusicEnabled) {
			if (playerScore % transitionScore != 0) {
				audioSource.clip = clipCollectible;
				audioSource.Play ();
			} else
				PlayLevelUpSound ();
		}
	}

	public void PlayGameOverSound()
	{
		if (isMusicEnabled) 
		{
			bgAudioSource.Pause ();
			audioSource.clip = clipGameOver;
			audioSource.Play ();
		}
	}

	public void PlayRestartSound()
	{
		if (isMusicEnabled) 
		{
			audioSource.clip = clipGameRestart;
			audioSource.Play ();
		}
	}

	public void PlayLevelUpSound()
	{
		if (isMusicEnabled) 
		{
			audioSource.clip = clipLevelUp;
			audioSource.Play ();
		}
	}

	public void PauseMusic()
	{
		bgAudioSource.Pause ();
		audioSource.Pause ();
	}

	public void ResumeMusic()
	{
		bgAudioSource.UnPause ();
		audioSource.UnPause ();
	}

	public void DisableSound()
	{
		bgAudioSource.Stop ();
		audioSource.Stop ();
		isMusicEnabled = false;
	}

	public void EnableSound()
	{
		isMusicEnabled = true;
		bgAudioSource.Play ();
	}
}
