#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour {

    // Serialized variables
    [SerializeField]
    private GameObject RedKnight, Smail, LianHwa;
    [SerializeField]
    private UnityEngine.UI.Text goldDisplay, EnemyHealthText;
    [SerializeField]
    private Slider enemyHealth;

    private Player player;
    
    void Start () {
        player = GetComponent<Player>();
        //Invoke("SpawnEnemy", 1.0f);

        // Uncomment to test animations
        //GameObject enemy = Instantiate(Smail, transform);
        //enemy.GetComponent<Enemy>().damageDelay = 1.05f;
        //enemy.GetComponent<Enemy>().FightManager = this;
        //enemy.GetComponent<Enemy>().player = player;
        //enemy.GetComponent<Enemy>().enemyHealth = enemyHealth;
        //enemy.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
        //enemy.GetComponent<Enemy>().maxHealth = 100;
    }

    //public void UpdateGold()
    //{
    //    goldDisplay.text = "Gold: " + player.currentGold;
    //}

    public void SpawnEnemyInXSeconds(float x)
    {
        Invoke("SpawnEnemy", x);
    }

    private void SpawnEnemy()
    {
        GameObject enemy;
        if (GameManager.instance.bossType == "RedKnight")
        { 
            enemy = Instantiate(RedKnight, transform);
            enemy.GetComponent<Enemy>().damageDelay = 1.05f;
        }
        else if (GameManager.instance.bossType == "Smail")
        { 
            enemy = Instantiate(Smail, transform);
            enemy.GetComponent<Enemy>().damageDelay = 1.0f;
        }
        else // "LianHwa"
        { 
            enemy = Instantiate(LianHwa, transform);
            enemy.GetComponent<Enemy>().damageDelay = .85f;
        }

        enemy.GetComponent<Enemy>().FightManager = this;
        enemy.GetComponent<Enemy>().player = player;
        enemy.GetComponent<Enemy>().enemyHealth = enemyHealth;
        enemy.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
        enemy.GetComponent<Enemy>().maxHealth = GameManager.instance.bossHealth;
    }
}
