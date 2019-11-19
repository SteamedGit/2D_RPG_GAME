using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour  //Author: Timothy Hitge (with help from John Stejskal and Alex G) Year: 2019 
{
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    bool isGrounded;
    bool isFacingLeft;
    bool isFacingRight;

    [SerializeField]
    Transform groundCheck;
    public Transform GroundCheck { get => groundCheck; set => groundCheck = value; } //these get and set methods fix some errors thrown up when making these variables a serialized field. 
   

    [SerializeField]
    Transform groundCheckL;
    public Transform GroundCheckL { get => groundCheckL; set => groundCheckL = value; }
    

    [SerializeField]
    Transform groundCheckR;
    public Transform GroundCheckR { get => groundCheckR; set => groundCheckR = value; }


    [SerializeField]
    private float runSpeed = 1.5f;

    [SerializeField]
    private float jumpSpeed = 5f;

   

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isFacingRight = true; //apparently its useful to keep track of what direction the player is facing 
        isFacingLeft = false;
       


    }

    private void FixedUpdate()
    {
        
        //Player Movement & Corresponding Animation
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")) ||                     //Check if the player is grounded
            Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;            
            animator.Play("Player_jump");      //If the player is not grounded play the jump animation
           
         
        }

        if (Input.GetKey("d") || Input.GetKey("right"))  //If the user inputs d or the standard right key for input, apply a velocity runSpeed to the right 
        {
            rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y); 
            if (isGrounded)                                               //If the player is grounded play the running to the right animation
            {
                animator.Play("Player_runRight");
                transform.localScale = new Vector3(1,1,1); //Local Scale oriented to the right 
                isFacingRight = true;
                isFacingLeft = false;
                Debug.Log(isFacingRight);
                Debug.Log(isFacingLeft);
            }
           
            
        }
        else if (Input.GetKey("a") || Input.GetKey("left")) //If the user inputs a or the standard left key for input, apply a velocity runSpeed to the left
        {
            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);
            if (isGrounded)                                           //If the player is grounded play the running to the left animation 
            {
                animator.Play("Player_runLeft");    //flip the object(includes colliders) in the X direction so that the player faces the left 
                transform.localScale = new Vector3(-1,1,1); //Local Scale oriented to the left 
                isFacingRight = false;
                isFacingLeft = true;
                Debug.Log(isFacingRight);
                Debug.Log(isFacingLeft);
            }

            
        }
        else                                                //if the player is moving neither right nor left & is on a ground layer, play the idle animation
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            if (isGrounded)
            {
                animator.Play("Player_idle");
               
            }
           
        }

       if ((Input.GetKey("space")||Input.GetKey("w")||Input.GetKey("up")) && isGrounded)    //if the player is on the ground and the user has pressed the space key, apply a velocity jumpspeed upwards 
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            animator.Play("Player_jump");             //play the jump animation
        
            
        } 
    }
}
