using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float rechts;
    public float links;
    public int health = 50;

    private Animator anim;
    private Vector3 rotation;

    public GameObject deathEffect;
    // Start is called before the first frame update
    void Start()
    {
        rechts = transform.position.x + rechts;
        links = transform.position.x - links;
        rotation = transform.eulerAngles;
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if(transform.position.x < links)
        {
            transform.eulerAngles = rotation - new Vector3(0, 180, 0);
        } else if(transform.position.x > rechts)
        {
            transform.eulerAngles = rotation;
        }

        if(health <= 0)
        {
            FindObjectOfType<AudioManager>().Play("enemydeath");
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
