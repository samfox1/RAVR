using UnityEngine;

public class BPMFlasher : MonoBehaviour
{
    public AudioSource audioSource;
    public Light lightToFlash;
    public float bpm = 120f; // Set the beats per minute here

    private float secondsPerBeat;
    private float timer;
    private bool isLightOn = false;

    void Start()
    {
        if (audioSource == null || lightToFlash == null)
        {
            Debug.LogError("AudioSource or LightToFlash not assigned!");
            return;
        }

        secondsPerBeat = 60f / bpm;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= secondsPerBeat)
        {
            // Reset the timer and flash the light
            timer -= secondsPerBeat;
            
            isLightOn = !isLightOn;
            lightToFlash.enabled = isLightOn;
        }
    }
}