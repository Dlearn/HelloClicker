using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    // GameObjects
    public static GameObject myManager;
    public GameObject RedKnight;
    public UnityEngine.UI.Text goldDisplay;
    public UnityEngine.UI.Text EnemyHealthText;

    public float currentGold = 0f;
    public int attackPerClick = 1;

    // Enemy Spawning
    //public bool hasEnemy;
    //private Vector3 _spawnPoint = new Vector3(20f, 60f);

    // Use this for initialization
    void Start () {
        myManager = gameObject;
        Invoke("SpawnEnemy", 1.0f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateGold()
    {
        goldDisplay.text = "Gold: " + currentGold;
    }

    public void SpawnEnemyIn1Second()
    {
        Invoke("SpawnEnemy", 1.0f);
    }

    private void SpawnEnemy()
    {
        GameObject newRedKnight = (GameObject) Instantiate(RedKnight, transform);
        newRedKnight.GetComponent<Enemy>().UiManager = this;
        newRedKnight.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
    }
}
