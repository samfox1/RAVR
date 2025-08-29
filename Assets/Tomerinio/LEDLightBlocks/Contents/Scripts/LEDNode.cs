// LEDNode.cs (Updated Version)
using UnityEngine;
using System.Collections; // Required for IEnumerator

public class LEDNode : MonoBehaviour
{
    private Renderer rend;
    private Material instanceMaterial; // Use an instance material to avoid affecting other objects

    // nextNode and prevNode are NOT public fields here anymore

    public Color pulseColor = Color.white;
    public float lightDuration = 0.1f;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("LEDNode: No Renderer found on " + gameObject.name + ". Please ensure this GameObject has a Mesh Renderer.", this);
            enabled = false;
            return;
        }

        // Get an instance of the material to avoid modifying the shared asset material
        instanceMaterial = rend.material;
        if (instanceMaterial == null)
        {
            Debug.LogError("LEDNode: No Material found on Renderer for " + gameObject.name + ".", this);
            enabled = false;
            return;
        }

        // Ensure the material supports emission. Most Standard shaders do.
        if (!instanceMaterial.IsKeywordEnabled("_EMISSION"))
        {
            instanceMaterial.EnableKeyword("_EMISSION");
        }
        instanceMaterial.SetColor("_EmissionColor", Color.black); // Start with emission off
    }

    public void TriggerLight()
    {
        StopAllCoroutines(); // Stop any ongoing blink for this node
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        // Ensure emission is on and set color
        instanceMaterial.SetColor("_EmissionColor", pulseColor);

        // Wait for the duration of the light pulse
        yield return new WaitForSeconds(lightDuration);

        // Turn off emission
        instanceMaterial.SetColor("_EmissionColor", Color.black); // Set emission to black to turn off
    }
}