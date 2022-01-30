using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public ParticleSystem particles;
    public GameObject safetyZone;
    public void PlayRespawn()
    {
        particles.Play();
    }

    public void EnableSafetyZone(bool enabled)
    {
        safetyZone.SetActive(enabled);
    }
}
