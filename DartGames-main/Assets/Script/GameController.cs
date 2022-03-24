using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public enum GameMode { A=301,B=501,HighScore,Justice };

public class GameController : MonoBehaviour
{
    public GameMode gameMode = GameMode.A;
    public int dartStock = 3;
    public int round = 1;

    public Image dart1, dart2, dart3;
    public List<GameObject> dartList;
    public Text roundText;
    public GameObject gameSelectButton,clearPanel,SettingCanvas,MenuCanvas,textEffect,ChangeTargetEffect ,PointScore, TouchPad;
    public bool waitForAds ,bust,justReset;

    public Image TotalDart;
    public Sprite TotalDartSprite, TotalScoreSprite;
    public Image blackscreen;

    public GameObject roundAnimText;
    //OpenSettingMenu
    public void OpenSetting()
    {
        if (GetComponent<DartFly>().currentMode == Mode.Aim)
        {
            GetComponent<DartFly>().currentMode = Mode.Wait;
            MenuCanvas.SetActive(true);
        }
    }

    //turnOffSetting
    public void CloseSetting()
    {
        if (GetComponent<DartFly>().currentMode == Mode.Wait)
        {
            CloseMenu();
            SettingCanvas.SetActive(false);
        }
    }
    //turnOffMenuAndStartGame
    public void CloseMenu()
    {
        GetComponent<DartFly>().currentMode = Mode.Aim;
        MenuCanvas.SetActive(false);
    }
    
    //turnOffDartIcon
    public void DisableDart()
    {
        Debug.Log("dartstock=" + dartStock);
        if (dartStock == 2)
        {
            dart3.enabled = false;


        }
        else if (dartStock == 1)
        {
            dart2.enabled = false;

        }
        else if (dartStock == 0)
        {
            dart1.enabled = false;

        }
    }


    //PrepareNextDart,if lastdart go to next round
    public void NextDart()
    {
        
        dartList.Add(GetComponent<DartFly>().currentDart);
        if (justReset == true)
        {
            justReset = false;
        }
        else
        {
            dartStock--;
        }
        DisableDart();

        if (dartStock == 0 || bust==true)
        {
           
           GetComponent<DartFly>().currentMode = Mode.Wait;
          
           StartCoroutine(ChangeRound());
         
           



        }
        else
        {
            GetComponent<DartFly>().HandResetAnim();
            GetComponent<DartFly>().currentMode = Mode.Aim;
        }
    }


 
    //next round,if bust show bust
    IEnumerator ChangeRound()
    {
        bool isFail = false;
        if (bust)
        {
            PlayTextEffect(0);
            yield return new WaitForSeconds(2f);
            bust = false;
            StopTextEffect(0);
            if(gameMode == GameMode.Justice)
            {
                ShowFailPanel();
                isFail = true;

            }
        }
        
        if (gameMode == GameMode.Justice)
        {
            if (GetComponent<ScoreSystem>().currentPoint > 0)
            {
                ShowFailPanel();
                Debug.Log(GetComponent<ScoreSystem>().currentPoint);
                isFail = true;
            }
            else
            {
                justReset = true;
                isFail = false;
            }
        }
        else
        {
            if (gameMode == GameMode.HighScore)
            {
                if(round >= 8)
                {
                    ShowFailPanel();
                    isFail = true;
                }
            }
            else
            {
                if (round >= 15)
                {
                    ShowFailPanel();
                    isFail = true;
                }
                else
                {
                    isFail = false;
                }
            }
           
        }

        if (!isFail)
        {
            //if suceess go to next round and reset dart to 3
         
            roundAnimText.GetComponent<RoundAnim>().IsAnimatingOn();
            roundAnimText.gameObject.SetActive(true);
            GetComponent<SoundController>().PlayAudio(4);
            while (roundAnimText.GetComponent<RoundAnim>().IsAnimating())
            {
                yield return null;
            }
            round++;
            roundText.text = round.ToString();
            dartStock = 3;
            dart1.enabled = true;
            dart2.enabled = true;
            dart3.enabled = true;
            for (int i = dartList.Count - 1; i >= 0; i--)
            {
                Destroy(dartList[i]);
            }
            dartList.Clear();
            GetComponent<DartFly>().currentMode = Mode.Aim;
            roundAnimText.gameObject.SetActive(false);
            GetComponent<DartFly>().HandResetAnim();
        }
     
    }
    
    //change game mode
    public void ChangeMode(int modenumber)
    {
        switch(modenumber)
        {
            case 0:
                gameMode = GameMode.A;
                GetComponent<ScoreSystem>().SetStartingPoint(301);
                break;
            case 1:
                gameMode = GameMode.B;
                GetComponent<ScoreSystem>().SetStartingPoint(501);
                break;
            case 2:
                gameMode = GameMode.HighScore;
                GetComponent<ScoreSystem>().SetStartingPoint(0);
                break;
            case 3:
                gameMode = GameMode.Justice;
                //???????????_??
                int random = Random.Range(1, 20) *Random.Range(2, 3);
                GetComponent<ScoreSystem>().SetStartingPoint(random);
                break;
        }
        gameSelectButton.SetActive(false);
        GetComponent<DartFly>().currentMode = Mode.Aim;

    }

    //justice mode random target number
    public int RandomNextTarget()
    {
        
        return Random.Range(1, 20) * Random.Range(2, 3);
    }
    
    //show and unshow next target text(justice mode)

    public IEnumerator ShowNextTarget()
    {
        justReset = true;
        ChangeTargetEffect.SetActive(true);
        yield return new WaitForSeconds(1);
        ChangeTargetEffect.SetActive(false);

       yield return StartCoroutine( ChangeRound());
        GetComponent<ScoreSystem>().SetStartingPoint(RandomNextTarget());
        yield return null;
    }

    //show clear panel 
    public void ShowClearPanel()
    {
      
        clearPanel.transform.GetChild(3).gameObject.SetActive(true);
        clearPanel.transform.GetChild(4).gameObject.SetActive(false);
        GetComponent<SoundController>().PlayHakusyu();
        Debug.Log((round - 1 * 3) + (dartStock));
      //  PlayerPrefs.SetInt("HiScore301", (round - 1 * 3) + (dartStock));
        GetComponent<SoundController>().PlayAudio(2);
        ShowGameResult();
    }
    //show result panel depending on mode
    private void ShowGameResult()
    {
        GetComponent<DartFly>().currentMode = Mode.ShowResult;
        clearPanel.SetActive(true);
        if (gameMode == GameMode.A || gameMode == GameMode.B)
        {
            TotalDart.sprite = TotalDartSprite;
            if (gameMode == GameMode.A)
            {
                clearPanel.transform.GetChild(0).GetComponent<Text>().text = "301";
                int currentScore = dartStock + ((round - 1) * 3);
                Debug.Log(currentScore);
                Debug.Log(PlayerPrefs.GetInt("High301", 99));
                if (PlayerPrefs.GetInt("High301", 99) > currentScore)
                {
                    PlayerPrefs.SetInt("High301", currentScore);
                }
                
            }
            else
            {
                clearPanel.transform.GetChild(0).GetComponent<Text>().text = "501";
                int currentScore = dartStock + ((round - 1) * 3);
                Debug.Log(currentScore);
                if (PlayerPrefs.GetInt("High501", 99) > currentScore)
                {
                    PlayerPrefs.SetInt("High501", currentScore);
                }
                
            }
        }
        else
        {
            if (gameMode == GameMode.HighScore)
            {
                clearPanel.transform.GetChild(0).GetComponent<Text>().text = "CountUp";

                int currentScore = dartStock + ((round - 1) * 3);
                Debug.Log(currentScore);
                if (PlayerPrefs.GetInt("HighC", 99) > currentScore)
                {
                    PlayerPrefs.SetInt("HighC",currentScore);
                }
                    
            }
            else
            {
                clearPanel.transform.GetChild(0).GetComponent<Text>().text = "Justice";
                TotalDart.sprite = TotalScoreSprite;
                int currentScore = GetComponent<ScoreSystem>().justiceScore;
                if (PlayerPrefs.GetInt("HighJ2", 0) < currentScore)
                {
                    PlayerPrefs.SetInt("HighJ1", round);
                    PlayerPrefs.SetInt("HighJ2", currentScore);
                }
            }
        }
       
        clearPanel.transform.GetChild(1).GetComponent<Text>().text = round.ToString();
        clearPanel.transform.GetChild(2).GetComponent<Text>().text = ((round - 1) * 3 + dartStock).ToString();
    }

    //show fail panel
    public void ShowFailPanel()
    {
        
        clearPanel.transform.GetChild(3).gameObject.SetActive(false);
        clearPanel.transform.GetChild(4).gameObject.SetActive(true);
        //Debug.Log((round - 1 * 3) + (dartStock));
        ShowGameResult();
    }

    //resetgame button if pushed
    public void ResetGame()
    {
        StartCoroutine(StartAds());
    }

    //check number of play, if second time show ads video
    IEnumerator StartAds()
    {
        if (GetComponent<AdsScript>().GetNoAds())
        {
            blackscreen.color = new Color32(0, 0, 0, 0);
            yield return StartCoroutine(BlackScreenFade());
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

          
        }
        else
        {
            if (PlayerPrefs.GetInt("Ads", 0) <3)
            {
                Debug.Log(PlayerPrefs.GetInt("Ads", 0));
                blackscreen.color = new Color32(0, 0, 0, 0);
                yield return StartCoroutine(BlackScreenFade());
                PlayerPrefs.SetInt("Ads", PlayerPrefs.GetInt("Ads", 0) + 1);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                PlayerPrefs.SetInt("Ads",0);
                SettingCanvas.GetComponent<MixerController>().TurnOffVol();
                GetComponent<AdsScript>().ShowAd();
                waitForAds = true;
                yield return waitForAds;
                //GetComponent<AdsScript>().LoadBanner();

                
            }



          
            
           
            
        }
      
    }

    
    //black screen animation
    IEnumerator BlackScreenFade()
    {

        Color32 blackscreenColor = blackscreen.color;
        while (blackscreenColor.a !=255)
        {
            blackscreenColor.a +=5;
            blackscreen.color = blackscreenColor;
          
            yield return new WaitForSeconds(0.005f);
        }

        yield return new WaitForSeconds(1);

    }
    //return to title screen
    public void ToTitle()
    {
        if (!GetComponent<AdsScript>().GetNoAds())
        {
            GetComponent<AdsScript>().HideBannerAd();
        }
        SceneManager.LoadScene(0);
    }
    private void Start()
    {
        SettingCanvas.GetComponent<MixerController>().Initialize();
        Input.multiTouchEnabled=false;
        justReset = false;
        ChangeMode(PlayerPrefs.GetInt("mode"));
        //prepare ads and banner
        if (!GetComponent<AdsScript>().GetNoAds())
        {
            GetComponent<AdsScript>().InitializeAds();
            GetComponent<AdsScript>().LoadBanner();
            GetComponent<AdsScript>().LoadAd();

        }

        //change text to point or score depends on mode
        if (gameMode== GameMode.Justice)
        {
           // PointScore.GetComponent<PointScoreChange>().ChangeToScore();
        }
        else
        {
           // PointScore.GetComponent<PointScoreChange>().ChangeToPoint();
        }

    }

    //turn on text effect
    public void PlayTextEffect(int i)
    {
        textEffect.transform.GetChild(i).gameObject.SetActive(true);

    }
    //turn off text effect
    public void StopTextEffect(int i)
    {
        textEffect.transform.GetChild(i).gameObject.SetActive(false);

    }
}
