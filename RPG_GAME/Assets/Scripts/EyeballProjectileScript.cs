using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Eyeball projectile script.
//Projectile saved as a prefab and initialised by the eyeball.
//TODO: Explosion animations.
public class EyeballProjectileScript : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    int damage;

    Vector3 destination;

    //Gets an empty object positioned in the chest of the player. Used instead of the player's real position so that projectiles aren't fired at the player's feet.
    private void Start()
    {
        Transform player = GameObject.Find("PlayerAttackTarget").transform; 
        destination = player.position;
        
    }

    //If the projectile reaches the player's position at the moment the projectile was fired(ie. when destination was initialised) it is destroyed after a small delay.
    //Otherwise the projectile moves another step along the path towards the destination.
    void Update()
    {
        if(transform.position == destination)
        {
            StartCoroutine("DoEndOfLife");
            Destroy(gameObject);
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);
        
    }
    //TODO: Explosion animation? Perhaps different to when the player is hit. (asssuming this isn't triggered if the player doesn't move??)
    IEnumerable DoEndOfLife()
    {
        yield return new WaitForSeconds(0.01f);
    }

    //If it hits the player it is immediately destroyed.
    //The player controller handles taking damage from the projectile.
    //TODO: Some explosion animation.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }
    }

    public int doDamage()
    {
        return damage;
    }

}
