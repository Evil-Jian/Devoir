using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject attackPoint;
    public float attackRadius;
    public LayerMask enemies;
    [SerializeField] private float damage;

    public void Attack()    //Triggers during the skeleton attack animation
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemies);

        foreach(Collider2D enemyGameObject in enemy)
        {
            Debug.Log("Hit player");
            
            PlayerMovement playerHealth = enemyGameObject.GetComponent<PlayerMovement>();
            if(playerHealth != null) // Add null check to prevent NullReferenceException
                playerHealth.currentPlayerHealth -= damage;
        }
    }

    private void OnDrawGizmos() //For viewing the gizmo attack circle
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }
}
