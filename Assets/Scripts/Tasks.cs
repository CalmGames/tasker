using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tasks : MonoBehaviour
{

    [SerializeField]
    public int nTasks;

    [Header("Objects")]
    public GameObject list;
    public GameObject taskItem;

    public void ImportData(string data)
    {
        string[] nTasks = data.Split('/');

        string[] lines = nTasks[1].Split('%');

        for (int i = 0; i < lines.Length - 1; i++)
        {
            string[] content = lines[i].Split(':');

            switch (content[1])
            {
                case "0":
                    content[1] = " <b><color=green>0</color></b> ";
                    break;
                case "1":
                    content[1] = " <b><color=yellow>0</color></b> ";
                    break;
                case "2":
                    content[1] = " <b><color=red>0</color></b> ";
                    break;
            }

            id = int.Parse(content[0]);
            title = content[2];
            importance = int.Parse(content[1]);
            task = content[3];

        }
    }

    public void CreateTasks ()
    {
        for (int i = 0; i < nTasks; i++)
        {
            GameObject obj = Instantiate(taskItem, Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(list.transform);
        }
    }

}
