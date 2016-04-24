using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayGamesConfig : MonoBehaviour
{

	private string Leaderboard = "CgkI34W329IKEAIQDA";

	private string Level1 = "CgkI34W329IKEAIQAQ";
	private string Level2 = "CgkI34W329IKEAIQAg";
	private string Level3 = "CgkI34W329IKEAIQAw";
	private string Level4 = "CgkI34W329IKEAIQBA";
	private string Level5 = "CgkI34W329IKEAIQBQ";
	private string Level6 = "CgkI34W329IKEAIQBg";
	private string Level7 = "CgkI34W329IKEAIQBw";
	private string Level8 = "CgkI34W329IKEAIQCA";
	private string Level9 = "CgkI34W329IKEAIQCQ";
	private string Level10 = "CgkI34W329IKEAIQCg";
	private string SmurikenMaster = "CgkI34W329IKEAIQCw";


	// Use this for initialization
	void Start ()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ()
		                                      // enables saving game progress.
			.EnableSavedGames ()
		                                      // require access to a player's Google+ social graph to sign in
			.RequireGooglePlus ()
			.Build ();

		PlayGamesPlatform.InitializeInstance (config);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate ();
	}

	public void SignIn ()
	{
		Social.localUser.Authenticate ((bool success) => {
			if (success == true)
				Debug.Log ("Successful Login");
			else
				Debug.Log ("Login failed");
		});
	}

	public void UnlockAchievement (string achieve)
	{
		achieve = GetAchievementStr (achieve);
		Social.ReportProgress (achieve, 100.0f, (bool success) => {
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
			return Level1;
		case "Level2":
			return Level2;
		case "Level3":
			return Level3;
		case "Level4":
			return Level4;
		case "Level5":
			return Level5;
		case "Level6":
			return Level6;
		case "Level7":
			return Level7;
		case "Level8":
			return Level8;
		case "Level9":
			return Level9;
		case "Level10":
			return Level10;
		default:
			return SmurikenMaster;
		}
	}

	public void PostToLeaderBoard (int score)
	{
		Social.ReportScore (score, Leaderboard, (bool success) => {
			if (success == true)
				Debug.Log ("Successful Leaderboard post");
			else
				Debug.Log ("Leaderboard post failed");
		});
	}

	public void ShowLeaderBoardUI ()
	{
		PlayGamesPlatform.Instance.ShowLeaderboardUI (Leaderboard);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

}
