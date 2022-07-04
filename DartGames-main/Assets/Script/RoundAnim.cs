using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundAnim : MonoBehaviour
{
   private bool isAnimating =false;
    public bool IsAnimating(){
        return isAnimating;
    }
    public void IsAnimatingOn()
    {
        isAnimating = true;
    }

    public void IsAnimatingOff()
    {
        isAnimating = false;
    }

}
