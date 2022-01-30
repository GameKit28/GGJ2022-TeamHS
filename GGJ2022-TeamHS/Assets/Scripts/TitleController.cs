using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleController : MonoBehaviour
{
    [Header("Button Config")]
    public ParticleButtons playButton;
    public ParticleButtons quitButton;
    public ParticleButtons optionsButton;
    [Space]
    [Header("Panel Config")]
    public GameObject mainButtonsPanel;
    public GameObject optionsPanel;
    public GameObject levelButtonsPanel;
    public Image fade;
    [Space]
    [Header("Popup Config")]
    public GameObject popup;
    public TMPro.TMP_Text popupText;
    public LayoutGroup popupButtonsPanel;
    public Button popupButtonPrefab;

    private struct PopupButton
    {
        public string text;
        public UnityAction onClickAction;
    }

    //TODO One assumption I'm going to make is that hitting B or ESC on the main menu will bring you back to the main buttons panel -cb
    void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        optionsButton.onClick.AddListener(OnOptionsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void OnPlayClicked()
    {
        fade.DOColor(Color.black, 2.5f).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        });
        //mainButtonsPanel.SetActive(false);
        //levelButtonsPanel.SetActive(true);
    }

    void OnOptionsClicked()
    {

        mainButtonsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    void OnQuitClicked()
    {
        //TODO Save progress here or we should prob just save as we go? -cb
        ShowPopup("Are you sure you want to quit?", new PopupButton[]
        {
            new PopupButton{text = "Cancel"},
            new PopupButton{text = "Quit", onClickAction = Application.Quit}
        }); ;
    }

    void ShowPopup(string bodyText, params PopupButton[] buttons)
    {

        popupText.text = bodyText;
        popupButtonsPanel.enabled = false;

        for (int i = 0; i < buttons.Length; i++)
        {
            PopupButton button = buttons[i];
            var buttonInstance = Instantiate(popupButtonPrefab, popupButtonsPanel.transform);
            buttonInstance.GetComponentInChildren<TMPro.TMP_Text>().text = button.text;
            buttonInstance.onClick.AddListener(() =>
            {
                button.onClickAction?.Invoke();
                HidePopup();
            });
            if(i == 0)
            {
                EventSystem.current.SetSelectedGameObject(buttonInstance.gameObject);
            }
        }

        popupButtonsPanel.enabled = true;
        popup.SetActive(true);
    }

    void HidePopup()
    {
        popup.SetActive(false);
        foreach(Transform childButtons in popupButtonsPanel.transform)
        {
            Destroy(childButtons.gameObject);
        }
        EventSystem.current.SetSelectedGameObject(quitButton.gameObject);
    }
}
