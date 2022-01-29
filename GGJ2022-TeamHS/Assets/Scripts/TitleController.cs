using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{

    public Button playButton;
    public Button quitButton;
    public Button optionsButton;

    public GameObject optionsPanel;
    public GameObject levelButtonsPanel;
    public GameObject mainButtonsPanel;

    public GameObject popup;
    public TMPro.TMP_Text popupText;
    public Button popupLeftButton;
    public TMPro.TMP_Text popupLeftButtonText;
    public Button popupRightButton;
    public TMPro.TMP_Text popupRightButtonText;

    //TODO One assumption I'm going to make is that hitting B or ESC on the main menu will bring you back to the main buttons panel -cb
    void Start()
    {
        try
        {
            playButton.onClick.AddListener(OnPlayClicked);
            optionsButton.onClick.AddListener(OnOptionsClicked);
            quitButton.onClick.AddListener(OnQuitClicked);
        }
        catch
        {
            Debug.LogWarning("Title Screen button refs are missing in the inspector so some of them prob wont work.");
        }

    }

    void OnPlayClicked()
    {
        try
        {
            mainButtonsPanel.SetActive(false);
            levelButtonsPanel.SetActive(true);
        }
        catch
        {
            Debug.LogWarning("Title Screen panel refs are missing in the inspector so some of them prob wont work.");

        }
    }

    void OnOptionsClicked()
    {
        try
        {
            mainButtonsPanel.SetActive(false);
            optionsPanel.SetActive(true);
        }
        catch
        {
            Debug.LogWarning("Title Screen panel refs are missing in the inspector so some of them prob wont work.");
        }
    }

    void OnQuitClicked()
    {
        //TODO Save progress here or we should prob just save as we go? -cb
        Application.Quit();
    }


}
