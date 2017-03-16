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
        Invoke("SpawnEnemy", 1.5f);

        // Uncomment to test animations
        //GameObject enemy = (GameObject)Instantiate(LianHwa, transform);
        //enemy.GetComponent<Enemy>().FightManager = this;
        //enemy.GetComponent<Enemy>().player = player;
        //enemy.GetComponent<Enemy>().enemyHealth = enemyHealth;
        //enemy.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
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
        if (GameManager.instance.bossType == "RedKnight")
            enemy = (GameObject) Instantiate(RedKnight, transform);
        else if (GameManager.instance.bossType == "Smail")
            enemy = (GameObject) Instantiate(Smail, transform);
        else // "LianHwa"
            enemy = (GameObject) Instantiate(LianHwa, transform);

        enemy.GetComponent<Enemy>().FightManager = this;
        enemy.GetComponent<Enemy>().player = player;
        enemy.GetComponent<Enemy>().enemyHealth = enemyHealth;
        enemy.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
    }
}
