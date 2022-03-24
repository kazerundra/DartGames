using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTutorialButton : MonoBehaviour
{
    [SerializeField]
    private int CurrentPage;
    [SerializeField]
    private int MaxPage;

    [SerializeField]
    private GameObject PrevButton, NextButton,ButtonPrefab,TutorialImage,ButList,Title;
    [SerializeField]
    private Sprite ButtonOff, ButtonOn;

    public List<Sprite> TutorialImageList;

    public List<Sprite> KihonTutorial;
    public List<Image> CircleBList;

    public bool Stop = false;
    public MixerController mixerController;

    public void PrevT()
    {
        ChangeSprite(CircleBList[CurrentPage], ButtonOff);
        CurrentPage--;
        ChangeSprite(CircleBList[CurrentPage], ButtonOn);
        mixerController.PlaySe(1);
        ChangeTutorialPage();
        if (NextButton.GetComponent<Button>().IsInteractable() == false)
        {
            NextButton.GetComponent<Button>().interactable = true;
        }
      
    }
    public void NextT()
    {
        ChangeSprite(CircleBList[CurrentPage], ButtonOff);
        CurrentPage++;
        ChangeSprite(CircleBList[CurrentPage], ButtonOn);
        mixerController.PlaySe(1);
        ChangeTutorialPage();
        if (PrevButton.GetComponent<Button>().IsInteractable() == false)
        {
            PrevButton.GetComponent<Button>().interactable = true;
        }
    }
    private void ChangeTutorialPage()
    {
        if (CurrentPage == 0)
        {
            PrevButton.GetComponent<Button>().interactable = false;
        }
        else if (CurrentPage == MaxPage)
        {
            NextButton.GetComponent<Button>().interactable = false;
        }
        TutorialImage.GetComponent<Image>().sprite = TutorialImageList[CurrentPage];
    }

    public void SetTutorial(List<Sprite> SpriteList)
    {
        TutorialImageList.Clear();
        foreach(Sprite sprite in SpriteList)
        {
            TutorialImageList.Add(sprite);
        }
        MaxPage = TutorialImageList.Count - 1;
        for (int i=0; i<= MaxPage; i++)
        {
            GameObject go = Instantiate(ButtonPrefab, ButList.transform);
            CircleBList.Add(  go.GetComponent<Image>());
        }
        ChangeSprite( CircleBList[0],ButtonOn);
        PrevButton.GetComponent<Button>().interactable = false;
        transform.GetChild(0).gameObject.SetActive(false);

    }
    private void ChangeSprite(Image image,Sprite target)
    {
        image.sprite = target; 
    }


    // Start is called before the first frame update
    void Start()
    {
        SetTutorial(KihonTutorial);
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        Stop = false;
        mixerController.PlaySe(1);
    }

    public IEnumerator TutorialPause()
    {
        while (Stop) {
            yield return null;

        }
        Title.GetComponent<TitleScript>().SentakuButton.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void StartKihonTutorial()
    {
        Stop = true;
        ResetTutorial();
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(TutorialPause());
    }

    private void ResetTutorial()
    {
        for (int i = 0; i <= MaxPage; i++)
        {
            if (i == 0)
            {
                CircleBList[i].sprite = ButtonOn;
            }
            else
            {
                CircleBList[i].sprite = ButtonOff;
            }
           
        }
        CurrentPage = 0;
        ChangeTutorialPage();
    }
}
