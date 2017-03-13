using UnityEngine;
using UnityEngine.UI;

public class SoloManager : MonoBehaviour {

    [SerializeField]
    private GameObject InviteListPanel, InviteList, InviteButton;

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
            newInviteButton.GetComponentInChildren<Text>().text = "Invite " + playerName;
            newInviteButton.transform.SetParent(InviteList.transform, false);
            Button btn = newInviteButton.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                JSONObject invitee = new JSONObject(JSONObject.Type.OBJECT);
                invitee.AddField("username", playerName);
                GameManager.socket.Emit("invite", invitee);
                InviteListPanel.SetActive(false);
            });
        }
    }
}
