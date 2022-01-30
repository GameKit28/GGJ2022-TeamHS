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
    public int nextLevelIndex;

    private bool levelStarted = false;
    private bool endOfLevel = false;
    private InputAction toggleAction;

    public void Start()
    {
        toggleAction = player.GetComponent<PlayerInput>().actions["ToggleCamera"];
        toggleAction.performed += ToggleCamera;
        var displayString = toggleAction.GetBindingDisplayString();
        startText.text = $"Press {displayString} to Start!";
    }

    private void ToggleCamera(InputAction.CallbackContext obj)
    {
        if (endOfLevel) 
        {
            SceneManager.LoadScene(nextLevelIndex);
        };

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
            endOfLevel = true;
            mapCam.enabled = true;
            startText.text = $"Ya win bitch!";
            DOTween.Sequence()
                .Append(startText.DOColor(Color.white, 1.5f))
                .Append(startText.rectTransform.DOPunchScale(Vector3.one * 2, 3))
                .OnComplete(() => startText.text = $"Press {toggleAction.GetBindingDisplayString()} to Continue!");
        }
    }
}
