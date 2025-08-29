using UnityEngine;

public class DoorTriggerSimple : MonoBehaviour
{
    public Transform doorPivot;       // Assign the door pivot in the Inspector
    public float openAngle = 90f;     // The angle to open the door
    public float openSpeed = 2f;      // The speed of rotation

    private Quaternion closedRotation;
    private Quaternion openRotationPositive;
    private Quaternion openRotationNegative;
    private bool isPlayerInside = false;

    private Transform playerTransform;

    void Start()
    {
        if (doorPivot == null)
        {
            Debug.LogError("Assign the doorPivot in inspector");
            return;
        }

        closedRotation = doorPivot.rotation;
        // Corrected to rotate on the Z-axis
        openRotationPositive = closedRotation * Quaternion.Euler(0, 0, openAngle);
        openRotationNegative = closedRotation * Quaternion.Euler(0, 0, -openAngle);
    }

    void Update()
    {
        if (doorPivot == null) return;

        Quaternion targetRotation = closedRotation;

        if (isPlayerInside)
        {
            if (playerTransform != null)
            {
                // Now checking the player's Z position to determine rotation direction
                if (playerTransform.position.z > doorPivot.position.z)
                {
                    targetRotation = openRotationPositive;
                }
                else
                {
                    targetRotation = openRotationNegative;
                }
            }
        }

        doorPivot.rotation = Quaternion.Slerp(doorPivot.rotation, targetRotation, Time.deltaTime * openSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isPlayerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerTransform = null;
        }
    }
}