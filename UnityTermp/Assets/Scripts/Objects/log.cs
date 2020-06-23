using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class log : Enemy
{
    public Transform    target;
    public float        chaseRadius;
    public float        attackRadius;
    public Transform    homePosition;


    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if ( distance <= chaseRadius && distance > attackRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

}
