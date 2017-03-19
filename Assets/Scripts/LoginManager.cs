#pragma warning disable 0649
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    [SerializeField]
    private GameObject UsernameInput, SubmitUsername, Content, Logo;
    [SerializeField]
    private AudioClip introMusic;

    void Start()
    {
        SoundManager.instance.PlayMusic(introMusic);
    }

    public void MakeLoginVisible()
    {
        UsernameInput.SetActive(true);
        SubmitUsername.SetActive(true);
        StartCoroutine(FadeToWithChild(UsernameInput, 1, 1));
        StartCoroutine(FadeToWithChild(SubmitUsername, 1, 1));
        StartCoroutine(FadeTo(Content, 0.5f, 1));
        StartCoroutine(FadeTo(Logo, 1, 1));
    }

    IEnumerator FadeTo(GameObject go, float aValue, float aTime)
    {
        yield return new WaitForSeconds(1);
        float initialAlpha = go.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            go.GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Lerp(initialAlpha, aValue, t));
            yield return null;
        }
    }

    IEnumerator FadeToWithChild(GameObject go, float aFinal, float aTime)
    {
        yield return new WaitForSeconds(1);
        Text childText = go.GetComponentInChildren<Text>();
        float r = go.GetComponent<Image>().color.r;
        float g = go.GetComponent<Image>().color.g;
        float b = go.GetComponent<Image>().color.b;
        float a = go.GetComponent<Image>().color.a;

        float rC = childText.color.r;
        float gC = childText.color.g;
        float bC = childText.color.b;
        float aC = childText.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            go.GetComponent<Image>().color = new Color(r, g, b, Mathf.Lerp(a, aFinal, t));
            childText.color = new Color(rC, gC, bC, Mathf.Lerp(aC, aFinal, t));
            yield return null;
        }
    }

    public void addUser()
    {
        string username = UsernameInput.GetComponent<InputField>().text;
        if (username != "")
        {
            GameManager.instance.myUsername = username;
            JSONObject data = new JSONObject(JSONObject.Type.STRING);
            data.str = username;
            GameManager.socket.Emit("add user", data);
            GameManager.instance.usernameInput = true;
        }
    }
}
