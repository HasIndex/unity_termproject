using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCharacterUsingServerPostion(int y, int x)
    {
        Vector3 vector = new Vector3();
        vector.x = x;
        vector.y = y;

        Debug.Log($"move server postion x {x}, y {y}");

        //myRigidbody.MovePosition(vector);
    }

}
