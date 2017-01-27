using UnityEngine;
using System.Collections;

public class upgradeManager : MonoBehaviour {

    // Other GameObjects
    public UIManager UiManager;
    public Player player;
    public UnityEngine.UI.Text itemInfo;

    public int cost;
    public int clickPower;
    public string itemName;
    //private float _newCost;

    void Start()
    {
        player = UiManager.player;
        itemInfo.text = itemName + "\n Cost: " + cost + "\nPower: "+ clickPower;
    }

    public void PurchasedUpgrade()
    {
        if (player.currentGold >= cost)
        {
            player.currentGold -= cost;
            UiManager.UpdateGold();
            player.attackPerClick += clickPower;
        }
    }
}
