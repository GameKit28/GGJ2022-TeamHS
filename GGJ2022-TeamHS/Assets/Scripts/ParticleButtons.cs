using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ParticleButtons : Button
{
    public ParticleSystem particles;

    void Start()
    {
        if (particles == null) return;
        particles.gameObject.SetActive(false);
    }

    public void Highlighted ()
    {
        if (particles == null) return;
        particles.gameObject.SetActive(true);
        targetGraphic.enabled = true;
    }

    public void Normal()
    {
        if (particles == null) return;
        particles.gameObject.SetActive(false);
        targetGraphic.enabled = false;
    }

    //void Update()
    //{
    //    if (particles == null || !Application.isPlaying) return;
    //    if (IsHighlighted() && particles.isStopped)
    //    {
    //        particles.Play();
    //    }
    //    else if(particles.isPlaying)
    //    {
    //        particles.Stop();
    //    }
    //}
}
