using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EnemySpawner : NetworkBehaviour
{

    // GameObjects
    public static GameObject myManager;
    private Player player;
    public GameObject RedKnight;
    public GameObject Smail;
    public UnityEngine.UI.Text goldDisplay;
    public Slider enemyHealth;
    public UnityEngine.UI.Text EnemyHealthText;

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

    public void SpawnEnemyInXSeconds(float x)
    {
        Invoke("SpawnEnemy", x);
    }

    private void SpawnEnemy()
    {
        GameObject newRedKnight = (GameObject) Instantiate(RedKnight, transform);
        //GameObject newRedKnight = (GameObject)Instantiate(Smail, transform);
        newRedKnight.GetComponent<Enemy>().EnemySpawner = this;
        newRedKnight.GetComponent<Enemy>().player = player;
        newRedKnight.GetComponent<Enemy>().enemyHealth = enemyHealth;
        newRedKnight.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
    }
}
