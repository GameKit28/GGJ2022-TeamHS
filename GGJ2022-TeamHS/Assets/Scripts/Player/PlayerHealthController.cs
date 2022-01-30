using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;


public interface IHealthEventReceiver : IEventSystemHandler
{
    void OnPlayerDamageStart();
    void OnPlayerDamageEnd();
    void PlayerDefeated();
}

public class PlayerHealthController : MonoBehaviour, ILightEventReceiver
{
    public int allowedPointsInSun = 1;

    public List<Component> healthEventReceivers = new List<Component>();
    private List<IHealthEventReceiver> _healthEventReceivers;

    private int numberInSun = 0;

    private bool immortal = false;

    void Start()
    {
        _healthEventReceivers = healthEventReceivers.Select(i => i.GetComponent<IHealthEventReceiver>()).ToList();
    }

    void ILightEventReceiver.OnSunlightEnter()
    {
        if(numberInSun == 0 && !immortal)
        {
            _healthEventReceivers.ForEach(i => i.OnPlayerDamageStart());
        }
        numberInSun++;

        if(numberInSun > allowedPointsInSun && !immortal)
        {
            _healthEventReceivers.ForEach(i => i.PlayerDefeated());
        }
    }

    void ILightEventReceiver.OnSunlightExit()
    {
        numberInSun--;

        if (numberInSun == 0)
        {
            _healthEventReceivers.ForEach(i => i.OnPlayerDamageEnd());
        }
    }

    public void SetImmortal(bool immortal)
    {
        this.immortal = immortal;
        if(immortal == false && numberInSun > allowedPointsInSun)
        {
            _healthEventReceivers.ForEach(i => i.PlayerDefeated());
        }
    }
}
