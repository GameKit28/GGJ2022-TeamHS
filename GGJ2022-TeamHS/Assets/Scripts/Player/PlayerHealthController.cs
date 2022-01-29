using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public interface IHealthEventReceiver : IEventSystemHandler
{
    void OnPlayerDamageStart();
    void OnPlayerDamageEnd();
    void PlayerDefeated();
}

public class PlayerHealthController : MonoBehaviour, ILightEventReceiver
{
    public int allowedPointsInSun = 1;

    [SerializeReference]
    public List<IHealthEventReceiver> healthEventReceivers = new List<IHealthEventReceiver>();

    private int numberInSun = 0;

    void Start()
    {
        healthEventReceivers = new List<IHealthEventReceiver>(healthEventReceivers);
    }

    void ILightEventReceiver.OnSunlightEnter()
    {
        if(numberInSun == 0)
        {
            healthEventReceivers.ForEach(i => i.OnPlayerDamageStart());
        }
        numberInSun++;

        if(numberInSun > allowedPointsInSun)
        {
            healthEventReceivers.ForEach(i => i.PlayerDefeated());
        }
    }

    void ILightEventReceiver.OnSunlightExit()
    {
        numberInSun--;
        if(numberInSun == 0)
        {
            healthEventReceivers.ForEach(i => i.OnPlayerDamageEnd());
        }
    }
}
