using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject fireworks;
    public float fadeInDuration = 1.5f;

    private bool levelStarted = false;
    private bool endOfLevel = false;
    private InputAction toggleAction;
    public AudioSource startSound;
    public Image fade;

    private float levelCompletionTime;

    public void Start()
    {
        startText.text = $"Press Space or Y/Triangle to Start!";
        //#if !UNITY_EDITOR
        fade.color = Color.black;
        fade.gameObject.SetActive(true);
        fade.DOColor(Color.clear, fadeInDuration).OnComplete(() =>
        {
            var input = player.GetComponent<PlayerInput>();
            toggleAction = input.actions["ToggleCamera"];
            toggleAction.performed += ToggleCamera;
        //#endif
            fade.gameObject.SetActive(false);
        //#if !UNITY_EDITOR
        });
        //#endif
        levelCompletionTime = 0;
    }

    private void OnDestroy()
    {
        toggleAction.performed -= ToggleCamera;
    }

    private void ToggleCamera(InputAction.CallbackContext obj)
    {
        if (endOfLevel) 
        {
            fade.gameObject.SetActive(true);
            fade.DOColor(Color.black, 0.2f).OnComplete(() =>
            {
                SceneManager.LoadScene(nextLevelIndex);
            });
        };

        if (!levelStarted) 
        {
            startSound.Play();
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
            other.GetComponentInParent<PlayerHealthController>()?.SetImmortal(true);
            endLevelCam.enabled = true;
            followCam.enabled = false;
            mapCam.enabled = false;
            endOfLevel = true;
            mapCam.enabled = true;
            fireworks.SetActive(true);
            startText.text = $"Press Space or Y/Triangle to Continue!";
            bestTimeText.text = $"{Mathf.RoundToInt(levelCompletionTime)} Seconds";
            winStatePanel.SetActive(true);
            DOTween.Sequence()
                .Append(startText.DOColor(Color.white, 1f))
                .Append(startText.rectTransform.DOShakePosition(1.5f, randomness: 30, strength: 0.5f, vibrato: 5))
                //.Join(player.transform.DOSpiral(10, speed: 50));
                .Join(player.transform.DOMoveY(1000, 100));
        }
    }
}
