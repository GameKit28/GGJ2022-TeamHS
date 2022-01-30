using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HealthAudio : MonoBehaviour, IHealthEventReceiver
{
    AudioSource audioSource;

    [SerializeField] private AudioClip burningSound;
    [SerializeField] private AudioClip deathSound;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.volume = 0f;
    }

    void IHealthEventReceiver.OnPlayerDamageEnd()
    {
        if(audioSource.clip == burningSound) audioSource.volume = 0;
    }

    void IHealthEventReceiver.OnPlayerDamageStart()
    {
        audioSource.clip = burningSound;
        audioSource.loop = true;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    void IHealthEventReceiver.PlayerDefeated()
    {
        audioSource.clip = deathSound;
        audioSource.loop = false;
        audioSource.volume = 1f;
        audioSource.PlayOneShot(deathSound);
    }
}
