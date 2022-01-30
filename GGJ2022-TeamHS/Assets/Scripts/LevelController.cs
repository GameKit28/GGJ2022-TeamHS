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
    public CinemachineVirtualCamera endLevelCam;
    public TMPro.TMP_Text startText;
    public GameObject winStatePanel;
    public TMPro.TMP_Text bestTimeText;
    public int nextLevelIndex;

    private bool levelStarted = false;
    private bool endOfLevel = false;
    private InputAction toggleAction;

    private float levelCompletionTime;

    public void Start()
    {
        levelCompletionTime = 0;
        toggleAction = player.GetComponent<PlayerInput>().actions["ToggleCamera"];
        toggleAction.performed += ToggleCamera;
        var displayString = toggleAction.GetBindingDisplayString();
        startText.text = $"Press {displayString} to Start!";
    }

    private void OnDestroy()
    {
        toggleAction.performed -= ToggleCamera;
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

    private void Update()
    {
        if(levelStarted && !endOfLevel)
        {
            levelCompletionTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //TODO win logic here
            endLevelCam.enabled = true;
            followCam.enabled = false;
            mapCam.enabled = false;
            endOfLevel = true;
            mapCam.enabled = true;
            startText.text = $"Ya win bitch!";
            bestTimeText.text = $"{Mathf.RoundToInt(levelCompletionTime)} Seconds";
            winStatePanel.SetActive(true);
            DOTween.Sequence()
                .Append(startText.DOColor(Color.white, 1.5f))
                .Append(startText.rectTransform.DOPunchScale(Vector3.one * 2, 3))
                .Join(player.transform.DOSpiral(5, speed: 5))
                .Join(player.transform.DOMoveY(100, 5)
                .OnComplete(() => startText.text = $"Press {toggleAction.GetBindingDisplayString()} to Continue!"));
        }
    }
}
