using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointScoreChange : MonoBehaviour
{
    public Sprite point, score;

    public void ChangeToScore()
    {
        GetComponent<Image>().sprite = score;
    }
    public void ChangeToPoint()
    {
        GetComponent<Image>().sprite = point;
    }
   
}
