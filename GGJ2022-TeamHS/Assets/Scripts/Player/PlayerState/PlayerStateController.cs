using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;

public partial class PlayerStateController : MeFsm, IHealthEventReceiver
{
    public RespawnPoint respawnPoint;

    private const string SafetyZoneTag = "SafetyZone";

    void IHealthEventReceiver.OnPlayerDamageEnd()
    {
        //throw new System.NotImplementedException();
    }

    void IHealthEventReceiver.OnPlayerDamageStart()
    {
        //throw new System.NotImplementedException();
    }

    void IHealthEventReceiver.PlayerDefeated()
    {
        SwapState<DyingState>();
    }
}
