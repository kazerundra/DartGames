using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;


public class AdsScript : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{

    /// <summary>
    /// init
    /// </summary>
    [SerializeField] string _androidGameId = "4282191";
    [SerializeField] string _iOsGameId = "4282190";
   
    [SerializeField] bool _testMode =false;
    [SerializeField] bool _enablePerPlacementMode = true;
    private string _gameId;
    [SerializeField] bool _noAds = false;

    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    [SerializeField] string _androidBannerId = "Banner_Android";
    [SerializeField] string _iOsBannerId = "Banner_iOS";
    

    [SerializeField] BannerPosition _bannerPosition = BannerPosition.TOP_CENTER;
    string _adUnitIdBanner, _adUnitIdVid;


    public bool GetNoAds()
    {
        return _noAds;
    }

    void Awake()
    {
        _noAds = true;
        /*
#if UNITY_ANDROID
        _noAds = false;
#elif UNITY_IOS
                  _noAds =false;
#elif UNITY_STANDALONE_OSX
                 _noAds = true;
#elif UNITY_STANDALONE_WIN
                  _noAds = true;
#endif
        */
    }

    //?L????????
    public void InitializeAds()
    {
       
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsGameId
            : _androidGameId;
       // Debug.Log(_gameId +"1");
        //Debug.Log(_androidGameId+ "1");
        Advertisement.Initialize(_gameId, _testMode, _enablePerPlacementMode, this);
        LoadVidAds();
        
    }
    //???????L????ID
    public void LoadVidAds()
    {
        _adUnitIdVid = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;

        //Debug.Log(_gameId + "2");
        //Debug.Log(_adUnitIdVid);
       // Advertisement.Banner.SetPosition(_bannerPosition);
    }
    //?o?i?[?L????ID
    public void LoadBannerAds()
    {
        _adUnitIdBanner = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsBannerId
            : _androidBannerId;

        //Debug.Log(_gameId + "2");
        //Debug.Log(_adUnitIdBanner);
        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    public void OnInitializationComplete()
    {
        //Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
       // Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    ////---------------------------------------------------------------------------
    // Ad Unit?????[?h
    public void LoadAd()
    {
        //HideBannerAd();
        LoadVidAds();
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        //Debug.Log("Loading Ad: " + _adUnitIdVid);
        Advertisement.Load(_adUnitIdVid, this);
    }

    // Ad Unit???L?????\??: 
    public void ShowAd()
    {
        //LoadAd();
        // Note that if the ad content wasn't previously loaded, this method will fail
       // Debug.Log("Showing Ad: " + _adUnitIdVid);
        Advertisement.Show(_adUnitIdVid, this);
    }

    // Implement Load Listener and Show Listener interface methods:  
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        //Debug.Log("loadComp");
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execite code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        GetComponent<GameController>().waitForAds = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Optionally execite code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) {
        Advertisement.Banner.Hide();
        Debug.Log("vidstart");
    }
    public void OnUnityAdsShowClick(string adUnitId) { Debug.Log("vidClick"); }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        Debug.Log("videCOmpl");
        GetComponent<GameController>().waitForAds = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }




    public void LoadBanner()
    {
        LoadBannerAds();
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitIdBanner, options);
    }
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        ShowBannerAd();
    }

    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }

    // Implement a method to call when the Show Banner button is clicked:
    void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_adUnitIdBanner, options);
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(_androidBannerId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(_androidBannerId);
    }

    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { Advertisement.Banner.Hide(); }
    void OnBannerShown() { }
    void OnBannerHidden() { }

}
