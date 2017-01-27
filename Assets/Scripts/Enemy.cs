using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    // Other GameObject References
    public UIManager UiManager;
    public Player player;
    public UnityEngine.UI.Text EnemyHealthText;
    public Image damageImage;
    public Slider enemyHealth;
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
    private int _maxHealth = 20;
    private int _goldBounty = 10;
    private float _attackPeriod = 5.0f;

    // Health and Healthbar
    private bool damaged;
    private float enemyCurrentHealth;
    public float flashSpeed = 1f;
    public Color flashColor = new Color(1f, 0f, 0f, .1f);

    // Enemy UI
    public Slider healthSlider;
    
    
    // Use this for initialization
    void Awake()
    {
        _audioSouce = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        enemyCurrentHealth = _maxHealth;
        EnemyHealthText.text = enemyCurrentHealth + " / " + _maxHealth;
        InvokeRepeating("AttackTrigger", 0.0f, _attackPeriod);
        
        //enemyHealth.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        enemyHealth.value = enemyCurrentHealth;
        damaged = false;
    }

    void Update()
    {
        // TODO: Move to player logic
        if (damaged)
        {
            damageImage.color = flashColor;
        } else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    private void OnMouseDown() // Player attacks enemy
    {
        enemyCurrentHealth -= player.attackPerClick;
        enemyCurrentHealth = Mathf.Max(0.0f, enemyCurrentHealth);
        EnemyHealthText.text = enemyCurrentHealth + " / " + _maxHealth;

        damaged = true;
        enemyHealth.value = enemyCurrentHealth;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("RedKnightIdle"))
            anim.SetTrigger("Hurt");
        
        int randSoundClip = Random.Range(0, 5);
        //float volScale = 1;
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

        if (enemyCurrentHealth <= 0)
        {
            Death();
        }
    }

    private void AttackTrigger()
    {
        anim.SetTrigger("Attack");
    }

    public void Death()
    {
        UiManager.SpawnEnemyIn1Second();
        Destroy(gameObject);
        player.currentGold += _goldBounty;
        UiManager.UpdateGold();
    }
}
