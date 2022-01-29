using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform trackedObject;
    public Vector3 focalPoint;

    public float orbitRadius = 5f;
    public float cameraHeight = 2f;

    public float cameraSmootingFactor = 0.5f;

    private Vector3 cameraPlaneScalar = new Vector3(1, 0, 1); //Camera moves along the X,Z plane;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Temp
        focalPoint = trackedObject.position + Vector3.forward;

        Vector3 trackedObjectToFocalVector = Vector3.Scale(focalPoint - trackedObject.position, cameraPlaneScalar).normalized;
        Vector3 targetCameraPos = trackedObject.position + (-trackedObjectToFocalVector * orbitRadius) + (cameraHeight * Vector3.up);

        this.transform.position = Vector3.Lerp(this.transform.position, targetCameraPos, cameraSmootingFactor);
        this.transform.LookAt(focalPoint);
    }
}
