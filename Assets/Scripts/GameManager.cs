using System;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class GameManager : MonoBehaviour {

    // Constants
    const int PING_FREQUENCY = 5;

    // Public GameObjects
    public GameObject InviteButton;

    // Private GameObjects found at runtime
    private GameObject UsernameInput, SubmitUsername, InviteList;

    // Static singletons
    public static SocketIOComponent socket;
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    // Variables
    private String myUsername;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        UsernameInput = GameObject.Find("UsernameInput");
        SubmitUsername = GameObject.Find("SubmitUsername");
        InviteList = GameObject.Find("InviteList");

        socket = GetComponent<SocketIOComponent>();

        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);

        socket.On("login", (SocketIOEvent e) => {
            UsernameInput.SetActive(false);
            SubmitUsername.SetActive(false);
            // Query for who is online
            InvokeRepeating("LookingForParty", 0, PING_FREQUENCY);
        });
        socket.On("solo players", (SocketIOEvent e) => {
            var obj = e.data;
            //print(obj.keys[i]); print(obj.list[i].str);

            // KILL ALL THE CHILDREN
            foreach (Transform child in InviteList.transform)
            {
                Destroy(child.gameObject);
            }

            // Create new children
            for (int i = 0; i < obj.list.Count; i++)
            {
                String playerName = obj.list[i].str;
                if (playerName == myUsername) continue; // Can't invite yourself

                GameObject newInviteButton = Instantiate(InviteButton);
                newInviteButton.GetComponentInChildren<Text>().text = "Invite " + playerName;
                newInviteButton.transform.SetParent(InviteList.transform, false);
                Button btn = newInviteButton.GetComponent<Button>();
                btn.onClick.AddListener(() => {
                    JSONObject invitee = new JSONObject(JSONObject.Type.OBJECT);
                    invitee.AddField("username", playerName);
                    socket.Emit("invite", invitee);
                });
            }
        });
        socket.On("form party", (SocketIOEvent e) => {
            socket.Emit("formed party");
            CancelInvoke();

            // START WALKING
            InvokeRepeating("SendCoordinates", 0, PING_FREQUENCY);
        });
        socket.On("party on obj", (SocketIOEvent e) => {
            CancelInvoke();
            socket.Emit("fighting boss");
        });
        //socket.On("boss hit", BossHit);
    }

    public void addUser()
    {
        String username = UsernameInput.GetComponent<InputField>().text;
        if (username != "")
        {
            myUsername = username;
            JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
            data.AddField("username", username);
            socket.Emit("add user", data);
        }
    }

    void LookingForParty()
    {
        socket.Emit("get solos");
    }

    public void invitePlayer()
    {
        JSONObject invitee = new JSONObject(JSONObject.Type.OBJECT);
        invitee.AddField("username", "user0");
        socket.Emit("invite", invitee);
    }

    void SendCoordinates()
    {
        int jitter_x = UnityEngine.Random.Range(0, 3); // -1,0,1,2
        int jitter_y = UnityEngine.Random.Range(0, 3); // -1,0,1,2
        print(jitter_x + ", " + jitter_y);
        JSONObject coord = new JSONObject(JSONObject.Type.OBJECT);
        coord.AddField("x", 50 + jitter_x);
        coord.AddField("y", 50 + jitter_y);
        socket.Emit("cur coord", coord);
    }

    public void Attack()
    {
        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("damage", UnityEngine.Random.Range(10, 31));
        socket.Emit("attack", data);
    }

    void TestOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    void TestError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }
}
