#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    [SerializeField]
    private GameObject UsernameInput, SubmitUsername;

    public void MakeLoginVisible()
    {
        UsernameInput.SetActive(true);
        SubmitUsername.SetActive(true);
    }

    public void addUser()
    {
        string username = UsernameInput.GetComponent<InputField>().text;
        if (username != "")
        {
            GameManager.instance.myUsername = username;
            JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
            data.AddField("username", username);
            GameManager.socket.Emit("add user", data);
        }
    }
}
