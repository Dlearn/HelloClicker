using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    // Other GameObject References
    public FightManager FightManager;
    public Player player;
    
    // Enemy UI
    public Slider enemyHealth;
    public Text EnemyHealthText;

    // Animator
    private Animator anim;

    // Enemy Variables
    private int enemyCurrentHealth;
    public int maxHealth;
    public float damageDelay;
    private int goldBounty = 10;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        enemyHealth.gameObject.SetActive(true);
        //maxHealth = 100;
        enemyHealth.maxValue = maxHealth;
        enemyCurrentHealth = maxHealth;
        enemyHealth.value = maxHealth;
        EnemyHealthText.text = maxHealth + " / " + maxHealth;

        int attackPeriod = Random.Range(4,6);
        InvokeRepeating("AttackTrigger", 1, attackPeriod);
    }

    private void OnMouseDown() // Player attacks enemy
    {
        if (!GameManager.instance.selectedMaceNotSword) return;
        
        // Can't attack if you're dead.
        if (!player.isAlive) return;

        SoundManager.instance.MaceAttack();
        DamageEnemy();
    }

    public void DamageEnemy()
    {
        // Play Animation
        if (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            anim.SetTrigger("Hurt");

        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("damage", Random.Range(4, 8));
        GameManager.socket.Emit("attack", data);
    }

    public void UpdateHealth(int remainingHealth)
    {
        enemyCurrentHealth = remainingHealth;
        enemyCurrentHealth = Mathf.Max(0, enemyCurrentHealth);
        enemyHealth.value = enemyCurrentHealth;
        EnemyHealthText.text = enemyCurrentHealth + " / " + maxHealth;

        if (enemyCurrentHealth <= 0)
        {
            Death();
        }
    }

    private void AttackTrigger()
    {
        anim.SetTrigger("Attack");
        Invoke("DamagePlayer", damageDelay); 
    }

    private void DamagePlayer()
    {
        int damage = Random.Range(28, 35);
        player.GetHitXDamage(damage);
    }

    public void Death()
    {
        // Stop attack trigger
        CancelInvoke();
        
        // Make GameObject Inactive
        enemyHealth.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Destroy(gameObject);

        // TODO: Get gold! GetGold()
        //player.currentGold += goldBounty;
        //FightManager.UpdateGold();
    }

    void LoadSoloScene()
    {
        SceneManager.LoadScene("Solo");
        Destroy(gameObject);
    }
}
