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
    public static Int32                 serverPort { get; } = 21302;
    internal C2Client                   client;
    public static Int64                 uniqueSessionID = -1;

    private static Dictionary<Int64, GameObject> otherMap = new Dictionary<long, GameObject>();

    void Awake()
    {
        DontDestroyOnLoad(this);

        LoadConfigUsingJson();
        
        client = C2Client.Instance; //client = new C2Client(player);
        //client.Player = C2Client.; //(player);
    }

    private void Update()
    {
    }

    private void LoadConfigUsingJson()
    {
        
    }

    public MainPlayer Player
    {
        get
        {
            return client.Player;
        }
    }

    public void Add(long id, int objectType, int y, int x)
    {
        GameObject gobj = null;

        switch (objectType)
        {
            case 0:
                gobj = ObjectPooler.Instance.Spawn("NPC2", new Vector3(x, y));
                break;

            case 1:
                gobj = ObjectPooler.Instance.Spawn("OtherPlayer", new Vector3(x, y));
                break;

            case 2:
                gobj = ObjectPooler.Instance.Spawn("Orge", new Vector3(x, y));
                break;

            case 3:
                gobj = ObjectPooler.Instance.Spawn("log", new Vector3(x, y));
                break;

            default:
                Debug.Log($"obj type : {objectType}  x : {x} y {y}");
                new NotImplementedException();
                break;
        }


        if(null != gobj)
        {
            try
            {
                otherMap.Add(id, gobj);
            }
            catch(ArgumentException)
            {
                //GameObject obj;
                //otherMap.TryGetValue(id, out obj);
                ObjectPooler.Instance.Recycle(gobj.tag, gobj);
            }
        }
    }

    public void Remove(long id)
    {
        GameObject go;

        
        otherMap.TryGetValue(id, out go);
        if (go == null)
        {
            Debug.Log($"없는 id : {id}");
            return;
        }

        ObjectPooler.Instance.Recycle(go.tag, go);
    }

}
