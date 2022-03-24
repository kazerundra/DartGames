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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
