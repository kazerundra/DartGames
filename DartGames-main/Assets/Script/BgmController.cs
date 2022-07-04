using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBehaviour
{

    public MixerController mixerController;

    void Start()
    {
        mixerController.Initialize();
        GetComponent<AudioSource>().Play();
    }

}
