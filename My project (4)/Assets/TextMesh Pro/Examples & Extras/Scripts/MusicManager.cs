using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance; // Singleton instance
    public AudioSource musicSource; // AudioSource component to play music
    public AudioClip[] musicTracks; // Array of music tracks
    public int currentTrackIndex = 0; // Currently selected track index

    private void Awake()
    {
        // Singleton pattern to persist music across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this GameObject
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    private void Start()
    {
        PlayCurrentTrack();
    }

    public void PlayCurrentTrack()
    {
        if (musicTracks != null && currentTrackIndex >= 0 && currentTrackIndex < musicTracks.Length)
        {
            musicSource.clip = musicTracks[currentTrackIndex];
            musicSource.loop = true;
            musicSource.Play();
            Debug.Log("Playing track: " + musicTracks[currentTrackIndex].name);
        }
        else
        {
            Debug.LogError("Invalid track index or tracks not assigned.");
        }
    }

    public void SetTrack(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < musicTracks.Length)
        {
            currentTrackIndex = trackIndex;
            PlayCurrentTrack();
        }
    }
}
