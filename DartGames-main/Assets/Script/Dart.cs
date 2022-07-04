using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{

    public bool isOnBoard = false;
    public bool clone;
    public bool IsOnBoard()
    {
        return isOnBoard;
    }
    public void IsOnBoard(bool change)
    {
        isOnBoard= change;
        if (clone)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
