using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task 
{

    [SerializeField]
    public int id;

    [SerializeField]
    public string title;

    [SerializeField]
    [Multiline]
    public string task;

    [Space(8)]
    public GameObject list;
    public GameObject taskItem;

    [SerializeField]
    public int importance;

    void Start()
    {
        if (title != "") SetData();
    }

    void SetData ()
    {

    }

}
