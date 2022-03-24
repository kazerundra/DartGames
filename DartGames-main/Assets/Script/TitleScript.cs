using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour
{
    public GameObject SentakuButton,TitleBlackScreen,Rin,BlackPanel,Bgm,SaveData,HiScore,Spawntutorial;
    public bool opening = false;
    private const float speed = 1;


    //opening black screen
    IEnumerator Opening()
    {
        Color32 blackscreenColor = TitleBlackScreen.GetComponent<Image>().color;
        
       while(blackscreenColor.a > 0)
        {
            if(blackscreenColor.a -2 < 0)
            {
                blackscreenColor.a = 0;
            }
            else
            {
                blackscreenColor.a -= 2;
            }
            TitleBlackScreen.GetComponent<Image>().color = blackscreenColor;
            yield return new WaitForSeconds(0.0001f);

        }
        TitleBlackScreen.gameObject.SetActive(false);
        opening = true;
        Bgm.GetComponent<AudioSource>().Play();
        yield return null;
    }

    //make screen black
    public IEnumerator ToBlackScreen()
    {
        
        TitleBlackScreen.gameObject.SetActive(true);
        Color32 blackscreenColor = TitleBlackScreen.GetComponent<Image>().color;

        while (blackscreenColor.a < 255)
        {
            if(blackscreenColor.a +2 > 255)
            {
                blackscreenColor.a = 255;
            }
            else {
                blackscreenColor.a += 2;
            }

            TitleBlackScreen.GetComponent<Image>().color = blackscreenColor;
            yield return new WaitForSeconds(0.0001f);

        }
       
        yield return null;
    }

    // init
    void Start()
    {
        StartCoroutine(Opening());
        SaveData.GetComponent<MixerController>().Initialize();
    }

    //move to stage select menu
    public void ToStageSelect()
    {
        BlackPanel.SetActive(true);
        SentakuButton.SetActive(true);
        SaveData.GetComponent<MixerController>().PlaySe(1);
        Rin.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (SentakuButton.activeInHierarchy == false && opening && !SaveData.GetComponent<SaveData>().IsSetting && !HiScore.activeInHierarchy && Spawntutorial.GetComponent<SpawnTutorialButton>().Stop ==false)
        {
            if (Input.GetMouseButton(0))
            {
                //if first time show tutorial screen
                if (PlayerPrefs.GetInt("Kihon", 0) == 0)
                {
                    BlackPanel.SetActive(true);
                    SaveData.GetComponent<MixerController>().PlaySe(1);
                    Rin.SetActive(true);
                    PlayerPrefs.SetInt("Kihon",1);
                    Spawntutorial.GetComponent<SpawnTutorialButton>().StartKihonTutorial();
                }
                else
                {
                  
                    ToStageSelect();
                }
            }
        }
     
    }
}
