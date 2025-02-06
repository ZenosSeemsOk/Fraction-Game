using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class musicManager : MonoBehaviour
{
    public float sfxVolume;
    public float musicVolume;
    public AudioSource a_Source;
    public AudioClip bgMusic;
    public AudioClip buttonClick;
    public AudioClip levelClick;
    public AudioClip subLevelClick;
    public AudioClip levelComplete;
    public AudioClip bow_Stretch;
    public AudioClip bow_Release;
    public AudioClip arrow_correct;
    public AudioClip arrow_fail;
    public AudioClip cannon_hit;
    public AudioClip cannon_release;
    public AudioClip fraction_Display;
    public AudioClip number_Placed;
    public AudioClip puzzle_Snapped;
    public static musicManager Instance;

    private Dictionary<string, AudioClip> audioDic;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);


        a_Source.loop = true;
    }
    void Start()
    {
        // Initialize the audio dictionary
        audioDic = new Dictionary<string, AudioClip>
    {
        { "bgMusic", bgMusic },
        { "buttonClick", buttonClick },
        { "levelClick", levelClick },
        { "subLevelClick", subLevelClick },
        { "levelComplete", levelComplete },
        { "bow_Stretch", bow_Stretch },
        { "bow_Release", bow_Release },
        { "arrow_correct", arrow_correct },
        { "arrow_fail", arrow_fail },
        { "cannon_hit", cannon_hit },
        { "cannon_release", cannon_release },
        { "fraction_Display", fraction_Display },
        { "number_Placed", number_Placed },
        { "puzzle_Snapped", puzzle_Snapped }
    };
    }

    public void PlayBG()
    {
        a_Source.clip = bgMusic;
        a_Source.volume = musicVolume;
        a_Source.Play();
    }

    public void PlayOnceClip(string clipName)
    {
        a_Source.volume = sfxVolume;
        if(audioDic.TryGetValue(clipName, out AudioClip audioClip))
        {
            a_Source.PlayOneShot(audioClip);
        }
    }


}
