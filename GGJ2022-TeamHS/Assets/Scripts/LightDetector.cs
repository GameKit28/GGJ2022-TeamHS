using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ILightEventReceiver : IEventSystemHandler
{
    void OnSunlightEnter();
    void OnSunlightExit();
}

[System.Serializable]
public class LightDetector : MonoBehaviour
{
    private const int detectionInterval = 10;
    private bool isInSunlight = false;

    [SerializeField]
    public List<ILightEventReceiver> lightEventReceivers = new List<ILightEventReceiver>();

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % detectionInterval == 0)
        {
            bool newSunlightValue = SunManager.InSunlight(transform.position);
            if (newSunlightValue != isInSunlight)
            {
                if (newSunlightValue)
                {
                    lightEventReceivers.ForEach(i => i.OnSunlightEnter());
                }
                else
                {
                    lightEventReceivers.ForEach(i => i.OnSunlightExit());
                }
            }
            isInSunlight = newSunlightValue;
        }
    }

    public bool IsInSunlight()
    {
        return isInSunlight;
    }
}
