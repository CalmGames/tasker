using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenuScaler : MonoBehaviour
{
    private RectTransform rect;

    public string menu;

    public bool slide;
    float speed = 0.1f;

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

            //Tasks Panel
            case "Tasks" when !slide:
                rect.offsetMin = new Vector2(res.x, 0);
                rect.offsetMax = new Vector2(-res.y, 0);
                break;
        }
    }

}
