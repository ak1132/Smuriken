using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayGamesConfig
{

	private static GooglePlayGamesConfig _instance = null;

	private GooglePlayGamesConfig ()
	{
		// Code to initialize this object would go here
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate ();
	}

	public static GooglePlayGamesConfig Instance {
		get {
			if (_instance == null) {
				_instance = new GooglePlayGamesConfig ();
			}
			return _instance;
		}
	}
	// Use this for initialization
	void Start ()
	{
		//PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ()
		//.RequireGooglePlus () // require access to a player's Google+ social graph to sign in
		//.Build ();

		//PlayGamesPlatform.InitializeInstance (config);
		// recommended for debugging:

	}

	public void SignIn ()
	{
		if (!PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.Authenticate ((bool success) => {
				if (success) {
					Debug.Log ("Silently signed in! Welcome " + PlayGamesPlatform.Instance.localUser.userName);
				} else {
					Debug.Log ("Oh... we're not signed in.");
				}
			}, true);
		} else {
			Debug.Log ("We're already signed in");
		}
	}

	public void UnlockAchievement (string achieve)
	{
		achieve = GetAchievementStr (achieve);
		PlayGamesPlatform.Instance.ReportProgress (achieve, 100.0f, (bool success) => {
			if (success == true)
				Debug.Log ("Successful Achievement Unlocked");
			else
				Debug.Log ("Achievement Unlocked failed");
		});
	}

	private string GetAchievementStr (string name)
	{
		switch (name) {
		case "Level1":
			return GooglePlayGamesConstants.achievement_level_1;
		case "Level2":
			return GooglePlayGamesConstants.achievement_level_2;
		case "Level3":
			return GooglePlayGamesConstants.achievement_level_3;
		case "Level4":
			return GooglePlayGamesConstants.achievement_level_4;
		case "Level5":
			return GooglePlayGamesConstants.achievement_level_5;
		case "Level6":
			return GooglePlayGamesConstants.achievement_level_6;
		case "Level7":
			return GooglePlayGamesConstants.achievement_level_7;
		case "Level8":
			return GooglePlayGamesConstants.achievement_level_8;
		case "Level9":
			return GooglePlayGamesConstants.achievement_level_9;
		case "Level10":
			return GooglePlayGamesConstants.achievement_level_10;
		default:
			return GooglePlayGamesConstants.achievement_smuriken_master;
		}
	}

	public void PostToLeaderBoard (int score)
	{
		PlayGamesPlatform.Instance.ReportScore (score, GooglePlayGamesConstants.leaderboard_top_smuriken_masters, (bool success) => {
			if (success == true)
				Debug.Log ("Successful Leaderboard post");
			else
				Debug.Log ("Leaderboard post failed");
		});
	}

	public bool IsAuthenticated ()
	{
		return PlayGamesPlatform.Instance.localUser.authenticated;
	}

	public void ShowLeaderBoardUI ()
	{
		PlayGamesPlatform.Instance.ShowLeaderboardUI (GooglePlayGamesConstants.leaderboard_top_smuriken_masters);
	}

}
