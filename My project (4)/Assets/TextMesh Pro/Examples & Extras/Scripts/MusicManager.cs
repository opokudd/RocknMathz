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
        if (musicSource != null && menuMusic != null && !musicSource.isPlaying)
        {
            musicSource.clip = menuMusic; // Assign the music
            musicSource.loop = true; // Loop the music
            musicSource.Play(); // Start playing
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