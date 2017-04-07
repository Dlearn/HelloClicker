using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    // Other GameObject References
    public FightManager FightManager;
    public Player player;
    
    // Enemy UI
    public Slider enemyHealth;
    public UnityEngine.UI.Text EnemyHealthText;

    // Animator
    private Animator anim;

    // Audio
    private AudioSource _audioSouce;
    public AudioClip goldSound0;
    public AudioClip goldSound1;
    public AudioClip goldSound2;
    public AudioClip goldSound3;
    public AudioClip goldSound4;
    private float volLowRange = 0.5f;
    private float volHighRange = 1.0f;

    // Enemy Variables
    private int enemyCurrentHealth;
    public int maxHealth;
    public float damageDelay;
    private int goldBounty = 10;
    
    void Awake()
    {
        _audioSouce = GetComponent<AudioSource>();
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
        // Can't attack if you're dead.
        if (!player.isAlive) return;

        // Play Animation
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            anim.SetTrigger("Hurt");

        // Play Sound
        int randSoundClip = Random.Range(0, 5);
        float volScale = Random.Range(volLowRange, volHighRange);
        switch (randSoundClip)
        {
            case 0:
                _audioSouce.PlayOneShot(goldSound0, volScale);
                break;
            case 1:
                _audioSouce.PlayOneShot(goldSound1, volScale);
                break;
            case 2:
                _audioSouce.PlayOneShot(goldSound2, volScale);
                break;
            case 3:
                _audioSouce.PlayOneShot(goldSound3, volScale);
                break;
            default:
                _audioSouce.PlayOneShot(goldSound4, volScale);
                break;
        }

        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("damage", Random.Range(4,8));
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
        player.enemy = this;
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
