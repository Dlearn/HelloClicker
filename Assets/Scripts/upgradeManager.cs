using UnityEngine;
using System.Collections;

public class upgradeManager : MonoBehaviour {

    // Other GameObjects
    public EnemySpawner EnemySpawner;
    public Player player;
    public UnityEngine.UI.Text itemInfo;

    public int cost;
    public int clickPower;
    public string itemName;
    //private float _newCost;

    void Start()
    {
        //player = EnemySpawner.player;
        itemInfo.text = itemName + "\n Cost: " + cost + "\nPower: "+ clickPower;
    }

    public void PurchasedUpgrade()
    {
        if (player.currentGold >= cost)
        {
            player.currentGold -= cost;
            EnemySpawner.UpdateGold();
            player.attackPerClick += clickPower;
        }
    }
}
