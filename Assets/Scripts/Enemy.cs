using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    // Other GameObjects
    public UIManager UiManager;
    public UnityEngine.UI.Text EnemyHealthText;
    private Animator anim;

    // Sound
    public AudioClip goldSound0;
    public AudioClip goldSound1;
    public AudioClip goldSound2;
    public AudioClip goldSound3;
    public AudioClip goldSound4;
    private AudioSource _audioSouce;
    private float volLowRange = 0.5f;
    private float volHighRange = 1.0f;

    // Enemy Constants
    private float _maxHealth = 20.0f;
    private float _goldBounty = 10.0f;
    
    // Enemy Vars
    private float enemyCurrentHealth;

    // Attack Vars
    private float _attackPeriod = 5.0f;
    
    // Use this for initialization
    void Awake()
    {
        _audioSouce = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        enemyCurrentHealth = _maxHealth;
        EnemyHealthText.text = "Enemy Health:\n" + enemyCurrentHealth + " / " + _maxHealth;
        anim = GetComponent<Animator>();
        InvokeRepeating("AttackTrigger", 0.0f, _attackPeriod);
    }

    void Update()
    {
    }

    private void OnMouseDown()
    {
        enemyCurrentHealth -= UiManager.attackPerClick;
        enemyCurrentHealth = Mathf.Max(0.0f, enemyCurrentHealth);
        EnemyHealthText.text = "Enemy Health:\n" + enemyCurrentHealth + " / " + _maxHealth;

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
        UiManager.currentGold += _goldBounty;
        UiManager.UpdateGold();
    }
}
