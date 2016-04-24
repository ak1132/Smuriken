using UnityEngine;

#if UNITY_ADS
using UnityEngine.Advertisements;

// only compile Ads code on supported platforms
#endif

public class UnityAdManager : MonoBehaviour
{
	UIManager uiManager;
	GameController gc;

	public void Start ()
	{
		#if UNITY_ADS
		if (!Advertisement.isInitialized && Advertisement.isSupported)
			Advertisement.Initialize ("1059908");	
		uiManager = GetComponent<UIManager> ();
		gc = GetComponent<GameController> ();
		#endif
	}

	public bool ShowDefaultAd ()
	{
		#if UNITY_ADS
		if (!Advertisement.IsReady ()) {
			Debug.Log ("Ads not ready for default zone");
			return false;
		}

		Advertisement.Show ();
		Debug.Log (Advertisement.gameId + " " + Advertisement.isShowing + " " + Advertisement.isSupported);
		#endif
		return true;
	}

	public void ShowRewardedAd ()
	{
		const string RewardedZoneId = "rewardedVideo";

		#if UNITY_ADS
		if (!Advertisement.IsReady (RewardedZoneId)) {
			Debug.Log (string.Format ("Ads not ready for zone '{0}'", RewardedZoneId));
			return;
		}

		var options = new ShowOptions { resultCallback = HandleShowResult };
		Advertisement.Show (RewardedZoneId, options);
		#endif
		return;
	}

	#if UNITY_ADS
	private void HandleShowResult (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			gc.OnAdCompletion ();
			uiManager.AdSeen ();
			break;
		case ShowResult.Skipped:
			Debug.Log ("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError ("The ad failed to be shown.");
			break;
		}
	}
	#endif

	public bool IsAdReady ()
	{
		const string RewardedZoneId = "rewardedVideo";

		#if UNITY_ADS
		if (!Advertisement.IsReady (RewardedZoneId)) {
			Debug.Log (string.Format ("Ads not ready for zone '{0}'", RewardedZoneId));
			return false;
		} else {
			return true;
		}
		#endif
		return false;
	}
}
