#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour {

    // Serialized variables
    public GameObject RedKnight, Smail, LianHwa;
    public GameObject SwipeManager;
    public Text EnemyHealthText;
    public Slider enemyHealth;

    private Player player;
    
    void Start () {
        SoundManager.instance.PlayFightMusic();

        player = GetComponent<Player>();

        // Uncomment to test animations
        //GameObject enemy = Instantiate(RedKnight, new Vector3(.7f, .8f, 1), Quaternion.identity);
        //enemy.GetComponent<Enemy>().damageDelay = 1.05f;
        //enemy.GetComponent<Enemy>().FightManager = this;
        //enemy.GetComponent<Enemy>().player = player;
        //enemy.GetComponent<Enemy>().enemyHealth = enemyHealth;
        //enemy.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
        //enemy.GetComponent<Enemy>().maxHealth = 100;
        //SwipeTrail swipeTrail = Instantiate(SwipeManager).GetComponent<SwipeTrail>();
        //swipeTrail.enemy = enemy.GetComponent<Enemy>();
    }

    public void SpawnEnemyInXSeconds(float x)
    {
        Invoke("SpawnEnemy", x);
    }

    private void SpawnEnemy()
    {
        GameObject enemy;
        if (GameManager.instance.bossType == "RedKnight")
        {
            enemy = Instantiate(RedKnight, new Vector3(.7f, 1, 1), Quaternion.identity);
            enemy.GetComponent<Enemy>().damageDelay = 1.05f;
        }
        else if (GameManager.instance.bossType == "Smail")
        { 
            enemy = Instantiate(Smail, new Vector3(0, 0, 1), Quaternion.identity);
            enemy.GetComponent<Enemy>().damageDelay = 1.0f;
        }
        else // "LianHwa"
        { 
            enemy = Instantiate(LianHwa, new Vector3(0, 0, 1), Quaternion.identity);
            enemy.GetComponent<Enemy>().damageDelay = .85f;
        }

        enemy.GetComponent<Enemy>().FightManager = this;
        enemy.GetComponent<Enemy>().player = player;
        enemy.GetComponent<Enemy>().enemyHealth = enemyHealth;
        enemy.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
        enemy.GetComponent<Enemy>().maxHealth = GameManager.instance.bossHealth;

        // Create SwipeTrail
        if (!GameManager.instance.selectedMaceNotSword)
        { 
            SwipeTrail swipeTrail = Instantiate(SwipeManager).GetComponent<SwipeTrail>();
            swipeTrail.enemy = enemy.GetComponent<Enemy>();
        }
    }
}
