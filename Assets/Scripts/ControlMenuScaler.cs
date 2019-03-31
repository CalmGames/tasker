using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenuScaler : MonoBehaviour
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
        PanelScaler();
    }

    void PanelScaler()
    {

        Vector2 res = new Vector2(Screen.width, -Screen.width);

        switch (menu)
        {
            //Main Panel
            case "Main" when !slide:
                rect.offsetMin = new Vector2(0, 0);
                rect.offsetMax = new Vector2(0, 0);
                break;
            case "mainToTasks" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(0, res.y), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(0, -res.x), speed);
                break;
            case "MainToTasks" when !slide:
                rect.offsetMin = new Vector2(0, res.y);
                rect.offsetMax = new Vector2(0, -res.x);
                break;
            case "mainToTasksBack" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(0, 0), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(0, 0), speed);
                break;

            //Tasks Panel
            case "Tasks" when !slide:
                rect.offsetMin = new Vector2(res.x, 0);
                rect.offsetMax = new Vector2(-res.y, 0);
                break;
            case "tasksToMain" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(0, 0), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(0, 0), speed);
                break;
            case "TasksToMain" when !slide:
                rect.offsetMin = new Vector2(0, 0);
                rect.offsetMax = new Vector2(0, 0);
                break;
            case "tasksToMainBack" when slide:
                rect.offsetMin = Vector2.Lerp(rect.offsetMin, new Vector2(res.x, 0), speed);
                rect.offsetMax = Vector2.Lerp(rect.offsetMax, new Vector2(-res.y, 0), speed);
                break;

        }
    }

}
