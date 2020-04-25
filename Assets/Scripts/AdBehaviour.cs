using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdBehaviour : MonoBehaviour
{
    private InterstitialAd interstitial;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        RequestInterstitial();
    }
    
    private void RequestInterstitial()
    {
        #if UNITY_ANDROID
            string adUnitId = "Your AD Unit ID";     
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void GameOver()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
            this.interstitial.Destroy();
            RequestInterstitial();
        }

    }

}
