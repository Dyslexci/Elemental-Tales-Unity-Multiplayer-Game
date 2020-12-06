using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Controls melee attack for player 1.
 */

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private float attackRate = 2f;
    private float nextAttackTime = 0f;

    private void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack") && gameObject.GetComponent<CharacterControllerPlayer1>().getElement().Equals("Fire"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<DestroyableDoor>().damageDoor(40);
        }
    }

     private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
