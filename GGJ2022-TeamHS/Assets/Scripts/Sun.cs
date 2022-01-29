using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Sun : MonoBehaviour
{
    public float rotationRate = 15f;

    // Start is called before the first frame update
    void Start()
    {
        SunManager.RegisterSun(this);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(50, Time.realtimeSinceStartup * rotationRate, 0);
    }

    public Vector3 GetShadowVector()
    {
        return transform.forward;
    }
}
