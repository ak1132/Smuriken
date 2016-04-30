using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class GoogleAdManager : MonoBehaviour
{

	BannerView bannerView;
	InterstitialAd interstitial;

	// Use this for initialization
	void Start ()
	{
		RequestBanner ();
		RequestInterstitial ();
	}

	private void RequestBanner ()
	{
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-3278882458593647/1273605819";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView (adUnitId, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
//		AdRequest request = new AdRequest.Builder().Build();
		AdRequest request = new AdRequest.Builder ()
			.Build ();
		// Load the banner with the request.
		bannerView.LoadAd (request);
	}

	private void RequestInterstitial ()
	{
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-3278882458593647/2471137412";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd (adUnitId);
		// Create an empty ad request.
//		AdRequest request = new AdRequest.Builder().Build();
		AdRequest request = new AdRequest.Builder ()
			.Build ();
		// Load the interstitial with the request.
		interstitial.LoadAd (request);
	}

	public void DisplayBannerAd ()
	{
		bannerView.Show ();
	}

	public void HideBannerAd ()
	{
		bannerView.Hide ();
	}

	public void DisplayInterstitialAd ()
	{
		if (interstitial.IsLoaded ()) {
			interstitial.Show ();
		}
	}

	public void DestroyAllAds ()
	{
		bannerView.Destroy ();
		interstitial.Destroy ();
	}
}
