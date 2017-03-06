using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Client : MonoBehaviour {

    public SocketIOComponent socket;

    public void Start()
    {
        GameObject go = GameObject.Find("GameManager(Clone)");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("open", TestOpen);
        socket.On("boop", TestBoop);
        socket.On("error", TestError);
        socket.On("close", TestClose);

        StartCoroutine("Foo");
    }

    private IEnumerator BeepBoop()
    {
        // wait 1 seconds and continue
        yield return new WaitForSeconds(1);

        socket.Emit("add user");

        // wait 3 seconds and continue
        yield return new WaitForSeconds(3);

        socket.Emit("beep");

        // wait 2 seconds and continue
        yield return new WaitForSeconds(2);

        socket.Emit("beep");

        // wait ONE FRAME and continue
        yield return null;

        socket.Emit("beep");
        socket.Emit("beep");
    }

    private IEnumerator Foo()
    {
        // wait 1 seconds and continue
        yield return new WaitForSeconds(1);

        socket.Emit("add user");
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
