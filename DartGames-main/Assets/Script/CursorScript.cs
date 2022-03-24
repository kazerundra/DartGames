using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    private DartFly dartFly;

    public void PowerUp()
    {
        dartFly.powerIsUp = true;
    }
    public void PowerDown()
    {
        dartFly.powerIsUp = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        dartFly = GameObject.Find("GameController").GetComponent<DartFly>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
