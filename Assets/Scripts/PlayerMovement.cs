using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private Transform playerObject;

    //attacking
    public GameObject attackPoint;
    public float radius;
    public LayerMask enemies;
    public float damage;
    //end of attacking variables

    //health
    public float playerHealth;
    public float currentPlayerHealth;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 5f;          //Movement speed and jump height
    [SerializeField] private float jumpForce = 10f;

    [SerializeField] private AudioSource attackSoundEffect;
    [SerializeField] private AudioSource moveSoundEffect;

    private enum MovementState { idle, running, jumping, falling, slicing, hurting, dead} //Player states

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        playerObject = GetComponent<Transform>();
        currentPlayerHealth = playerHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        if(currentPlayerHealth < playerHealth)      //When the player is hurt
        {
            playerHealth = currentPlayerHealth;
            anim.SetInteger("playerState", (int)MovementState.hurting);
        }
        else if(currentPlayerHealth > playerHealth)
        {
            playerHealth = currentPlayerHealth;
        }
        else if(playerHealth <= 0)          //When the player dies
        {
            anim.SetInteger("playerState", (int)MovementState.dead);
            Destroy(rb);
            Destroy(coll);
            Destroy(gameObject, 5);
        }

        else{        
            dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);    //Running

            if (Input.GetButtonDown("Jump") && isGrounded()) //Key for keyboard, button for input manager
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);      //Jump
            }

            if(Input.GetButtonDown("Fire2"))        //When the user attacks
            {
                anim.SetInteger("playerState", (int)MovementState.slicing);
                attackSoundEffect.Play();
            }

            else
            {
                UpdateAnimationState();
            } 
        }       
    }

    public void Attack()       //Triggers during the player attack animation
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemies);

        foreach(Collider2D enemyGameObject in enemy)
        {
            Debug.Log("Hit enemy");
            EnemyHealth enemyHealth = enemyGameObject.GetComponent<EnemyHealth>();
            if(enemyHealth != null) // Add null check to prevent NullReferenceException
                enemyHealth.health -= damage;
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if(!isGrounded()){
            moveSoundEffect.Stop();
        }

        if(dirX > 0f) //running right
        {
            if(!moveSoundEffect.isPlaying){
                moveSoundEffect.Play();
            }
            state = MovementState.running;
            playerObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if(dirX < 0f) //running left
        {
            if(!moveSoundEffect.isPlaying){
                moveSoundEffect.Play();
            }
            state = MovementState.running;
            playerObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {           //Player is idle
            state = MovementState.idle;
            moveSoundEffect.Stop();
        }

        if(rb.velocity.y > .1f) //Player jumping
        {
            state = MovementState.jumping;
        }
        else if(rb.velocity.y < -.1f){  //Player falling
            state = MovementState.falling;
        }

        anim.SetInteger("playerState", (int)state);
    }

    private bool isGrounded()   //Player to only jump on the ground
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }


    private void OnDrawGizmos() //For viewing the gizmo attack circle
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }
}
