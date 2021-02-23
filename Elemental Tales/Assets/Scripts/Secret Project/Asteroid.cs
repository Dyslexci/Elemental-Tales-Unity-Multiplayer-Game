using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Asteroid : MonoBehaviour
{
    float health;
    int score;

    void Start()
    {
        float size = Random.Range(.3f, .8f);
        score = Mathf.RoundToInt(1000 * size);
        health = 180f * size;
        transform.localScale = new Vector2(size, size);
        GetComponent<SpriteRenderer>().size = new Vector2(size, size);
        GetComponentInChildren<Light2D>().pointLightOuterRadius = GetComponentInChildren<Light2D>().pointLightOuterRadius * size;
    }

    void Update()
    {
        if(health <= 0)
        {
            
            Death();
        }
    }

    private void FixedUpdate()
    {
        //transform.Rotate(0, 0, Random.Range(10, 200) * Time.deltaTime); //rotates 50 degrees per second around z axis

        if (transform.position.x > 120 || transform.position.x < -120 || transform.position.y > 80 || transform.position.y < -80)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
            Debug.Log("Bullet hit");
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);
    }

    void Death()
    {
        GameObject.Find("GameMaster").GetComponent<AsteroidsGameMaster>().AddScore(score);
        GetComponent<Rigidbody2D>().simulated = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
