using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private Transform attackPoint;
    private float attackRange = 0.5f;
    private LayerMask enemyLaters;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 5f;          //Movement speed and jump height
    [SerializeField] private float jumpForce = 10f;

    private enum MovementState { idle, running, jumping, falling, slicing }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Game Starts");
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);    //Running

        if (Input.GetButtonDown("Jump") && isGrounded()) //Key for keyboard, button for input manager
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);      //Jump
        }

        if(Input.GetButtonDown("Fire2"))
        {
            Attack();
        }

        else
        {
            UpdateAnimationState();
        }        
    }

    private void Attack()
    {
        anim.SetInteger("playerState", (int)MovementState.slicing);

        //Detect enemies in range of attack
        //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage enemies
        //foreach(Collider2D enemy in hitEnemies)
        //{
            //Debug.Log("We hit " + enemy.name);
        //}
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if(dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if(dirX < 0f) //left
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if(rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if(rb.velocity.y < -.1f){
            state = MovementState.falling;
        }

        anim.SetInteger("playerState", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
