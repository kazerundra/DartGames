using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinAnim : MonoBehaviour
{

    public void ChangeToSmite()
    {
        GetComponent<Animator>().SetTrigger("ToSmile");
    }
      
    public void ChangeToNormal()
    {
        GetComponent<Animator>().SetTrigger("ToNormal");
        
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
