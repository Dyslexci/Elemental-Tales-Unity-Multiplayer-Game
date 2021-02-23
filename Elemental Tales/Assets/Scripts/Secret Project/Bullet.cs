using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    Rigidbody2D bulletBody;

    [SerializeField] Transform pos;
    [SerializeField] float radius = 1.5f;
    [SerializeField] private LayerMask layer;

    public ParticleSystem hit;
    public GameObject forwardFace;

    void Awake()
    {
        bulletBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bulletBody.velocity = transform.up * speed;
    }

    private void FixedUpdate()
    {
        checkPresent();

        if(transform.position.x > 120 || transform.position.x < -120 || transform.position.y > 80 || transform.position.y < -80)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Destroyable"))
        {
            collision.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
            Debug.Log("Bullet hit");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Checks for the players presence based off Physics2D collider circles and displays the hint if the player has entered the switch collider.
    /// </summary>
    private void checkPresent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.position, radius, layer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Destroyable")
            {
                Instantiate(hit, transform.position, forwardFace.transform.rotation);
                colliders[i].GetComponent<Asteroid>().TakeDamage(damage);
                Debug.Log("Bullet hit");
                Destroy(gameObject);
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(pos.position, radius);
    }
}
