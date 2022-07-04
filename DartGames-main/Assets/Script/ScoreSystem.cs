using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public Text targetPoint;
    public TMPro.TextMeshProUGUI pointText;
    public GameObject pointAnimGo;
    public int currentPoint=301;
    private const float pointTextTimer = 1f;
    public int justiceScore=0;

    public bool great, nice, BullEye,isChange;
    // Start is called before the first frame update
    void Start()
    {
        targetPoint = GameObject.Find("TargetPoint").GetComponent<Text>();
    }

    public void SetStartingPoint(int point)
    {
        //isChange = true;
        currentPoint = point;
        targetPoint.text = currentPoint.ToString();
    }
    public void ChangePoint(int point, int multiplier)
    {
        int tempPoint = currentPoint;
        if (point == 25 || point == 50)
        {
            BullEye = true;
            nice = false;
        }
      
        if (multiplier == 1)
        {
            if (GetComponent<GameController>().gameMode != GameMode.HighScore)
            {
                currentPoint -= point;
                if (currentPoint < 0)
                {
                    currentPoint = tempPoint;
                    GetComponent<GameController>().bust = true;
                }
            }
            else
            {
                currentPoint += point;
            }


           
            pointText.text = point.ToString();
            StartCoroutine(PointShowAnimation());
        }
        else
        {
            if (GetComponent<GameController>().gameMode != GameMode.HighScore)
            {
                currentPoint -= (point * multiplier);
                if (currentPoint < 0)
                {
                    currentPoint = tempPoint;
                    GetComponent<GameController>().bust = true;
                }
            }
            else
            {
                currentPoint += (point * multiplier);
            }            
            pointText.text = point.ToString() +'x' +multiplier ;
            StartCoroutine(PointShowAnimation());

        }
       





    }



    IEnumerator PointShowAnimation()
    {
        bool clear = false;
        GetComponent<DartFly>().currentMode = Mode.WaitScore;
        float timer = 0;
        if (currentPoint == 0 && GetComponent<GameController>().gameMode!= GameMode.HighScore)
        {
            GetComponent<DartFly>().ThrowDartFakeSlow();
        }
        else
        {
            if(currentPoint >=300 &&GetComponent<GameController>().gameMode == GameMode.HighScore)
            {
                GetComponent<DartFly>().ThrowDartFakeSlow();
            }
            else
            {
                GetComponent<DartFly>().ThrowDartFake();
            }
           
        }
      
    
        while (GetComponent<DartFly>().waitCameraAnim)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);
        pointAnimGo.SetActive(true);
        
        while (timer <= pointTextTimer)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (BullEye)
        {
            BullEye = false;
            GetComponent<GameController>().PlayTextEffect(3);

            GetComponent<SoundController>().PlayRandomSugoi();
            yield return new WaitForSeconds(2);
            GetComponent<GameController>().StopTextEffect(3);
        }
        else if (great)
        {
            great = false;
            GetComponent<GameController>().PlayTextEffect(2);
            GetComponent<SoundController>().PlayRandomSugoi();
            yield return new WaitForSeconds(2);
            GetComponent<GameController>().StopTextEffect(2);
        }
        else if (nice)
        {
            nice = false;
            GetComponent<GameController>().PlayTextEffect(1);
            yield return new WaitForSeconds(2);
            GetComponent<GameController>().StopTextEffect(1);
        }
        pointAnimGo.SetActive(false);
        targetPoint.text = currentPoint.ToString();

        if (GetComponent<GameController>().gameMode != GameMode.HighScore)
        {
            
            if(currentPoint == 0)
            {
                if (GetComponent<GameController>().gameMode == GameMode.Justice)
                {
                    CalculateScore(GetComponent<GameController>().dartStock);
                    

                    yield return StartCoroutine(GetComponent<GameController>().ShowNextTarget());
                    
                }
                else
                {
                    GetComponent<GameController>().DisableDart();
                    GetComponent<GameController>().ShowClearPanel();
                    clear = true;
                }

            }
            else
            {
                GetComponent<DartFly>().currentMode = Mode.Throw;
                GetComponent<DartFly>().Tap();
            }
        }
        else
        {
            if(currentPoint >=300)
            {
                GetComponent<GameController>().DisableDart();
                GetComponent<GameController>().ShowClearPanel();
                clear = true;
            }
            GetComponent<DartFly>().currentMode = Mode.Throw;
            GetComponent<DartFly>().Tap();
        }

        if (!clear)
        {
            GetComponent<GameController>().NextDart();
         
        }
        
    }


    void CalculateScore(int dartStock)
    {
        if(dartStock == 0)
        {
            justiceScore += 5;
            justiceScore += GetComponent<GameController>().round;
        }
        else if (dartStock == 1)
        {
            justiceScore +=15;
            justiceScore += GetComponent<GameController>().round;
        }
        else if (dartStock == 2)
        {
            justiceScore += 30;
            justiceScore += GetComponent<GameController>().round;
        }
    }

}
