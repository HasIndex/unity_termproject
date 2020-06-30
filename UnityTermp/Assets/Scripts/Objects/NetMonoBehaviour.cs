using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetMonoBehaviour : MonoBehaviour
{
    protected Rigidbody2D   myRigidbody;
    public long          ServerID { get; set; } = default;
    public string        Nickname { get; set; } = String.Empty;


    public void MoveToPositionUsingServerPostion(int y, int x)
    {
        Vector2 vector = new Vector2();
        vector.x = x;
        vector.y = y;

        try 
        { 
            myRigidbody.MovePosition(vector);
        }
        catch(MissingReferenceException)
        {
            Debug.Log($"MissingReferenceException tag: {tag}");
        }
    }
}