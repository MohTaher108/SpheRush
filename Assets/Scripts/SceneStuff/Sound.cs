using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    // Sliders in a specific range for volume and pitch of the audio
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    // The source for playing the noise itself
    [HideInInspector]
    public AudioSource source;

}
