using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text scoreText;
	public Text levelUpText;
	public GameObject pauseImage;
	public GameObject pauseParent;
	public Button pauseButton;
	public Button resumeButton;
	public Button soundOnButton;
	public Button soundOffButton;
	public GameObject gameOverParent;
	public Button restart;
	public Button watchAd;
	public Text adWatchedText;
	public InputManager player;

	private GameController gameController;
	private Animator pauseParentAnimator;
//	private Animator gameOverParentAnimator;
	private Animator levelUpTextAnimator;
	private UnityAdManager unityAd;

	void Awake()
	{
		pauseParentAnimator = pauseParent.GetComponent<Animator> ();
//		gameOverParentAnimator = gameOverParent.GetComponent<Animator> ();
		levelUpTextAnimator = levelUpText.gameObject.GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
		gameController = gameObject.GetComponent<GameController> ();
		if (PlayerPrefs.HasKey ("Sound")) {
			if (PlayerPrefs.GetInt ("Sound") == 1)
				SoundOn ();
			else
				SoundOff ();
		} else {
			SoundOn ();
		}
		unityAd = GetComponent<UnityAdManager> ();
		Invoke ("StartGameAnimation", 1.0f);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		pauseButton.gameObject.SetActive (false);
		resumeButton.gameObject.SetActive (true);
		pauseImage.SetActive (true);
		gameController.PauseMusic ();
		player.TogglePlayerMovement ();
	}

	public void ResumeGame()
	{
		Time.timeScale = 1.0f;
		resumeButton.gameObject.SetActive (false);
		pauseButton.gameObject.SetActive (true);
		pauseImage.SetActive (false);
		gameController.ResumeMusic ();
		player.TogglePlayerMovement ();
	}

	public void SoundOn()
	{
		PlayerPrefs.SetInt ("Sound", 1);
		soundOffButton.gameObject.SetActive (false);
		soundOnButton.gameObject.SetActive (true);
		gameController.EnableSound ();
	}

	public void SoundOff()
	{
		PlayerPrefs.SetInt ("Sound", 0);
		soundOnButton.gameObject.SetActive (false);
		soundOffButton.gameObject.SetActive (true);
		gameController.DisableSound ();
	}

	void StartGameAnimation()
	{
		pauseParentAnimator.SetTrigger ("Start");
	}

	public void GameOverAnimation()
	{
		pauseButton.interactable = false;
		pauseParentAnimator.SetTrigger ("GameOver");
		if (unityAd.IsAdReady ())
			watchAd.gameObject.SetActive (true);
		Invoke ("ShowGameOverParent", 0.75f);
	}

	public void UpdateScore(int score)
	{
		scoreText.text = score.ToString();
	}

	public void StartLevelUpAnimation(int level)
	{
		levelUpText.gameObject.SetActive (true);
		levelUpText.text = "Level " + level.ToString ();
		levelUpTextAnimator.SetBool ("LevelUp", true);
		Invoke ("EndLevelUpAnimation", 2.8f);
	}

	void EndLevelUpAnimation()
	{
		levelUpTextAnimator.SetBool ("LevelUp", false);
		levelUpText.gameObject.SetActive (false);
	}

	public void ShowGameOverParent()
	{
		pauseParent.SetActive (false);
		gameOverParent.SetActive (true);
		Time.timeScale = 0.0f;
	}

	public void RestartGame()
	{
		restart.interactable = false;
//		gameOverParentAnimator.SetTrigger ("Restart");
		Time.timeScale = 0.5f;
		gameController.PlayRestartSound ();
		Invoke ("CallResetLevel", 0.75f);
	}

	void CallResetLevel()
	{
		gameController.ResetLevel ();
	}

	public void ShowAd()
	{
		unityAd.ShowRewardedAd ();
	}

	public void AdSeen()
	{
		watchAd.gameObject.SetActive (false);
		adWatchedText.gameObject.SetActive (true);
	}
}
