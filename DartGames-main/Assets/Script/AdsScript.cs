using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;


public class AdsScript : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{

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


    public void InitializeAds()
    {
       
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, _enablePerPlacementMode, this);
        LoadVidAds();
        
    }

    public void LoadVidAds()
    {
        _adUnitIdVid = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }

    public void LoadBannerAds()
    {
        _adUnitIdBanner = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsBannerId
            : _androidBannerId;

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

  
    public void LoadAd()
    {

        LoadVidAds();
        Advertisement.Load(_adUnitIdVid, this);
    }

 
    public void ShowAd()
    {
        Advertisement.Show(_adUnitIdVid, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        //Debug.Log("loadComp");
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        GetComponent<GameController>().waitForAds = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    // バナーエラー
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }

    // バナーを表示
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

    //バナー隠す
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { Advertisement.Banner.Hide(); }
    void OnBannerShown() { }
    void OnBannerHidden() { }

}
