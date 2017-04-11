#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;

public class SoloManager : MonoBehaviour {

    [SerializeField]
    private GameObject InviteListPanel, InviteList, InviteButton;
    void Start()
    {
        SoundManager.instance.PlayAmbientMusic();

        // Query for who is online
        InvokeRepeating("LookingForParty", 0, GameManager.instance.PING_FREQUENCY);
    }

    public void UpdateList(JSONObject obj)
    {
        //print(obj.keys[i]); print(obj.list[i].str);

        // KILL ALL THE CHILDREN
        foreach (Transform child in InviteList.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new children
        for (int i = 0; i < obj.list.Count; i++)
        {
            string playerName = obj.list[i].str;
            if (playerName == GameManager.instance.myUsername) continue; // Can't invite yourself

            GameObject newInviteButton = Instantiate(InviteButton);
            newInviteButton.GetComponentInChildren<Text>().text = playerName;
            newInviteButton.transform.SetParent(InviteList.transform, false);
            Button btn = newInviteButton.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                JSONObject invitee = new JSONObject(JSONObject.Type.OBJECT);
                invitee.AddField("username", playerName);
                GameManager.socket.Emit("invite", invitee);
            });
        }
    }

    public void HideInviteListPanel()
    {
        InviteListPanel.SetActive(false);
    }

    void LookingForParty()
    {
        GameManager.socket.Emit("get solos");
    }
}