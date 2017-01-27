using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // GameObjects
    public static GameObject myManager;
    public Player player;
    public GameObject RedKnight;
    public UnityEngine.UI.Text goldDisplay;
    public UnityEngine.UI.Text EnemyHealthText;
    public Image damageImage;
    public Slider enemyHealth;

    // Enemy Spawning
    //private Vector3 _spawnPoint = new Vector3(20f, 60f);

    void Start () {
        player = GetComponent<Player>();
        myManager = gameObject;
        Invoke("SpawnEnemy", 1.0f);
    }
	
	void Update () {
	
	}

    public void UpdateGold()
    {
        goldDisplay.text = "Gold: " + player.currentGold;
    }

    public void SpawnEnemyIn1Second()
    {
        Invoke("SpawnEnemy", 1.0f);
    }

    private void SpawnEnemy()
    {
        GameObject newRedKnight = (GameObject) Instantiate(RedKnight, transform);
        newRedKnight.GetComponent<Enemy>().UiManager = this;
        newRedKnight.GetComponent<Enemy>().player = player;
        newRedKnight.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
        newRedKnight.GetComponent<Enemy>().damageImage = damageImage;
        newRedKnight.GetComponent<Enemy>().enemyHealth = enemyHealth;
    }
}
