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
    public bool isAlive;
    public int _maxHealth = 100;
    public int currentHealth = 100;
    public int attackPerClick;

    // Health
    private bool damaged = false;
    public float flashSpeed = 1f;
    public Color flashColor = new Color(1f, 0f, 0f, .1f);

    // Announcement UI
    private GameObject announcementPanel;

    void Start () {
        isAlive = true;
        // Initialize from player stats
        attackPerClick = 1;
        damaged = false;

        announcementPanel = GameObject.Find("AnnouncementPanel");
    }

    public void GetHitXDamage(int damage)
    {
        if (Input.touchCount == 2)
        {
            SoundManager.instance.Block();
        } else
        {
            damaged = true;
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
            playerHealth.value = currentHealth;
            PlayerHealthText.text = currentHealth + " / " + _maxHealth;

            if (GameManager.instance.bossType == "RedKnight")
            {
                SoundManager.instance.RedKnightAttack();
            }
            else if (GameManager.instance.bossType == "Smail")
            {
                SoundManager.instance.SmailAttack();
            }
            else // "LianHwa"
            {
                SoundManager.instance.LianHwaAttack();
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

        SoundManager.instance.PlayWinMusic();
        // Change scene in 10 seconds
        Invoke("LoadSoloScene", 9.5f);
    }

    void Death()
    {
        isAlive = false;
        // TODO: Decide what happens when player dies
        CancelInvoke();
        announcementPanel.GetComponent<Image>().color = new Color(1, 1, 1, 210.0f / 225f);
        announcementPanel.GetComponentInChildren<Text>().text = "Game Over";

        SoundManager.instance.PlayLoseMusic();

        // Change scene in 6 seconds
        Invoke("LoadSoloScene", 6);
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
