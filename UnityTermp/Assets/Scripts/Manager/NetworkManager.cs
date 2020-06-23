using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.IO;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    private static NetworkManager       instance; // = new C2Network();
    private static string               gameVersion = "for testing";
    private static string               appVersion = "alpha";
    public static string                serverIP { get; } = "127.0.0.1";
    public static Int32                 serverPort { get; } = 9000;
    private static Dictionary<Int64, object> otherMap = new Dictionary<long, object>();
    internal C2Client                   client;
    public static Int64                 uniqueSessionID = -1;

    [SerializeField] PlayerMovement player;
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadConfigUsingJson();
        //client = new C2Client(player);
        client = C2Client.Instance;
        //client.Player = C2Client.;
        //(player);
    }

    private void Update()
    {
    }

    private void LoadConfigUsingJson()
    {
        
    }


}
