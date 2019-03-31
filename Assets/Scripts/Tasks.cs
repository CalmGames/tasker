using System;
using UnityEngine;

public class Tasks : MonoBehaviour
{

    public Color[] importanceColors;

    [HideInInspector]
    public int nTasks;

    [Header("Objects")]
    public GameObject[] spawnedList;
    public GameObject list;
    public GameObject taskItem;

    public void ImportData(string data)
    {
        string[] nTasksData = data.Split('/');

        nTasks = int.Parse(nTasksData[0]);

        string[] lines = nTasksData[1].Split('%');

        for (int i = 0; i < lines.Length - 1; i++)
        {
            string[] content = lines[i].Split(':');
            //Send Data
            CreateTasks(content);
        }
    }

    public void CreateTasks (string[] data)
    {
            GameObject obj = Instantiate(taskItem, Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(list.transform);
            Task objData = obj.GetComponent<Task>();

            Array.Resize(ref spawnedList, spawnedList.Length + 1);
            spawnedList[spawnedList.Length -1] = obj;

            objData.id = int.Parse(data[0]);
            objData.title = data[2];
            objData.importance = int.Parse(data[1]);
            objData.task = data[3].ToString();
            objData.SetData(importanceColors);
    }

    public void DeleteTasks()
    {
        for (int i = 0; i < spawnedList.Length; i++)
        {
            Destroy(spawnedList[i]);
        }
        Array.Resize(ref spawnedList, 0);
    }
}
