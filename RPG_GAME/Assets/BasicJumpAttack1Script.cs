using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicJumpAttack1Script : MonoBehaviour
{
    [SerializeField]
    int damage;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int doDamage()
    {
        return damage;
    }
}
