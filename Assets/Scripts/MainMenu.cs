using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public UnityEvent onSettingsClickEvent;
    public int sceneIndexToLoad;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnStartButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
