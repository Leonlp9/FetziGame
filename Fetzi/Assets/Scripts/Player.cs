using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //private float speed = 5f; // Geschwindigkeit des Spielers
    public float jumpForce = 7f; // Sprungkraft des Spielers
    public float doubleJumpForce = 6f; // Sprungkraft des Doppelsprungs
    public float wallJumpForce = 10f; // Sprungkraft des Wand-Sprungs
    public float wallSlideSpeed = 2f; // Geschwindigkeit des Wand-Rutschens
    public float wallJumpDuration = 0.2f; // Dauer des Wand-Sprungs

    private int Tode;
    private bool isDead = false;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    public float normalSpeed = 5f;
    public float shiftSpeed = 2f;
    public float controllSpeed = 8f;

    public GameObject panel;
    public GameObject deathEffect;
    public Text TodeGui;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        Tode = PlayerPrefs.GetInt("tode", 0);
        TodeGui.text = "Tode: " + Tode.ToString();
    }


    void Update()
    {

        if(rb.position.y < -10)
        {
             Death();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            CoinManager.AddMoney();
            FindObjectOfType<AudioManager>().Play("coin");
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Truhe" && !other.GetComponent<Animator>().GetBool("OpenChest"))
        {
            CoinManager.AddMoney();
            CoinManager.AddMoney();
            CoinManager.AddMoney();
            CoinManager.AddMoney();
            CoinManager.AddMoney();
            FindObjectOfType<AudioManager>().Play("coin");
            other.GetComponent<Animator>().SetBool("OpenChest", true);
        }

        if (!isDead)
        {
            if (other.gameObject.tag == "Spike")
            {
                Death();
            }
            if (other.gameObject.tag == "Enemy")
            {
                Death();
            }

        }

        if (other.gameObject.tag == "Ziel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    public void Death()
    {
        isDead = true;
        panel.SetActive(true);
        Tode++;
        PlayerPrefs.SetInt("tode", Tode);
        TodeGui.text = "Tode: " + Tode.ToString();
        FindObjectOfType<AudioManager>().Play("tod");
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}

