using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack1Script : MonoBehaviour
{
    // Start is called before the first frame update
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
