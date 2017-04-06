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

    private bool is_ready;
    private bool selectedMaceNotSword;

	void Start () {
        is_ready = false;
        selectedMaceNotSword = true;
    }
	
    public void Ready()
    {
        
        is_ready = !is_ready;
        if (is_ready)
        {
            readyButton.image.overrideSprite = unreadyButtonSprite;
        } else
        {
            readyButton.image.overrideSprite = readyButtonSprite;
        }

        JSONObject data = new JSONObject(JSONObject.Type.BOOL);
        data.b = is_ready;
        GameManager.socket.Emit("is ready", data);
    }

    public void SelectMace()
    {
        print("1");
        if (!selectedMaceNotSword)
        {
            print("2");
            selectedMaceNotSword = true;
            maceButton.image.overrideSprite = selectedButton;
            swordButton.image.overrideSprite = unselectedButton;
        }
    }

    public void SelectSword()
    {
        print("a");
        if (selectedMaceNotSword)
        {
            print("b");
            selectedMaceNotSword = false;
            maceButton.image.overrideSprite = unselectedButton;
            swordButton.image.overrideSprite = selectedButton;
        }
    }
}
