using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;


public class SaveData : MonoBehaviour
{
    int sentakuMode;
    AsyncOperation loadingOperation;
    public bool IsSetting;
    private bool pressed = false;
    GameObject modeButton;


    //
    public void CloseSetting()
    {
        IsSetting = false;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            modeButton.SetActive(true); 
        }
    }
    public void SettingBool()
    {
        IsSetting = true;
    }



    public void SelectMode(int index)
    {
        if (!pressed)
        {
            pressed = true;
            sentakuMode = index;
            PlayerPrefs.SetInt("mode", index);
            StartCoroutine(GameLoad());
        }
          
    }

    //load main game
    IEnumerator GameLoad()
    {
        GameObject title = GameObject.Find("Title");
        GameObject rin = GameObject.Find("Rin");
        rin.GetComponent<RinAnim>().ChangeToNormal();
        GetComponent<MixerController>().PlaySe(0);
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(title.GetComponent<TitleScript>().ToBlackScreen());
        loadingOperation = SceneManager.LoadSceneAsync(1);
        
        while (!loadingOperation.isDone)
        {
            yield return null;
        }
     
    }
    // Start is called before the first frame update
    void Start()
    {

        modeButton = GameObject.Find("ModeButton").gameObject;
        modeButton.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
