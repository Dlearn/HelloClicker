using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    // Other GameObjects
    public Slider playerHealth;
    public Text PlayerHealthText;
    public Image damageImage;
    public Image shield;

    // Player Stats
    public int _maxHealth = 100;
    public int currentHealth = 100;
    public int attackPerClick;
    public int currentGold = 0;

    // Other Scripts
    public Enemy enemy;

    // Sound
    private AudioSource _audioSouce;
    public AudioClip hitSound0;
    public AudioClip hitSound1;
    public AudioClip blockSound;
    private float volLowRange = 0.5f;
    private float volHighRange = 1.0f;

    // Health
    private bool damaged = false;
    public float flashSpeed = 1f;
    public Color flashColor = new Color(1f, 0f, 0f, .1f);

    // Announcement UI
    private GameObject announcementPanel;

    void Start () {
        // Initialize from player stats
        attackPerClick = 1;
        currentGold = 0;
        damaged = false;

        announcementPanel = GameObject.Find("AnnouncementPanel");
    }

    private void Awake()
    {
        _audioSouce = GetComponent<AudioSource>();
    }

    public void GetHitXDamage(int damage)
    {
        if (Input.touchCount == 2)
        {
            _audioSouce.PlayOneShot(blockSound, 1f);
        } else
        {
            damaged = true;
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
            playerHealth.value = currentHealth;
            PlayerHealthText.text = currentHealth + " / " + _maxHealth;

            int randSoundClip = Random.Range(0, 2);
            float volScale = Random.Range(volLowRange, volHighRange);
            switch (randSoundClip)
            {
                case 0:
                    _audioSouce.PlayOneShot(hitSound0, volScale);
                    break;
                case 1:
                    _audioSouce.PlayOneShot(hitSound1, volScale);
                    break;
            }

            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    public void Victory()
    {
        // Display Victory
        announcementPanel.GetComponent<Image>().color = new Color(1, 1, 1, 210.0f / 225f);
        announcementPanel.GetComponentInChildren<Text>().text = "VICTORY";
    }

    void Death()
    {
        // TODO: Decide what happens when player dies
        CancelInvoke();
        announcementPanel.GetComponent<Image>().color = new Color(1, 1, 1, 210.0f / 225f);
        announcementPanel.GetComponentInChildren<Text>().text = "Game Over";

        // Change scene in 3 seconds
        Invoke("LoadSoloScene", 3);
    }

    void LoadSoloScene()
    {
        SceneManager.LoadScene("Solo");
    }

    void Update ()
    {
        // Flash Red
        if (damaged)
        {
            damageImage.color = flashColor;
        } else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

        // Transparent shield
        Touch[] myTouches = Input.touches;
        if (Input.touchCount == 2)
        {
            //Input.GetTouch(0);
            float x = (myTouches[0].position.x + myTouches[1].position.x) / 2;
            float y = (myTouches[0].position.y + myTouches[1].position.y) / 2;
            shield.gameObject.SetActive(true);
            shield.transform.position = new Vector2(x,y);
        } else
        {
            shield.gameObject.SetActive(false);
        }
    }
}
