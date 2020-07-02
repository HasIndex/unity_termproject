using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class ConfigLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextAsset jsonFile = Resources.Load("config") as TextAsset;

        string MapText = jsonFile.text;  // 스트링에 로드된 텍스트 에셋을 저장

        ServerConfig serverConfig = JsonMapper.ToObject<ServerConfig>(MapText);  // 맵퍼를 이용해서, 텍스트를 매핑. 

        Debug.Log($"{serverConfig.serverIP} : {serverConfig.serverPort}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
