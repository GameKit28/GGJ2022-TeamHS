using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public ParticleSystem particles;

    public void PlayRespawn()
    {
        particles.Play();
    }
}
