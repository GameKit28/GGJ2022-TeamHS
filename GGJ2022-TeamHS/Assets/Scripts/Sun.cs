using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Sun : MonoBehaviour
{
    public float rotationRate = 15f;
    private Light light;
    private Coroutine SunLerper;
    private float originalLightTemp;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        originalLightTemp = light.colorTemperature;
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
    public void ChangeSunTemp(float temp) {
        SunLerper = StartCoroutine(LerpSunColor(temp));
    }

    public void ResetSunTemp() {
        SunLerper = StartCoroutine(LerpSunColor(originalLightTemp));

    }

    private void EndIt() {
        StopCoroutine(SunLerper);
    }
    private IEnumerator LerpSunColor(float newTemp) {
        float currTemp = light.colorTemperature;
        float t = 0f;

        while (t <= 1f) {
            t += Time.deltaTime / 4;
            light.colorTemperature = Mathf.Lerp(currTemp, newTemp, t);
            yield return null;
        }

        EndIt();
    }
}
