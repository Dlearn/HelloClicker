using UnityEngine;
using System.Collections;

public class upgradeManager : MonoBehaviour {

    // Other GameObjects
    public UIManager UiManager;
    public UnityEngine.UI.Text itemInfo;


    public float cost;
    public int count = 0;
    public int clickPower;
    public string itemName;
    //private float _newCost;

    void Start()
    {
        itemInfo.text = itemName + "\n Cost: " + cost + "\nPower: "+ clickPower;
    }

    public void PurchasedUpgrade()
    {
        if (UiManager.currentGold >= cost)
        {
            UiManager.currentGold -= cost;
            count++;
            UiManager.attackPerClick += clickPower;
            cost = Mathf.Round(cost * 1.15f);
            //_newCost = Mathf.Pow(cost, _newCost = cost);
        }
    }

}
