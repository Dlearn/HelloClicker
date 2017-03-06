using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class GameManager : MonoBehaviour {

    const int PING_FREQUENCY = 5;

    public static SocketIOComponent socket;
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    private InputField username_input;

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
        username_input = GameObject.Find("UsernameInput").GetComponent<InputField>();

        socket = GetComponent<SocketIOComponent>();

        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);

        socket.On("form party", FormParty);
        socket.On("party on obj", PartyOnObj);
        //socket.On("boss hit", BossHit);
    }

    private IEnumerator Foo()
    {
        // wait 1 seconds and continue
        yield return new WaitForSeconds(1);

        socket.Emit("add user");
    }

    public void addUser()
    {
        print(username_input.text);
        JSONObject username = new JSONObject(JSONObject.Type.OBJECT);
        username.AddField("username", username_input.text);
        socket.Emit("add user", username);
        // TODO: START SEARCHING
    }

    public void invitePlayer()
    {
        print("invite!");
        JSONObject invitee = new JSONObject(JSONObject.Type.OBJECT);
        invitee.AddField("username", "user0");
        socket.Emit("invite", invitee);
    }

    void FormParty(SocketIOEvent e)
    {
        socket.Emit("formed party");
        CancelInvoke();

        // START SEARCHING
        InvokeRepeating("SendCoordinates", 0, PING_FREQUENCY);
    }

    void PartyOnObj(SocketIOEvent e)
    {
        CancelInvoke();
        socket.Emit("fighting boss");
    }

    void SendCoordinates()
    {
        int jitter_x = UnityEngine.Random.Range(0,3); // -1,0,1,2
        int jitter_y = UnityEngine.Random.Range(0,3); // -1,0,1,2
        print(jitter_x + ", " + jitter_y);
        JSONObject coord = new JSONObject(JSONObject.Type.OBJECT);
        coord.AddField("x", 50+jitter_x);
        coord.AddField("y", 50+jitter_y);
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
