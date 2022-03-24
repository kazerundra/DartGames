using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public List<AudioClip> seList;
    public List<AudioClip> sugoiList;
    public AudioClip clearSe;

    /// <summary>
    /// play se
    /// </summary>
    /// <param name="index"> se number from list</param>
    public void PlayAudio(int index)
    {
        GetComponent<AudioSource>().PlayOneShot(seList[index]);
    }

    //play random sugoi voice
    public void PlayRandomSugoi()
    {
        
        GetComponent<AudioSource>().PlayOneShot(sugoiList[0]);
    }
    // play clapping sound
    public void PlayHakusyu()
    {

        GetComponent<AudioSource>().PlayOneShot(clearSe);
    }
}
