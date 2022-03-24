using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBehaviour
{

    public MixerController mixerController;
    // Start is called before the first frame update
    void Start()
    {
        mixerController.Initialize();
        GetComponent<AudioSource>().Play();
    }

}
