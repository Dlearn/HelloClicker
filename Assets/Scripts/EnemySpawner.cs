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
    public GameObject LianHwa;
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
        GameObject enemy;
        int r = Random.Range(0, 2);
        if (r==1)
            enemy = (GameObject) Instantiate(LianHwa, transform);
        else 
            enemy = (GameObject) Instantiate(Smail, transform);

        enemy.GetComponent<Enemy>().EnemySpawner = this;
        enemy.GetComponent<Enemy>().player = player;
        enemy.GetComponent<Enemy>().enemyHealth = enemyHealth;
        enemy.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
    }
}
