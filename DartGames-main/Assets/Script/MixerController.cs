using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerController : MonoBehaviour
{
    public AudioMixer audioMixer;

    public List<AudioClip> SE_list;
    public List<AudioClip> BGM_list;

    private AudioSource SE, BGM;


    public Slider bgm, se, master;
    // initialize volume
    public void Initialize()
    {
        audioMixer.SetFloat("Master", ConvertToMixer(PlayerPrefs.GetFloat("Master", 1)));
        master.value = PlayerPrefs.GetFloat("Master",1);
        audioMixer.SetFloat("SE", ConvertToMixer(PlayerPrefs.GetFloat("SE", 1)));
        se.value = PlayerPrefs.GetFloat("SE", 1);
        audioMixer.SetFloat("BGM", ConvertToMixer(PlayerPrefs.GetFloat("BGM", 1)));
        bgm.value = PlayerPrefs.GetFloat("BGM", 1);
    }

    //turn off volume
    public void TurnOffVol()
    {
        audioMixer.SetFloat("Master", ConvertToMixer( 0.0001f));
    }

    //find audio source on start
    private void Start()
    {
        BGM = transform.GetChild(0).GetComponent<AudioSource>();
        SE = transform.GetChild(1).GetComponent<AudioSource>();
    }

    /// <summary>
    /// play sound effect
    /// </summary>
    /// <param name="index">se number</param>
    public void PlaySe(int index)
    {
        SE.PlayOneShot(SE_list[index]);
    }

    /// <summary>
    /// playBgm
    /// </summary>
    /// <param name="index"> BGM number</param>
    public void PlayBGM(int index)
    {
        BGM.loop = true;
        BGM.clip = BGM_list[index];
        BGM.Play();
    }


    /// <summary>
    /// convert float to mixer value
    /// </summary>
    /// <param name="value"> volume percent </param>
    /// <returns></returns>
    private float ConvertToMixer(float value)
    {
        float result = Mathf.Log10(value) * 20;

        return result;
    }


    /// <summary>
    /// change se volume
    /// </summary>
    /// <param name="value">SE value </param>
    public void ChangeSe(float value)
    {
        audioMixer.SetFloat("SE",ConvertToMixer(value));
        PlayerPrefs.SetFloat("SE", value);
    }

    /// <summary>
    /// change bgm volume
    /// </summary>
    /// <param name="value">volume value</param>
    public void ChangeBGM(float value)
    {
        audioMixer.SetFloat("BGM", ConvertToMixer(value));
        PlayerPrefs.SetFloat("BGM", value);
    }
    /// <summary>
    /// change master VOlume
    /// </summary>
    /// <param name="value"> volume value</param>
    public void ChangeMaster(float value)
    {
        audioMixer.SetFloat("Master", ConvertToMixer(value));
        PlayerPrefs.SetFloat("Master", value);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
