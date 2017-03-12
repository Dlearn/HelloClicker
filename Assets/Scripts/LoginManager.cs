using System;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    [SerializeField]
    private GameObject UsernameInput, SubmitUsername;

    void Awake () {
        GameManager GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void addUser()
    {
        String username = UsernameInput.GetComponent<InputField>().text;
        if (username != "")
        {
            GameManager.instance.myUsername = username;
            JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
            data.AddField("username", username);
            GameManager.socket.Emit("add user", data);
        }
    }
}
