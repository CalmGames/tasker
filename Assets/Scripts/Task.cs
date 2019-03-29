using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Task : MonoBehaviour
{

    [SerializeField]
    [HideInInspector]
    public int id;

    [SerializeField]
    [HideInInspector]
    public string title;

    [SerializeField]
    [Multiline]
    [HideInInspector]
    public string task;

    [SerializeField]
    [HideInInspector]
    public int importance;

    [Header("UI")]
    public Image importanceImg;
    public Text titleText;

    public void SetData (Color[] colors)
    {
        titleText.text = title;
        switch (importance)
        {
            case 0:
                importanceImg.color = colors[0];
                break;
            case 1:
                importanceImg.color = colors[1];
                break;
            case 2:
                importanceImg.color = colors[2];
                break;
        }
    }
}