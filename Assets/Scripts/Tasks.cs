using System;
using UnityEngine;

public class Tasks : MonoBehaviour
{

    public Color[] importanceColors;

    [Header("Objects")]
    public GameObject list;
    public GameObject taskItem;

    public void ImportData(string data)
    {
        string[] nTasksData = data.Split('/');

        string[] lines = nTasksData[1].Split('%');

        //Sort tasks
        string[] sorted = new string[5];
        string[] sort = new string[int.Parse(nTasksData[0])];
        /*
        for (int i = 0; i < lines.Length - 1; i++)
        {
            sorted = lines[i].Split(':');
            sort[i] = sorted[1];
        }
        Array.Sort(sort);
        for (int i = 0; i < sort.Length; i++)
        {
            lines[i] = 
        }
        */
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

            objData.id = int.Parse(data[0]);
            objData.title = data[2];
            objData.importance = int.Parse(data[1]);
            objData.task = data[3].ToString();
            objData.SetData(importanceColors);
    }

    public static Array SortString (string[] array)
    {
        if(array == null || array.Length == 0)
        {
            Debug.LogError("Sort String: Array passed i null");
            return null;
        }
        else
        {
            Array.Sort(array);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = string.Join("", array);
            }
            return array;
        }
    }

    public static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
    }

}
