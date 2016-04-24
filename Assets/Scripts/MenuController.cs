using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class MenuController : MonoBehaviour
{
	public Text highScore;
	public Animator parent;
	public Button playButton;
	public GooglePlayGamesConfig gamesConfig;
	private PlayerData playerData;

	void OnEnable ()
	{
		playerData = new PlayerData ();
		if (playerData.Load ())
			highScore.text = "BEST SCORE: " + playerData.data.HighScore.ToString ();
		else
			highScore.text = "BEST SCORE: 0";
	}

	// Use this for initialization
	void Start ()
	{
		if (Camera.main.aspect <= 0.57f)
			Camera.main.orthographicSize = 5.5f;
		else if (Camera.main.aspect <= 0.63f)
			Camera.main.orthographicSize = 5f;
		else
			Camera.main.orthographicSize = 4.5f;
//		Debug.Log ("Camera" + Camera.main.aspect);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void PlayPressed ()
	{
		playButton.interactable = false;
		parent.SetTrigger ("Play");
		Invoke ("LoadGame", 1.4f);
	}

	void LoadGame ()
	{
		gamesConfig = GetComponent<GooglePlayGamesConfig> ();
		gamesConfig.SignIn ();
		SceneManager.LoadSceneAsync ("Main Game");
	}

	public void LeaderBoards ()
	{
		gamesConfig.ShowLeaderBoardUI ();
	}
}
