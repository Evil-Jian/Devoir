using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Script : MonoBehaviour

{
    private Animator anim;
    private Rigidbody2D body;
    private BoxCollider2D box;
    public GameObject Player;

    public GameObject FOV;
    public float fovRadius;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float attackCooldown;

    private enum MovementState {idle, moving, attacking, hurt, dead};

    [SerializeField] private float moveSpeed;
    private float currentSpeed;
    public float stopDistance;
    private Transform target;

    private float dirX;
    private int healthData;
    private EnemyHealth healthAction;

    [SerializeField] private AudioSource hurtSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;

    private bool deathSoundPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        healthAction = GetComponent<EnemyHealth>();
        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        healthData = healthAction.ComputeHealth();
        
        if(healthData == 0)      //When the skeleton is hurt
        {
            hurtSoundEffect.Play();
            anim.SetInteger("goblinState", (int)MovementState.hurt);
        }

        else if(healthData == 1)        //When the skeleton dies
        {
            if(!deathSoundPlayed){
                deathSoundEffect.Play();
                deathSoundPlayed = true;
            }
            anim.SetInteger("goblinState", (int)MovementState.dead);
            Destroy(body);
            Destroy(box);
            Destroy(gameObject, 2);
        }
        
        else
        {
            if (target != null && Player != null)
            {
                // Move the enemy towards the player
                Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);

                if (distanceToPlayer < stopDistance) {      // Stop moving towards the player    
                    currentSpeed = 0f;
                    dirX = 0f;
                    if(cooldownTimer >= attackCooldown){    //Attack when the enemy is stops moving && player is in range
                        anim.SetInteger("goblinState", (int)MovementState.attacking);
                        cooldownTimer = 0f;
                        return;
                    }
                }
                else {
                    currentSpeed = moveSpeed;       // Resume moving towards the player
                    if(targetPosition.x > transform.position.x){
                        dirX = 1f;      //Enemy is moving right
                    }
                    else{
                        dirX = -1f;     //Enemy is moving left
                    }
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
                }
            }
            UpdateAnimationState();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)     //When the player enters the FOV
    {
        // Check if the player has entered the trigger collider
        if (collision.CompareTag("Player"))
        {
            target = collision.transform; // Set the player as the target
        }
    }

     private void OnTriggerExit2D(Collider2D collision)     //When the player exits the FOV
    {
        // Check if the player has exited the trigger collider
        if (collision.CompareTag("Player"))
        {
            target = null; // Reset the target
            dirX = 0f;
        }
    }

    private void UpdateAnimationState()  
    {
        MovementState state;
        if(dirX > 0f) //running right
        {
            state = MovementState.moving;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if(dirX < 0f) //running left
        {
            state = MovementState.moving;
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {
            state = MovementState.idle;
        }

        anim.SetInteger("goblinState", (int)state);
    }
}
