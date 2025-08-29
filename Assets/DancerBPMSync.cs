using UnityEngine;

public class DancerBPMSync : MonoBehaviour
{
    private Animator dancerAnimator;
    private BPMManager bpmManager;

    [Header("Animation Sync Settings")]
    [Tooltip("How many *musical beats* does one full loop of this specific dance animation take?")]
    [Range(0.1f, 16.0f)] // Beat range
    public float beatsPerAnimationLoop = 4.0f;

    void Awake()
    {
        // Gets the Animator component on this GameObject
        dancerAnimator = GetComponent<Animator>();
        if (dancerAnimator == null)
        {
            Debug.LogError("DancerBPMSync: No Animator component found on this GameObject. Disabling script.", this);
            enabled = false; // Disable the script if no Animator
            return;
        }

        // Find the global BPMManager in the scene
        bpmManager = FindObjectOfType<BPMManager>();
        if (bpmManager == null)
        {
            Debug.LogError("DancerBPMSync: No BPMManager found in the scene. Please add one. Disabling script.", this);
            enabled = false; // Disables if no BPMManager to sync with
            return;
        }

        // Initially set the animation speed based on the current BPM
        UpdateAnimationSpeed();
    }

    void OnEnable()
    {
        // Subscribes to the OnBeat event from BPMManager
        // This will allow for dynamix BPM changes
        BPMManager.OnBeat += OnBeatHandler;
    }

    void OnDisable()
    {
        // Unsubscribes to prevent memory leaks when Gameobject disabled
        BPMManager.OnBeat -= OnBeatHandler;
    }

    // This method is called by the BPMManager when beat occurs
    private void OnBeatHandler()
    {
        // For a *pulse* or *momentary reaction* on each beat
        // dancerAnimator.SetTrigger("Beat");
    }

    // Calculates and sets the Animator's overall speed based on desired BPM and the animation's beats per loop
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

}