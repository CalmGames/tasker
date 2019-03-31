using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScaler : MonoBehaviour
{
    private RectTransform rect;

    public string menu;

    public bool slide;
    [HideInInspector]
    public float speed = 0.1f;

    //BUILT-IN FUNCTIONS

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        ScreenScaler();
    }

    void ScreenScaler()
    {
        Vector2 res = new Vector2(Screen.width, -Screen.width);

        switch (menu)
        {
            //LOGIN
            case "login" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(res.y, rect.offsetMin.y), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(-res.x, rect.offsetMax.y), speed);
                break;
            case "Login" when !slide:
                rect.offsetMin = new Vector2(res.y, rect.offsetMin.y);
                rect.offsetMax = new Vector2(-res.x, rect.offsetMax.y);
                break;
            case "LoginBack" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, rect.offsetMin = new Vector2(0, 0), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, rect.offsetMax = new Vector2(0, 0), speed);
                break;

            //CONTROLS
            case "controls" when !slide:
                rect.offsetMin = new Vector2(res.x, rect.offsetMin.y);
                rect.offsetMax = new Vector2(res.x, 0);
                break;
            case "controls" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(0, rect.offsetMin.y), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(0, rect.offsetMax.y), speed);
                break;
            case "Controls" when !slide:
                rect.offsetMin = new Vector2(0, 0);
                rect.offsetMax = new Vector2(0, 0);
                break;
            case "ControlsBack" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(res.x, 0), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(res.x, 0), speed);
                break;

            //CONTROLS AND SETTINGS
            case "controlsToSettings" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(0, -Screen.height), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(0, -Screen.height), speed);
                break;
            case "ControlsToSettings" when !slide:
                rect.offsetMin = new Vector2(0, -Screen.height);
                rect.offsetMax = new Vector2(0, -Screen.height);
                break;
            case "settingsToControls" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(0, 0), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(0, 0), speed);
                break;

            //SETTINGS
            case "settings" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(DataBaseAdmin.instance.menus[1].rect.offsetMin.x, 0), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(DataBaseAdmin.instance.menus[1].rect.offsetMax.x, Screen.height), speed);
                break;
            case "settings" when !slide:
                rect.offsetMin = new Vector2(DataBaseAdmin.instance.menus[1].rect.offsetMin.x, Screen.height);
                rect.offsetMax = new Vector2(DataBaseAdmin.instance.menus[1].rect.offsetMax.x, Screen.height * 2);
                break;
            case "Settings" when !slide:
                rect.offsetMin = new Vector2(DataBaseAdmin.instance.menus[1].rect.offsetMin.x, 0);
                rect.offsetMax = new Vector2(DataBaseAdmin.instance.menus[1].rect.offsetMax.x, Screen.height);
                break;
            case "SettingsBack" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(DataBaseAdmin.instance.menus[1].rect.offsetMin.x, Screen.height), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(DataBaseAdmin.instance.menus[1].rect.offsetMax.x, Screen.height * 2), speed);
                break;
        }
    }

}
