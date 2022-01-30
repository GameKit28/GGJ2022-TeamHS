using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

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

    public List<Component> lightEventReceivers = new List<Component>();
    private List<ILightEventReceiver> _lightEventReceivers;

    private void Start()
    {
        _lightEventReceivers = lightEventReceivers.Select(i => i.GetComponent<ILightEventReceiver>()).ToList();
    }

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
                    _lightEventReceivers.ForEach(i => i.OnSunlightEnter());
                }
                else
                {
                    _lightEventReceivers.ForEach(i => i.OnSunlightExit());
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
