using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject controlsMenu;

    public void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }

    public void ActivateControlsMenu()
    {
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }
}
