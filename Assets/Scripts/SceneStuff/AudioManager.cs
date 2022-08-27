using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    // Singleton
    public static AudioManager instance;

    void Awake()
    {
        // Don't allow a second AudioManager to exist
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Keep the old AudioManager, so the sound doesn't get restarted
        DontDestroyOnLoad(gameObject);

        // Make an audio source for all our sounds
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("BackgroundMusic");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

}
