using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThorns : MonoBehaviour
{
    [SerializeField] private float thornDamage;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float attackCooldown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Retrieve the PlayerHealth component from the collided player object
            PlayerMovement playerHealth = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerHealth != null && cooldownTimer >= attackCooldown)
            {
                // Inflict damage on the player
                playerHealth.currentPlayerHealth -= thornDamage;
            }
            Debug.Log("Dead");
        }
    }
}
