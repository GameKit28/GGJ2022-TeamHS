using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Determines if a target is too close to the AI. If so, the target should be automatically found.
    /// </summary>
    public float minDetectionDistance = 3f;

    /// <summary>
    /// Determines the maximum distance that the AI can "see" the player. If the player is outside of this range they will be undetectable.
    /// </summary>
    public float maxDetectionDistance = 15f;

    /// <summary>
    /// Determines how wide the "arc" is of the AI's vision. This value represents one "eye" so it will be doubled later.
    /// </summary>
    public float fieldOfViewRange = 48f;

    /// <summary>
    /// Determines the layer(s) that the NPC can see through. For example, the "Floor" layer is not a layer that will return true if any raycasting is done on it.
    /// </summary>
    public LayerMask detectionLayers;

    /// <summary>
    /// Determines whether the editor should display the vision based gizmos or not.
    /// </summary>
    public bool showGizmos = true;

    /// <summary>
    /// The distance between the AI and the target.
    /// </summary>
    private float distanceToTarget;

    /// <summary>
    /// The transform of the AI character's EyeLineTransform component.
    /// </summary>
    [SerializeField]
    private Transform aiCharacterEyeLineTransform;

    /// <summary>
    /// The player character that the AI will be interacting with.
    /// </summary>
    private Transform playerTransform;

    /// <summary>
    /// Is the target in the Conic field of View?
    /// </summary>
    private bool inVisionCone;
    #endregion

    public Transform PlayerTransform => playerTransform ?? FindObjectOfType<PlayerMovementController>().transform;

    public bool InVisionCone
    {
        get
        {
            return inVisionCone;
        }
    }
    /// <summary>
    /// The AI searches for a gameobject tagged "Player" and returns true when the player has been found.
    /// </summary>
    /// <returns></returns>
    public Transform CanSeePlayer()
    {
        var rayhit = new RaycastHit();
        var rayDirection = PlayerTransform.position - transform.position;
        rayDirection.Normalize();

        //Debug.Log($"Difference: {Vector3.Angle(rayDirection, transform.forward)}, fov: {fieldOfViewRange * 0.5f}");
        //Debug.DrawRay(aiCharacterEyeLineTransform.position, rayDirection, Color.blue);
        if (Vector3.Angle(rayDirection, transform.forward) <= fieldOfViewRange * 0.5f) //Direction
        {
            if (Physics.Linecast(aiCharacterEyeLineTransform.position, PlayerTransform.position, out rayhit, detectionLayers) && Vector3.Distance(transform.position, PlayerTransform.position) < maxDetectionDistance) // LOS and Distance
            {
                if (rayhit.transform.tag == "Player")
                {
                    inVisionCone = true;
                    return rayhit.transform;
                }
            }
            else
            {
                inVisionCone = false;
            }
        }
        else
        {
            inVisionCone = false;
        }

        //var hitColliders = Physics.OverlapSphere(aiCharacterEyeLineTransform.position, minDetectionDistance, detectionLayers);
        return null;
    }

    public bool IsNearPlayer()
    {
        return Physics.OverlapSphere(aiCharacterEyeLineTransform.position, minDetectionDistance, detectionLayers).Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;

            var lineHeight = 0f;
            var theta = 0f;
            var x = minDetectionDistance * Mathf.Cos(theta);
            var z = minDetectionDistance * Mathf.Sin(theta);
            var pos = aiCharacterEyeLineTransform.position + new Vector3(x, lineHeight, z);
            var lastPos = pos;

            var direction = transform.forward * maxDetectionDistance;
            var leftRayRotation = Quaternion.AngleAxis(-(fieldOfViewRange / 2), Vector3.up);
            var leftRayDirection = leftRayRotation * transform.forward;
            var rightRayRotation = Quaternion.AngleAxis((fieldOfViewRange / 2), Vector3.up);
            var rightRayDirection = rightRayRotation * transform.forward;

            Gizmos.DrawRay(aiCharacterEyeLineTransform.position, direction);
            Gizmos.DrawRay(aiCharacterEyeLineTransform.position, leftRayDirection * maxDetectionDistance);
            Gizmos.DrawRay(aiCharacterEyeLineTransform.position, rightRayDirection * maxDetectionDistance);

            for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
            {
                x = minDetectionDistance * Mathf.Cos(theta);
                z = minDetectionDistance * Mathf.Sin(theta);
                Vector3 newPos = aiCharacterEyeLineTransform.position + new Vector3(x, lineHeight, z);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }
            Gizmos.DrawLine(pos, lastPos);
        }
    }
}
