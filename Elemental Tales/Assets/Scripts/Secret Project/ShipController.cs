using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    Rigidbody2D shipBody;
    ShipRotation shipRotation;
    public float shipSpeed;
    public float bulletSpeed;
    public float bulletDamage;
    public GameObject bulletPrefab;
    public GameObject gunSputter;
    public GameObject engineFire;
    public GameObject gun;
    public GameObject gunRotation;

    public CanvasGroup movedPanel;
    public CanvasGroup shotPanel;
    public CanvasGroup movedConfirmPanel;
    public CanvasGroup shotConfirmedPanel;

    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    bool hasShot;
    bool hasMoved;

    void Start()
    {
        shipBody = GetComponent<Rigidbody2D>();
        shipRotation = GetComponent<ShipRotation>();
        movedConfirmPanel.gameObject.SetActive(false);
        shotConfirmedPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Mouse0))
        {
            MoveShipForward();
            engineFire.SetActive(true);
            if (!hasMoved)
                StartCoroutine(TickMoved());
        } else if(!Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Mouse0))
        {
            engineFire.SetActive(false);
        }
        if(Input.GetKey(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            FireBullet();
            nextAttackTime = Time.time + 1f / attackRate;
            if (!hasShot)
                StartCoroutine(TickFired());
        }
    }

    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, new Vector2(gun.transform.position.x + (shipBody.velocity.x * Time.deltaTime), gun.transform.position.y + (shipBody.velocity.y * Time.deltaTime)), transform.rotation);
        GameObject sputter = Instantiate(gunSputter, new Vector2(gun.transform.position.x + (shipBody.velocity.x * Time.deltaTime), gun.transform.position.y + (shipBody.velocity.y * Time.deltaTime)), gun.transform.rotation);
        sputter.GetComponent<GunSputterCleanup>().shipSpeed = shipSpeed;
        bullet.GetComponent<Bullet>().speed = bulletSpeed;
        bullet.GetComponent<Bullet>().damage = bulletDamage;
    }

    void MoveShipForward()
    {
        shipBody.AddRelativeForce(Vector2.up * shipSpeed);
    }

    IEnumerator TickMoved()
    {
        hasMoved = true;
        movedConfirmPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        while(movedPanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            movedPanel.alpha -= .05f;
        }
        movedPanel.gameObject.SetActive(false);
    }

    IEnumerator TickFired()
    {
        hasMoved = true;
        shotConfirmedPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        while (shotPanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            shotPanel.alpha -= .05f;
        }
        shotPanel.gameObject.SetActive(false);
    }
}
