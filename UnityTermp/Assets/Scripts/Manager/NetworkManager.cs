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

    private static Dictionary<Int64, NetMonoBehaviour> otherMap = new Dictionary<long, NetMonoBehaviour>();

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

    public void Add(long id, int objectType, string nickname, int y, int x)
    {
        NetMonoBehaviour netMonoBehaviour = null;
        //GameObject gobj = null;

        switch (objectType)
        {
            case 0:
                netMonoBehaviour = ObjectPooler.Instance.SpawnUsingTag("OtherPlayer", new Vector3(x, -y));
                break;
            case 1:
                netMonoBehaviour = ObjectPooler.Instance.SpawnUsingTag("FixedLog", new Vector3(x, -y));
                break;

            case 2:
                netMonoBehaviour = ObjectPooler.Instance.SpawnUsingTag("Log", new Vector3(x, -y));
                break;

            case 3:
                netMonoBehaviour = ObjectPooler.Instance.SpawnUsingTag("FixedOgre", new Vector3(x, -y));
                break;

            case 4:
                netMonoBehaviour = ObjectPooler.Instance.SpawnUsingTag("Ogre", new Vector3(x, -y));
                break;

            case 5:
                netMonoBehaviour = ObjectPooler.Instance.SpawnUsingTag("NPC2", new Vector3(x, -y));
                break;
            default:
                Debug.Log($"obj type : {objectType}  x : {x} y {y}");
                new NotImplementedException();
                break;
        }

        
        if(null != netMonoBehaviour)
        {
            netMonoBehaviour.ServerID = id;
            netMonoBehaviour.Nickname = nickname;
            try
            {
                otherMap.Add(id, netMonoBehaviour);
            }
            catch(ArgumentException)
            {
                //GameObject obj;
                //otherMap.TryGetValue(id, out obj);
                //ObjectPooler.Instance.Recycle(netMonoBehaviour.gameObject.tag, netMonoBehaviour);

                ObjectPooler.Instance.Recycle(netMonoBehaviour.gameObject.tag, otherMap[id]);
                otherMap[id] = netMonoBehaviour;
            }
        }
        else
        {
            Debug.Log($"null Reference Exception id : {id} obj type : {objectType}  x : {x} y {y}");
        }
    }

    public void Remove(long id)
    {
        NetMonoBehaviour go;
        
        otherMap.TryGetValue(id, out go);
        if (go == null)
        {
            Debug.Log($"없는 id : {id}");
            return;
        }

        otherMap.Remove(id);
        ObjectPooler.Instance.Recycle(go.tag, go);
    }

    public NetMonoBehaviour TryGet(long id)
    {
        NetMonoBehaviour go;

        if(otherMap.TryGetValue(id, out go))
        {
            return go;
        }

        return null;
    }
}
