using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballProjectileScript : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    int damage;

    Vector3 destination;
   // float startTime;
    //float lifetTime = 2;


    private void Start()
    {
        //startTime = Time.time;
        Transform player = GameObject.Find("PlayerAttackTarget").transform;
        //Debug.Log(player.name);
        destination = player.position;
        
    }


   /* public void StartShoot()
    {

    } */
    void Update()
    {
       /* if(Time.time - startTime >= lifetTime)
        {
            Destroy(gameObject);
        } */

        if(transform.position == destination)
        {
            StartCoroutine("DoEndOfLife");
            Destroy(gameObject);
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);
        
    }

    IEnumerable DoEndOfLife()
    {
        yield return new WaitForSeconds(0.01f);
    }

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

   /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject);
        }

        if(collision.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }
    } */
}
