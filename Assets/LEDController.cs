using UnityEngine;
using System.Collections;

public class LEDController : MonoBehaviour
{

    [Header("LED Strip Configuration")]
    [Tooltip("Drag all your LEDNode GameObjects here in the order they should light up.")]
    public LEDNode[] ledNodes; // Array to hold all your LEDNode references

    private BPMManager bpmManager; // Reference to the BPMManager script on THIS GameObject!
    private AudioSource mainAudioSource; // Reference to the AudioSource on THIS GameObject!

    private float secondsPerBeat;
    private float nextBeatTime;
    private int currentIndex = 0; // Current index in the ledNodes array

    void Awake() // Changed from Start to Awake for earlier setup
    {
        // Get references to components on THIS GameObject
        bpmManager = GetComponent<BPMManager>();
        mainAudioSource = GetComponent<AudioSource>();

        if (bpmManager == null)
        {
            Debug.LogError("LEDController: BPMManager not found on this GameObject. Disabling script.", this);
            enabled = false;
            return;
        }
        if (mainAudioSource == null)
        {
            Debug.LogError("LEDController: AudioSource not found on this GameObject. Disabling script.", this);
            enabled = false;
            return;
        }
        if (ledNodes == null || ledNodes.Length == 0)
        {
            Debug.LogError("LEDController: No LEDNodes assigned to the array. Disabling script.", this);
            enabled = false;
            return;
        }

        // Initialize from BPMManager's values
        secondsPerBeat = 60f / bpmManager.bpm;
        nextBeatTime = Time.time + bpmManager.beatOffset; // Use BPMManager's offset
        
        // Ensure initial state of all lights is off
        foreach (LEDNode node in ledNodes)
        {
            if (node != null)
            {
                // This will set emission color to black through the Blink coroutine
                node.TriggerLight(); // Assuming TriggerLight also sets to black initially
            }
        }
    }

    void Update()
    {
        // Use the mainAudioSource and BPMManager's current BPM
        if (mainAudioSource.isPlaying && mainAudioSource.time >= nextBeatTime)
        {
            // Trigger the current LED node
            if (currentIndex >= 0 && currentIndex < ledNodes.Length && ledNodes[currentIndex] != null)
            {
                ledNodes[currentIndex].TriggerLight();
            }

            // Move to the next node in the circular loop
            currentIndex++;
            if (currentIndex >= ledNodes.Length)
            {
                currentIndex = 0; // Loop back to the beginning of the array
            }

            // Schedule the next beat based on BPMManager's current BPM
            secondsPerBeat = 60f / bpmManager.bpm; // Recalculate if BPM changes dynamically
            nextBeatTime += secondsPerBeat;
        }
    }

}