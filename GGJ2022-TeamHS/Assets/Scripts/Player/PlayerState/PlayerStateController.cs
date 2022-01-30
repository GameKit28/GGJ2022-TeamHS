using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeEngine.FsmManagement;

public partial class PlayerStateController : MeFsm, IHealthEventReceiver
{
    public RespawnPoint respawnPoint;

    private Material mat;
    private const string SafetyZoneTag = "SafetyZone";

    private Coroutine JiggleCoroutine;

    protected override void Start()
    {
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;

    }

    void IHealthEventReceiver.OnPlayerDamageEnd()
    {
        //throw new System.NotImplementedException();
    }

    void IHealthEventReceiver.OnPlayerDamageStart()
    {
        if (mat == null) {
            mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        }

        if (JiggleCoroutine != null) {
            StopCoroutine(JiggleCoroutine);
            JiggleCoroutine = StartCoroutine(Jiggle());
        }

    }

    void IHealthEventReceiver.PlayerDefeated()
    {
        SwapState<DyingState>();
    }

    private void StopThis() {
        StopCoroutine(JiggleCoroutine);

    }

    private IEnumerator Jiggle() {
        float tick = 0.8f;
        mat.SetFloat("_Brightness", 0.1f);

        while (tick > 0){
            tick -= Time.deltaTime;

            float intens = Mathf.Clamp(Random.Range(0f, 1f), 0.35f, 1);
            mat.SetFloat("_ColorIntensity", intens);
            Debug.Log($"intensity is: {intens}");

            float width = Mathf.Clamp(Random.Range(3f, 20f), 3f, 20f);
            mat.SetFloat("_OutlineWidth", width);
            Debug.Log($"width is: {width}");

            yield return null;
        }
        mat.SetFloat("_Brightness", 0.3f);
        mat.SetFloat("_ColorIntensity", 0f);
        mat.SetFloat("_OutlineWidth", 4.7f);
        StopThis();

    }
}
