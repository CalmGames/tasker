using System.Collections.Generic;
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

    //BUILT-IN FUNCTIONS

    private void Awake()
    {
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
        //Facilities for writing text faster.
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

        #pragma warning disable CS0618
        WWW www = new WWW("http://"+ server +"/tasker/login.php", form);
        yield return www;

        //Check if user is correct
        switch (www.text)
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
                userField.text = string.Empty;
                passField.text = string.Empty;
                infoText.color = colors[1];
                infoText.text = "El usuario o contraseña es incorrecto, por favor intentelo de nuevo";
                break;
            default:
                print("<color=red>error to login <b>" + www.text);
                break;
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

    //Use To Send task
    public void ValidateTaskCharacters(InputField text)
    {
        if (text.text.Contains(":") || text.text.Contains("%"))
        {
            print("este caracter no es valido");

            string i = text.text.Remove(text.text.Length - 1);
            text.text = i;
        }
    }

    public void SettingButton(bool open)
    {
        if (open) StartCoroutine(OpenSettingsMenu());
        else StartCoroutine(CloseSettingsMenu());
    }

    public void TasksButton()
    {
        StartCoroutine(GetTasks(false, userText.text));
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

            #pragma warning disable CS0618
            WWW www = new WWW("http://" + server + "/tasker/getTaskNotifications.php", form);
            yield return www;

            if (www.text != "0" && !www.text.Contains(":"))
            {
                notification.SetActive(true);
                notification.GetComponentInChildren<Text>().text = www.text;
            }
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("user", user);

            #pragma warning disable CS0618
            WWW www = new WWW("http://" + server + "/tasker/getTask.php", form);
            yield return www;

            string[] nTasks = www.text.Split('/');

            string[] lines = nTasks[1].Split('%');

            //if (content[0] == "0") print("no hay tareas disponibles");
            //if (content[0] != "0") print("tareas disponibles: " + nTasks[0]);
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
}