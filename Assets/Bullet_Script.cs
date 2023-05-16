using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject attackPoint;
    public float attackRadius;
    public LayerMask enemies;
    [SerializeField] private float damage;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            // Retrieve the PlayerHealth component from the collided player object
            PlayerMovement playerHealth = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerHealth != null)
            {
                // Inflict damage on the player
                playerHealth.currentPlayerHealth -= damage;
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() //For viewing the gizmo attack circle
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }
}
