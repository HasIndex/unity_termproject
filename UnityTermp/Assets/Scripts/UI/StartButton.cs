﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
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
        
    }
    
    public void OnClick()
    {
        //SceneManager.LoadSceneAsync("1_Game_mmo", LoadSceneMode.Single);

        
        Debug.Log(inputField.text);

        cs_packet_login login_packet;
        login_packet.header.size = (sbyte)Marshal.SizeOf<cs_packet_login>();
        login_packet.header.type = PacketType.C2S_LOGIN;

        // login_packet.name; 

        // C2Session.Instance.SendPacket<cs_packet_enter>(enter_packet);
    }

}
