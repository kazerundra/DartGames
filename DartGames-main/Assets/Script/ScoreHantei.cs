using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHantei : MonoBehaviour
{
    const float hanteiJikan = 1;
    public ScoreSystem scoreSystem;
    private GameController gameController;
    private DartFly dartFly;
    [SerializeField]  private int score1, score2, score3;
    private int shineScore=0;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer== 6)
        {
          
            if (collision.gameObject.GetComponent<DartScore>().scoreUpdated == false)
            {
                ScoreSystem scoreSystem = GameObject.Find("GameController").GetComponent<ScoreSystem>();
                collision.gameObject.GetComponent<DartScore>().scoreUpdated = true;
                int score = 0;
                int multiplier = 1;
                score = int.Parse(gameObject.transform.parent.name);
                if (gameObject.name == "x3" )
                {
               
                    multiplier = 3;
                    scoreSystem.great = true;
                }
                else if (gameObject.name == "x2" )
                {
                    multiplier = 2;
                    scoreSystem.nice = true;
                }

                GameMode mode=  GameObject.Find("GameController").GetComponent<GameController>().gameMode;
                if ((multiplier == 1&& score==25 && (mode==GameMode.A|| mode== GameMode.B)))
                {
                    score = 50;
                }
                collision.gameObject.GetComponent<DartScore>().score = score;
               
                scoreSystem.ChangePoint(score,multiplier);
            
               // }
            }
        }else if(collision.gameObject.layer == 8)
        {
            if (Time.timeScale != 1.0f)
            {
                Time.timeScale = 1f;
                StartCoroutine(ResetCameraAnim());
            }


        }
        else if (collision.gameObject.layer == 9)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.GetComponent<DartScore>().scoreUpdated == false)
            {

                collision.gameObject.GetComponent<DartScore>().scoreUpdated = true;
                int score = 0;
                int multiplier = 1;
                collision.gameObject.GetComponent<DartScore>().score = score;
                
                scoreSystem.ChangePoint(score, multiplier);
            }


        }
    }
    
    IEnumerator ResetCameraAnim()
    {
        DartFly dartFly = GameObject.Find("GameController").GetComponent<DartFly>();
        yield return new WaitForSeconds(2);
        dartFly.currentDart.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);

        dartFly.waitCameraAnim = false;
        dartFly.GetComponent<GameController>().TouchPad.SetActive(true);
    }
   
    // Start is called before the first frame update
    void Start()
    {
        scoreSystem= GetComponentInParent<ScoreSystem>();
        dartFly = GetComponentInParent<DartFly>();
        gameController = GetComponentInParent<GameController>();
        score1 = int.Parse( transform.parent.name);
        score2 = score1 * 2;
        score3 = score1 * 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (shineScore!= scoreSystem.currentPoint && dartFly.currentMode == Mode.Aim && gameController.gameMode != GameMode.HighScore)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            if (gameObject.name == "x3" && scoreSystem.currentPoint == score3)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (gameObject.name == "x2" && scoreSystem.currentPoint == score2)
            {
                transform.GetChild(0).gameObject.SetActive(true);

            }
            else if (scoreSystem.currentPoint == score1 && (gameObject.name == "x1" || gameObject.name == "x1s"))
            {
                transform.GetChild(0).gameObject.SetActive(true);

            }
           shineScore = scoreSystem.currentPoint;
        }
       
    }
}
