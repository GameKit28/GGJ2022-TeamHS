using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HealthAudio : MonoBehaviour, IHealthEventReceiver
{
    void IHealthEventReceiver.OnPlayerDamageEnd()
    {
        GetComponent<AudioSource>().volume = 0f;
    }

    void IHealthEventReceiver.OnPlayerDamageStart()
    {
        GetComponent<AudioSource>().volume = 1f;
    }

    void IHealthEventReceiver.PlayerDefeated()
    {
        //Do nothing
    }
}
