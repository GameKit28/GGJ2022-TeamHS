using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunManager : MonoBehaviour
{
    private static SunManager instance = null;
    private int ShadowLayerMask = 10;

    List<Sun> activeSuns = new List<Sun>();

    void Awake()
    {
        instance = this;
    }

    public static void RegisterSun(Sun sun)
    {
        instance.activeSuns.Add(sun);
    }

    /// <summary>
    /// Returns True if the provided point is within view of any of the suns.
    /// </summary>
    public static bool InSunlight(Vector3 point)
    {
        RaycastHit raycastHit;
        Ray ray;
        foreach (Sun sun in instance.activeSuns)
        {
            ray = new Ray(point, -sun.GetShadowVector());
            if (Physics.Raycast(ray, out raycastHit, instance.ShadowLayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.blue);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns the Suns that are within view of the provided point.
    /// </summary>
    public static List<Sun> SunsInView(Vector3 point)
    {
        List<Sun> visibleSuns = new List<Sun>();
        RaycastHit raycastHit;
        Ray ray;
        foreach(Sun sun in instance.activeSuns)
        {
            ray = new Ray(point, -sun.GetShadowVector());
            if (Physics.Raycast(ray, out raycastHit, instance.ShadowLayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.blue);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
                visibleSuns.Add(sun);
            }
        }
        return visibleSuns;
    }
}
