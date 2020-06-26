using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyType
{
    Peace, Agro
}


public class Enemy : MonoBehaviour
{
    public int              health;
    public string           enemyName;
    public int              baseAttack;
    public float            moveSpeed;
    private Rigidbody2D     myRigidbody;
    private int             serverID;
    private EnemyType       enemyType;


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

        myRigidbody.MovePosition(vector);
    }



}
