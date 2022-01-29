using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float playerSpeed = 0.005f;
    public Camera trackingCamera;

    private Vector3 movementPlaneScalar = new Vector3(1, 0, 1); //Up is the Y Axis. Camera moves along the X,Z plane

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));

        Vector3 cameraPlanalForward = Vector3.Scale(trackingCamera.transform.forward, movementPlaneScalar).normalized;
        Vector3 cameraPlanalRight = Vector3.Scale(trackingCamera.transform.right, movementPlaneScalar).normalized;
        Vector3 movementVector = (cameraPlanalForward * inputVector.y) + (cameraPlanalRight * inputVector.x);

        transform.Translate(movementVector * playerSpeed);
    }
}
