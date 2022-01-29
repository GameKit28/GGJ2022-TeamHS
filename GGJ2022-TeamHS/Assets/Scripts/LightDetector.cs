using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetector : MonoBehaviour
{
    private const int detectionInterval = 10;

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % detectionInterval == 0)
        {
            SunManager.InSunlight(transform.position);
        }
    }
}
