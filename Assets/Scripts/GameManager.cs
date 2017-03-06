using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameManager : MonoBehaviour {

    public static SocketIOComponent socket;
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private Client client;

    [Serializable]
    public class Name
    {
        public string username;
    }

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
        socket = GetComponent<SocketIOComponent>();

        socket.On("open", TestOpen);
        socket.On("boop", TestBoop);
        socket.On("error", TestError);
        socket.On("close", TestClose);

        StartCoroutine("Foo");
    }

    private IEnumerator Foo()
    {
        // wait 1 seconds and continue
        yield return new WaitForSeconds(1);

        socket.Emit("add user");
    }

    public void addUser()
    {
        Name input = new Name();
        input.username = "DYLAN HO";
        string json = JsonUtility.ToJson(input);
    }


    public void TestOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    public void TestBoop(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

        if (e.data == null) { return; }

        Debug.Log(
            "#####################################################" +
            "THIS: " + e.data.GetField("this").str +
            "#####################################################"
        );
    }

    public void TestError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }
}
