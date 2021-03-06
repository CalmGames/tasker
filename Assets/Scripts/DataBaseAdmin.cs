﻿using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DataBaseAdmin : MonoBehaviour
{
    public static DataBaseAdmin instance;

    public PanelScaler[] menus;     //0: Login Menu, 1: Controls Menu, 2: Settings Menu;
    [Space(7)]
    public bool animating;

    [Header("Data")]
    public string server;
    public int port;
    public string dataBase;
    public string user;
    public string pass;

    [Header("Login Menu")]
    public InputField userField;
    public InputField passField;
    public Button submit;
    [Space(6)]
    public GameObject loadingIcon;
    public Text infoText;
    public Color[] colors;

    [Header("Controls Menu")]
    public Text userText;
    public Text permisionsText;
    public GameObject notification;
    public Button[] controlsButtons;      //0: Settings, 1: Task, 2: SendTask, 3:  Users, 4: Logout;
    public ControlMenuScaler[] controlsPanels; //0: Main, 1: Tasks, 2: SendTask, 3: Users;

    [Header("Tasks")]
    private Tasks tasks;

    //BUILT-IN FUNCTIONS

    private void Awake()
    {
        //REFERENCES
        tasks = GetComponent<Tasks>();
        //SINGLETON
        if (DataBaseAdmin.instance == null)
        {
            DataBaseAdmin.instance = this;
        }
        else if (DataBaseAdmin.instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("Data Base Admin it has been instantiated for the second time. This should not happen.");
        }
    }

    private void Start()
    {
        //Facilities for writing text faster.
        if (SystemInfo.deviceType != DeviceType.Handheld) userField.Select();
    }

    private void Update()
    {
        //LOGIN MENU
        
        //ShortCuts for writing text faster.
        if (userField.isFocused && Input.GetKeyDown(KeyCode.Tab)) passField.Select();
        if (Input.GetKeyDown(KeyCode.Return)) SubmitButton();
    }

    //LOGIN MENU FUNCTIONS

    public void SubmitButton ()
    {
        if(userField.text  != "")StartCoroutine(Submit());
    }

    public void LogoutButton()
    {
        StartCoroutine(Logout());
    }

    IEnumerator Submit ()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", userField.text);
        form.AddField("pass", passField.text);

        using(UnityWebRequest www = UnityWebRequest.Post("http://" + server + "/tasker/login.php", form))
        {
            yield return www.SendWebRequest();

            //Check if user is correct
            switch (www.downloadHandler.text)
            {
                case "0: admin":
                    GetNotifications(userField.text);
                    //Set info text to null
                    infoText.text = string.Empty;
                    //Set Controls Text
                    userText.text = userField.text;
                    permisionsText.text = "Permisos: Administrador";
                    controlsButtons[3].interactable = true;
                    //Set transition to control menu
                    StartCoroutine(LoginControlsMenu());
                    break;
                case "0: user":
                    GetNotifications(userField.text);
                    //Set info text to null
                    infoText.text = string.Empty;
                    //Set Controls Text
                    userText.text = userField.text;
                    permisionsText.text = "Permisos: Usuario";
                    controlsButtons[3].interactable = false;
                    //Set transition to control menu
                    StartCoroutine(LoginControlsMenu());
                    break;
                case "3: User does exist":
                    //Show info text to user
                    userField.Select();
                    userField.text = string.Empty;
                    passField.text = string.Empty;
                    infoText.color = colors[1];
                    infoText.text = "El usuario o contraseña es incorrecto, por favor intentelo de nuevo";
                    break;
                default:
                    print("error to login " + www.downloadHandler.text);
                    break;
            }
        }     
    }

    IEnumerator Logout()
    {
        if (!animating)
        {
            animating = true;

            //Start anim
            menus[0].menu = "LoginBack";
            menus[1].menu = "ControlsBack";

            menus[0].slide = true;
            menus[1].slide = true;

            yield return new WaitForSeconds(1.2f);
            //Empty Texts
            userField.Select();
            notification.SetActive(false);
            controlsButtons[3].interactable = false;
            userText.text = "Usuario";
            permisionsText.text = "Permisos: Usuario";
            //Close anim
            menus[0].slide = false;
            menus[1].slide = false;

            menus[0].menu = "login";
            menus[1].menu = "clontrols";

            animating = false;
        }
    }

    //CONTROLS MENU FUNCTIONS

    /*Use To Send task
    public void ValidateTaskCharacters(InputField text)
    {
        if (text.text.Contains(":"))
        {
            print("este caracter no es valido en su lugar usa<b> ; </b>");
            string i = text.text.Remove(text.text.Length - 1);
            text.text = i;
        }
        if (text.text.Contains("%"))
        {
            print("este caracter no es valido en su lugar usa<b> / </b>");
            string i = text.text.Remove(text.text.Length - 1);
            text.text = i;
        }
    }
    */

    public void SettingButton(bool open)
    {
        if (open) StartCoroutine(OpenSettingsMenu());
        else StartCoroutine(CloseSettingsMenu());
    }

    public void TasksButton (bool open)
    {
        if (open) StartCoroutine(GetTasks(false, userText.text));
        else StartCoroutine(CloseTasksMenu());
    }

    public void GetNotifications(string user)
    {
        StartCoroutine(GetTasks(true, user));
    }

    IEnumerator GetTasks(bool notifications, string user)
    {
        if (notifications)
        {
            WWWForm form = new WWWForm();
            form.AddField("user", user);

            using (UnityWebRequest www = UnityWebRequest.Post("http://" + server + "/tasker/getTaskNotifications.php", form))
            {
                yield return www.SendWebRequest();

                if (www.downloadHandler.text != "0" && !www.downloadHandler.text.Contains(":"))
                {
                    controlsButtons[1].GetComponent<RectTransform>().sizeDelta = new Vector2(430, 240);
                    controlsButtons[1].GetComponentInChildren<Text>().text = "Tareas";
                    controlsButtons[1].interactable = true;
                    notification.SetActive(true);
                    notification.GetComponentInChildren<Text>().text = www.downloadHandler.text;
                }
                else
                {
                    controlsButtons[1].GetComponent<RectTransform>().sizeDelta = new Vector2(830, 240);
                    controlsButtons[1].GetComponentInChildren<Text>().text = "No Hay Tareas";
                    controlsButtons[1].interactable = false;
                }
            }
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("user", user);

            using (UnityWebRequest www = UnityWebRequest.Post("http://" + server + "/tasker/getTask.php", form))
            {
                yield return www.SendWebRequest();
                print("ms: " + www.timeout);
                tasks.ImportData(www.downloadHandler.text);
                StartCoroutine(OpenTasksMenu(www));
            }
        }
    }

    //ANIM COROUTINES

    IEnumerator LoginControlsMenu()
    {
        if (!animating)
        {
            animating = true;

            //Enable loading icon
            loadingIcon.SetActive(true);

            yield return new WaitForSeconds(2.5f);
            //Start anim
            menus[0].menu = "login";
            menus[0].slide = true;

            menus[1].menu = "controls";
            menus[1].slide = true;

            yield return new WaitForSeconds(1.2f);
            //Empty Login Fields
            userField.text = string.Empty;
            passField.text = string.Empty;

            //Close anim
            menus[0].menu = "Login";
            menus[0].slide = false;

            menus[1].menu = "Controls";
            menus[1].slide = false;

            //Disable loading icon
            loadingIcon.SetActive(false);

            animating = false;
        }
    }

    IEnumerator OpenSettingsMenu()
    {
        if (!animating)
        {
            animating = true;

            //Start anim
            menus[1].slide = true;
            menus[2].slide = true;

            menus[1].menu = "controlsToSettings";
            menus[2].menu = "settings";

            yield return new WaitForSeconds(1.2f);
            //Close anim
            menus[1].menu = "ControlsToMenu";
            menus[1].slide = false;

            menus[2].menu = "Settings";
            menus[2].slide = false;

            animating = false;
        }
    }

    IEnumerator CloseSettingsMenu()
    {
        if (!animating)
        {
            animating = true;
            
            //Start anim
            menus[1].slide = true;
            menus[2].slide = true;

            menus[1].menu = "settingsToControls";
            menus[2].menu = "SettingsBack";

            yield return new WaitForSeconds(1.2f);
            //Close anim
            menus[1].menu = "Controls";
            menus[1].slide = false;

            menus[2].menu = "settings";
            menus[2].slide = false;

            animating = false;
        }
    }

    IEnumerator OpenTasksMenu(UnityWebRequest www)
    {
        if (!animating)
        {
            animating = true;

            controlsPanels[0].menu = "mainToTasks";
            controlsPanels[0].slide = true;
            controlsPanels[0].speed = 0.13f;

            controlsPanels[1].menu = "tasksToMain";
            controlsPanels[1].slide = true;
            controlsPanels[1].speed = 0.07f;

            yield return new WaitForSeconds(1.4f);

            controlsPanels[0].menu = "MainToTasks";
            controlsPanels[0].slide = false;

            controlsPanels[1].menu = "TasksToMain";
            controlsPanels[1].slide = false;

            animating = false;
        }
    }

    IEnumerator CloseTasksMenu()
    {
        if (!animating)
        {
            animating = true;

            controlsPanels[0].menu = "mainToTasksBack";
            controlsPanels[0].slide = true;
            controlsPanels[0].speed = 0.07f;

            controlsPanels[1].menu = "tasksToMainBack";
            controlsPanels[1].slide = true;
            controlsPanels[1].speed = 0.13f;

            yield return new WaitForSeconds(1.4f);

            //Delete old tasks
            GetComponent<Tasks>().DeleteTasks();

            controlsPanels[0].menu = "Main";
            controlsPanels[0].slide = false;

            controlsPanels[1].menu = "Tasks";
            controlsPanels[1].slide = false;

            animating = false;
        }
    }

}