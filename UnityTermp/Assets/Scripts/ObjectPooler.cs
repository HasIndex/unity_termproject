using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.VFX;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [SerializeField] GameObject[] prefabs;
    private Dictionary<string, GameObject> prefabIndexDict;
    private Dictionary<string, List<GameObject>> poolDict;

    public void Awake()
    {
        prefabIndexDict = new Dictionary<string, GameObject>();
        poolDict = new Dictionary<string, List<GameObject>>();
        for (int i = 0; i < prefabs.Length; i++)
        {
            var gameObjectList = new List<GameObject>();
            var prefab = prefabs[i];
            var go = Instantiate(prefab);

            prefabIndexDict.Add(prefab.name, prefabs[i]);
            go.SetActive(false);
            go.transform.position = Vector3.zero;

            gameObjectList.Add(go);
            poolDict.Add(prefab.name, gameObjectList);
        }
    }

    public GameObject Spawn(string gameObjectName, Vector3 position)
    {
        List<GameObject> goList;
        if ( false == poolDict.TryGetValue(gameObjectName, out goList))
        { 
            Debug.Log("없넌 오브젝트.");
            return null;
        }


        GameObject gobj;
        if (goList.Count == 0)
        {
            gobj = Instantiate(prefabIndexDict[gameObjectName]);
    
        }
        else 
        {
            gobj = goList[0];
            goList.RemoveAt(0);
        }

        gobj.SetActive(true);
        gobj.transform.position = position;

        return gobj;
    }


    public void Recycle(string gameObjectName, GameObject go)
    {
        go.SetActive(false);
        go.transform.position = Vector3.zero;

        poolDict[gameObjectName].Add(go);
    }

    //GameObject tt;

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.X))
        //{
        //    tt =  ObjectPooler.Instance.Spawn( "NPC2", Vector3.zero);
        //}

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    ObjectPooler.Instance.Recycle("NPC2", tt);
        //}

    }


}


