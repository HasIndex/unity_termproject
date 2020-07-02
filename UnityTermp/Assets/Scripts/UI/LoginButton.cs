using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] InputField inputField;

    private void Awake()
    {
        //inputField = GetComponent<InputField>();
    }

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            OnClick();
        }
    }
    
    public void OnClick()
    {
        C2Client.Instance.Nickname = inputField.text;

        Schema schema;
        if (LocalDBManager.Instance.TrySelectSchema(C2Client.Instance.Nickname,  out schema))//.SelectSchema() ;
        {
            C2Client.Instance.SendLoginPacket(schema.level, schema.exp, schema.hp, schema.x, schema.y);
        }
        else
        {
            LocalDBManager.Instance.InsertSchema(C2Client.Instance.Nickname, 1, 0, 200, -1, -1);
            C2Client.Instance.SendLoginPacket(1, 0, 200, -1, -1);
        }
    }

}
