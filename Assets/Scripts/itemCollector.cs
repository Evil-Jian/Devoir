using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag("Heart"))
        {
            PlayerMovement health = GetComponent<PlayerMovement>();
            Destroy(collision.gameObject);
            health.currentPlayerHealth += 20;
        }
    }
}
