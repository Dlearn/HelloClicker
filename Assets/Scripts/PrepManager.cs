using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrepManager : MonoBehaviour {
    // Button Sprite:
    // https://dabuttonfactory.com/#t=Ready&f=Open+Sans&ts=50&tc=000&w=295&h=145&c=5&bgt=gradient&bgc=ea9999&ebgc=ea9999&bs=1&bc=000&shs=3&shc=444&sho=se

    public Button maceButton;
    public Button swordButton;
    public Sprite unselectedButton;
    public Sprite selectedButton;

    public Button readyButton;
    public Sprite readyButtonSprite;
    public Sprite unreadyButtonSprite;

    private bool isReady;

	void Start () {
        isReady = false;
        GameManager.instance.selectedMaceNotSword = true;
    }
	
    public void Ready()
    {
        isReady = !isReady;
        if (isReady)
        {
            readyButton.image.overrideSprite = unreadyButtonSprite;
        } else
        {
            readyButton.image.overrideSprite = readyButtonSprite;
        }

        JSONObject data = new JSONObject(JSONObject.Type.BOOL);
        data.b = isReady;
        GameManager.socket.Emit("is ready", data);
    }

    public void SelectMace()
    {
        if (!isReady && !GameManager.instance.selectedMaceNotSword)
        {
            GameManager.instance.selectedMaceNotSword = true;
            maceButton.image.overrideSprite = selectedButton;
            swordButton.image.overrideSprite = unselectedButton;
        }
    }

    public void SelectSword()
    {
        if (!isReady && GameManager.instance.selectedMaceNotSword)
        {
            GameManager.instance.selectedMaceNotSword = false;
            maceButton.image.overrideSprite = unselectedButton;
            swordButton.image.overrideSprite = selectedButton;
        }
    }
}
