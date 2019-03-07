using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public static SoundEffectController instance = null; // we have to make an instance so that other scripts can call it
    private AudioClip[] jumpingEffects;
    public AudioClip jump1;
    public AudioClip jump2;
    public AudioClip jump3;

    private AudioClip[] damageEffects;
    public AudioClip damage1;
    public AudioClip damage2;
    public AudioClip damage3;
    public AudioClip damage4;

    private AudioClip[] stickEffects;
    public AudioClip stick1;
    public AudioClip stick2;

    private AudioSource source;
    private float volume;

    void Awake()
    {
        // singleton 
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        source = GetComponent<AudioSource>();
        volume = 0.25f;
        jumpingEffects = new AudioClip[] { jump1, jump2, jump3 };
        damageEffects = new AudioClip[] { damage1, damage2, damage3, damage4 };
        stickEffects = new AudioClip[] { stick1, stick2 };
    }

    AudioClip pickRandomClip(AudioClip[] clips)
    {
        int index = Random.Range(0, clips.Length);
        return clips[index];
    }

    // this function is from https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/audio-and-sound-manager
    // RandomizeSoundEffect chooses randomly between various audio clips and slightly changes their pitch.
    private void RandomizeSoundEffect(params AudioClip[] clips)
    {
        float lowPitchRange = 0.95f;
        float highPitchRange = 1.05f;
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        source.pitch = randomPitch;
        source.clip = clips[randomIndex];
        source.Play();
    }

    public void Jump()
    {
        AudioClip jumpClip = pickRandomClip(jumpingEffects);
        source.PlayOneShot(jumpClip, volume);
    }

    public void Damage()
    {
        AudioClip damageClip = pickRandomClip(damageEffects);
        source.PlayOneShot(damageClip, volume);
    }

    public void Stick()
    {
        AudioClip stickClip = pickRandomClip(stickEffects);
        if(!source.isPlaying)
        {
            source.PlayOneShot(stickClip, volume);
        }
    }
}