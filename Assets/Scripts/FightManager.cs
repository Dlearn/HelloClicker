using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour {

    [SerializeField]
    private GameObject RedKnight, Smail, LianHwa;
    [SerializeField]
    private UnityEngine.UI.Text goldDisplay, EnemyHealthText;

    private Player player;
    
    public Slider enemyHealth;

    void Start () {
        player = GetComponent<Player>();
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
        int r = Random.Range(0, 3); // 0,1,2
        if (r==0)
            enemy = (GameObject) Instantiate(RedKnight, transform);
        else if (r==1)
            enemy = (GameObject) Instantiate(Smail, transform);
        else
            enemy = (GameObject) Instantiate(LianHwa, transform);

        enemy.GetComponent<Enemy>().FightManager = this;
        enemy.GetComponent<Enemy>().player = player;
        enemy.GetComponent<Enemy>().enemyHealth = enemyHealth;
        enemy.GetComponent<Enemy>().EnemyHealthText = EnemyHealthText;
    }
}
