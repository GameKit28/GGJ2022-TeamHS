using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    public float playerSpeed = 0.005f;
    public Camera trackingCamera;

    private Vector3 movementPlaneScalar = new Vector3(1, 0, 1); //Up is the Y Axis. Camera moves along the X,Z plane
    private Vector2 inputVector = Vector2.zero;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue input)
    {
        inputVector = input.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPlanalForward = Vector3.Scale(trackingCamera.transform.forward, movementPlaneScalar).normalized;
        Vector3 cameraPlanalRight = Vector3.Scale(trackingCamera.transform.right, movementPlaneScalar).normalized;
        Vector3 movementVector = (cameraPlanalForward * inputVector.y) + (cameraPlanalRight * inputVector.x);

        body.velocity = (movementVector * playerSpeed);
        var newRotation = Quaternion.Euler(0, trackingCamera.transform.eulerAngles.y, 0);
        body.transform.rotation = Quaternion.Lerp(body.transform.rotation, newRotation, Time.deltaTime * 3);
    }
}
