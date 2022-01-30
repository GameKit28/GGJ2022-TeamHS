using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DamageParticleManager : MonoBehaviour, IHealthEventReceiver
{
    private ParticleSystem particles;
    void Start() {
        particles = GetComponent<ParticleSystem>();
    }

    public void OnPlayerDamageEnd() {

    }

    public void OnPlayerDamageStart() {
        particles.Play();
    }

    public void PlayerDefeated() {

    }




}
