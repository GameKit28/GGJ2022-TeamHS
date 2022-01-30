using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HealthAudio : MonoBehaviour, IHealthEventReceiver
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
    }

    void IHealthEventReceiver.OnPlayerDamageEnd()
    {
        audioSource.volume = 0f;
    }

    void IHealthEventReceiver.OnPlayerDamageStart()
    {
        audioSource.volume = 1f;
    }

    void IHealthEventReceiver.PlayerDefeated()
    {
        //do nothing
    }
}
