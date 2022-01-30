using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject player;

    public Enemy[] enemies;

    public CinemachineVirtualCamera followCam;
    public CinemachineVirtualCamera mapCam;
    public TMPro.TMP_Text startText;

    private bool levelStarted = false;
    private bool disableCamToggling = false;

    public void Start()
    {
        player.GetComponent<PlayerInput>().actions["ToggleCamera"].performed += ToggleCamera;
    }

    private void ToggleCamera(InputAction.CallbackContext obj)
    {
        if (disableCamToggling) return;

        if (!levelStarted) 
        {
            startText.DOColor(Color.clear, 1.5f).OnComplete(() =>
            {
                foreach(var enemy in enemies)
                {
                    enemy.gameObject.SetActive(true);
                }
            });
         }
        mapCam.enabled = !mapCam.enabled;
        levelStarted = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //TODO win logic here
            disableCamToggling = true;
            mapCam.enabled = true;
            startText.text = "Ya win bitch!";
            DOTween.Sequence()
                .Append(startText.DOColor(Color.white, 1.5f))
                .Append(startText.rectTransform.DOPunchScale(Vector3.one * 2, 3))
                .OnComplete(() => SceneManager.LoadScene(0));
        }
    }
}
