using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BPMManager : MonoBehaviour
{
    [Header("BPM Settings")]
    [Range(60, 200)] // Allows setting BPM between 60 and 200
    public float bpm = 123f; // BPM Assignment - change l8r
    public float beatOffset = 0f; // Time in seconds to offset the beat start

    private float beatInterval; // Time in seconds between each beat
    private float nextBeatTime;

    // Event for other scripts to subscribe to
    public delegate void OnBeatDelegate();
    public static event OnBeatDelegate OnBeat;

    // Reference to the AudioSource playing the music
    [Header("Audio Source (Optional)")]
    public AudioSource musicSource;

    void Start()
    {
        UpdateBPMVariables();
        nextBeatTime = Time.time + beatOffset; // Set initial beat time
    }

    void Update()
    {
        if (Time.time >= nextBeatTime)
        {
            if (OnBeat != null) // Check if anyone is listening
            {
                OnBeat(); // Trigger beat event
            }
            nextBeatTime += beatInterval; // Schedule next beat
        }
    }

    // Method for changing BPM during runtime
    public void SetBPM(float newBPM)
    {
        bpm = newBPM;
        UpdateBPMVariables();
        // TODO: Adjust nextBeatTime to sync immediately
        nextBeatTime = Time.time + beatInterval; // This will re-sync to the new beat
    }

    private void UpdateBPMVariables()
    {
        beatInterval = 60f / bpm; // Calculate seconds per beat
    }

    // If you want to sync with actual music playback
    public void SyncWithMusic(AudioSource audioSource)
    {
        musicSource = audioSource;
        if (musicSource != null)
        {
            // Calculate current beat progress if music is already playing
            // This is a simplified sync, actual beat tracking is more complex
            float currentSongTime = musicSource.time;
            float beatsPassed = currentSongTime / beatInterval;
            float beatsToNext = Mathf.Ceil(beatsPassed) - beatsPassed;
            nextBeatTime = Time.time + (beatsToNext * beatInterval);
        }
    }
}