using UnityEngine;

public class DancerBPMSync : MonoBehaviour
{
    private Animator dancerAnimator; // Renamed for clarity
    private BPMManager bpmManager;    // Reference to the central BPM manager

    [Header("Animation Sync Settings")]
    [Tooltip("How many *musical beats* does one full loop of this specific dance animation take?")]
    [Range(0.1f, 16.0f)] // A reasonable range for beats per loop
    public float beatsPerAnimationLoop = 4.0f; // E.g., a 4/4 dance loop takes 4 beats

    void Awake()
    {
        // Get the Animator component on this GameObject
        dancerAnimator = GetComponent<Animator>();
        if (dancerAnimator == null)
        {
            Debug.LogError("DancerBPMSync: No Animator component found on this GameObject. Disabling script.", this);
            enabled = false; // Disable the script if no Animator is found
            return;
        }

        // Find the global BPMManager in the scene
        bpmManager = FindObjectOfType<BPMManager>();
        if (bpmManager == null)
        {
            Debug.LogError("DancerBPMSync: No BPMManager found in the scene. Please add one. Disabling script.", this);
            enabled = false; // Disable if no BPMManager to sync with
            return;
        }

        // Initially set the animation speed based on the current BPM
        UpdateAnimationSpeed();
    }

    void OnEnable()
    {
        // Subscribe to the OnBeat event from BPMManager
        // This will allow us to react if BPM changes dynamically (though we're mostly doing continuous speed)
        BPMManager.OnBeat += OnBeatHandler;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks when the GameObject is disabled or destroyed
        BPMManager.OnBeat -= OnBeatHandler;
    }

    // This method is called by the BPMManager every time a beat occurs.
    // While we're continuously adjusting speed, this could be used for beat-triggered effects.
    private void OnBeatHandler()
    {
        // For a continuous loop, the speed is already set in UpdateAnimationSpeed().
        // If you wanted a *pulse* or *momentary reaction* on each beat, you'd put that logic here.
        // Example: If you had a "Beat" trigger in your Animator Controller:
        // dancerAnimator.SetTrigger("Beat");
    }

    // This function calculates and sets the Animator's overall speed
    // to match the desired BPM and the animation's beats per loop.
    private void UpdateAnimationSpeed()
    {
        if (bpmManager == null || dancerAnimator == null) return;

        // Calculate beats per second from the global BPM
        float beatsPerSecond = bpmManager.bpm / 60f;

        // Calculate the target speed for the Animator
        // If beatsPerAnimationLoop is 4, and beatsPerSecond is 2 (120 BPM):
        // Target speed = 2 beats/second / 4 beats/loop = 0.5 loops/second
        // So, the Animator's speed needs to be 0.5 if the animation clip's natural speed is 1 loop/second.
        dancerAnimator.speed = beatsPerSecond / beatsPerAnimationLoop;
    }

    // Optional: If BPM can change during runtime, you'd need to re-calculate speed.
    // You could call UpdateAnimationSpeed() whenever BPMManager's BPM changes.
    // For simplicity, we'll assume BPM is mostly static after start, or handled by the BPMManager's OnBeat.
    // However, if the BPMManager uses a SetBPM method, you could also call UpdateAnimationSpeed directly from there
    // or set up an event for BPM changes.
}