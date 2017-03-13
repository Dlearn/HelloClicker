using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    // Other GameObject References
    public FightManager FightManager;
    public Player player;
    
    public Slider enemyHealth;
    public UnityEngine.UI.Text EnemyHealthText;

    private Animator anim;

    // Enemy Sounds
    private AudioSource _audioSouce;

    public AudioClip goldSound0;
    public AudioClip goldSound1;
    public AudioClip goldSound2;
    public AudioClip goldSound3;
    public AudioClip goldSound4;
    private float volLowRange = 0.5f;
    private float volHighRange = 1.0f;

    // Enemy Constants
    private float enemyCurrentHealth;
    private int _maxHealth = 20;
    private int _goldBounty = 10;
    private float _attackPeriod = 5.0f;
    

    // Use this for initialization
    void Awake()
    {
        _audioSouce = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        enemyHealth.gameObject.SetActive(true);
        enemyCurrentHealth = _maxHealth;
        EnemyHealthText.text = enemyCurrentHealth + " / " + _maxHealth;
        InvokeRepeating("AttackTrigger", 1.0f, _attackPeriod);
        
        //enemyHealth.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        enemyHealth.value = enemyCurrentHealth;
    }

    private void OnMouseDown() // Player attacks enemy
    {
        enemyCurrentHealth -= player.attackPerClick;
        enemyCurrentHealth = Mathf.Max(0.0f, enemyCurrentHealth);
        enemyHealth.value = enemyCurrentHealth;
        EnemyHealthText.text = enemyCurrentHealth + " / " + _maxHealth;

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
        data.AddField("damage", Random.Range(10,30));
        GameManager.socket.Emit("attack", data);

        if (enemyCurrentHealth <= 0)
        {
            Death();
        }
    }

    public void UpdateHealth(JSONObject obj)
    {
        // TODO: New health
        print("Update Health.");
        return;
    }

    private void AttackTrigger()
    {
        anim.SetTrigger("Attack");
        //print("Start Attack");
        Invoke("DamagePlayer", 1.25f); 
    }

    private void DamagePlayer()
    {
        player.GetHitXDamage(15);
    }

    public void Death()
    {
        // Stop attack trigger
        CancelInvoke();

        // Destroy the GameObject
        enemyHealth.gameObject.SetActive(false);
        Destroy(gameObject);

        // Display Victory
        GameObject.Find("VICTORY").GetComponent<Text>().text = "VICTORY";

        // Spawn new enemy
        //FightManager.SpawnEnemyInXSeconds(2f);   

        // TODO: Get gold! GetGold()
        player.currentGold += _goldBounty;
        FightManager.UpdateGold();
    }
}
