﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy eyeball script.
//Enemy chases player if he's within a certain range.
//Rotates to fire at player.
//TODO: Death animation
//TODO: Take damage animation.
public class EyeballScript : MonoBehaviour
{
    Animator Animator;
    Rigidbody2D rb2d;
    
    bool isChasing;
    bool isAttacking;

    [SerializeField]
    Transform player;

    [SerializeField]
    float agroRange;

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float stopDistance;

    [SerializeField]
    float fireRate;

    [SerializeField]
    int health;

    float timeTillNextAttack;
    float idleHeight;

    //Basic initialisation.
    //idleHeight is used as a way to keep the eyeball at a fixed height above the ground.
    //Collisions with the player were changing its position.
    void Start()
    {
        Animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        idleHeight = transform.position.y;
        isChasing = false;
        isAttacking = false;
    }

    // Update is called once per frame
    //****NOTE: Not sure whether any of this should be in FixedUpdate(). Working fine for now****
    //Calculates distance from eyeball to player. 
    //If within agrorange but not within stopdistance and not currently attacking the player, chases the player.
    //else if out of agrorange or at stopping distance, stop chasing player.
    //if within agrorange and within stopping distance attack the player.(this can be triggered the same frame that either of the other two are.)
    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer < agroRange && Math.Abs((transform.position.x - player.position.x)) > stopDistance && !isAttacking)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); //resets rotation as the eyeball may have rotated when firing.
            isChasing = true;
            ChasePlayer();
        }
        else if(distToPlayer > agroRange || Math.Abs((transform.position.x - player.position.x)) <= stopDistance)
        {
            isChasing = false;
            StopChasingPlayer();
        }
        
        if(distToPlayer < agroRange && Math.Abs((transform.position.x - player.position.x)) <= stopDistance) //if player in agro & eyeball at stopping distance
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize(); //we only care about direction. Vector changed to magnitude 1 with same direction.
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Attack(rotation);

            
        }

    }


    private void FixedUpdate()
    {
        if (!isChasing)
        {
            Animator.Play("Eyeball_idle");

        }

        if(health <= 0)
        {
            killSelf();
        }

    }

    //If player hits eyeball take damage.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BasicAttack1HitBox")
        {
            health = health - collision.gameObject.GetComponent<BasicAttack1Script>().doDamage();
            
        }
        if(collision.gameObject.name == "BasicJumpAttack1HitBox")
        {
            health = health - collision.gameObject.GetComponent<BasicJumpAttack1Script>().doDamage();
        }
    }

    private void killSelf()
    {
        Destroy(gameObject);
    }

    //Moves towards player along the x-axis. Flips localscale if needbe.
    void ChasePlayer()
    {
        if(transform.position.x < player.position.x) // on left of player
        {
            rb2d.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(1, 1);
            Animator.Play("Eyeball_run");
        }
        else if(transform.position.x > player.position.x) //on right of player
        {
            rb2d.velocity = new Vector2(-moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);
            Animator.Play("Eyeball_run");
        }
    }

    //Stops moving. If not at idleheight moves back to this height.
    void StopChasingPlayer()
    {
        rb2d.velocity = new Vector2(0, 0);
        if (transform.position.y != idleHeight)
        {
            transform.position = new Vector3(transform.position.x, idleHeight, 0);

        }

    }

    //Rotates the eyeball so that it is pointer at the player when it fires.
    //Instantiates a projectile once it is pointing at the player.
    void Attack(float rotation)
    {
        isAttacking = true;
        timeTillNextAttack -= Time.deltaTime;         
        if (timeTillNextAttack <= 0)
        {
            timeTillNextAttack = 1 / fireRate;
           // Vector3 direction = player.transform.position - transform.position;
           // direction.Normalize();
           // float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (transform.localScale.x == -1)
            {
                //transform.localScale = new Vector2(-1, 1);
                transform.rotation = Quaternion.Euler(0f, 0f, rotation + 180);

            }
            else
            {

                transform.rotation = Quaternion.Euler(0f, 0f, rotation);
            }
           // transform.LookAt(Vector3.forward, player.transform.position);
            Instantiate(projectile, firePoint.position, Quaternion.Euler(0f, 0f, rotation - 90));
        }



        isAttacking = false;
        //Instantiate(projectile, transform.position, transform.rotation);
    }

  

}
