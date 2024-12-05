using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



    public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance; // Singleton instance
    public AudioSource musicSource; // AudioSource component to play music
    public AudioClip menuMusic; // Assign the menu music clip in the Inspector

    private void Awake()
    {
        // Singleton pattern to persist music across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this GameObject
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    private void Start()
{
    if (menuMusic != null)
    {
        Debug.Log("Menu music clip assigned: " + menuMusic.name);
    }
    else
    {
        Debug.LogError("Menu music clip not assigned!");
    }

    if (musicSource != null)
    {
        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.Play();
        Debug.Log("Music started playing.");
    }
    else
    {
        Debug.LogError("AudioSource not assigned!");
    }
}


    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}