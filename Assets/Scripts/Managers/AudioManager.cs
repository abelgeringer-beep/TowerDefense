using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Audio[] audios;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        for(int i = 0; i < audios.Length; i++)
        {
            audios[i].source = gameObject.AddComponent<AudioSource>();
            audios[i].source.clip = audios[i].clip;
            audios[i].source.volume = audios[i].volume;
            audios[i].source.pitch = audios[i].pitch;
            audios[i].source.loop = audios[i].loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    private void Play(string name)
    {
        Audio a = Array.Find(audios, _ => _.name == name);
        if (a == null)
            return;

        a.source.Play();
    }
}
