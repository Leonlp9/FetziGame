using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioSound[] sounds;
    public static AudioManager instance;

    public float PlayerVolume;
    public float PlayerSFX;

    private void Start()
    {
        PlayerVolume = PlayerPrefs.GetFloat("volume", 0.5f);
        PlayerSFX = PlayerPrefs.GetFloat("volumeEffects", 0.5f);
        Application.runInBackground = true;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (AudioSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume * PlayerVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }

    private void Update()
    {

        foreach (AudioSound s in sounds)
        {
            s.source.volume = s.volume * PlayerVolume * PlayerSFX;
        }

    }

    public void Play(string name)
    {
        AudioSound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }


}