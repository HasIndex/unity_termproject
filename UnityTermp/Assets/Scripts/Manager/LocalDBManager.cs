using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class LocalDBManager : Singleton<LocalDBManager>
{
    private enum DBManagerState
    {
       Default , Save, 
    }


    private DBManagerState currentState = DBManagerState.Default;
    private float timer = 0.0f;

    public string NickName { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Hp { get; set; }

    private Dictionary<string, Schema> localDatabase = new Dictionary<string, Schema>();

    public bool HasSchema(string nick)
    {
        Schema schema;
        return localDatabase.TryGetValue(nick, out schema);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        LoadLocalDB();
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 5.0f && currentState == DBManagerState.Default)
        {
            LocalDBManager.Instance.SaveSchema(C2Client.Instance.Nickname);
            //currentState = DBManagerState.Save;
            timer = 0.0f;

            Debug.Log("save");
        }
    }

    public void LoadLocalDB()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("LocalDB");
        
        for(int n = 0; n < jsonFiles.Length; ++n)
        {
            string dbText = jsonFiles[n].text;  // 스트링에 로드된 텍스트 에셋을 저장

            Schema schema = JsonMapper.ToObject<Schema>(dbText);  // 맵퍼를 이용해서, 텍스트를 매핑. 

            localDatabase.Add(schema.nickname, schema);
        }
    }


    public bool UpdateSchema(string nick, int level = -1, int exp = -1, int hp = -1, int x = -1, int y = -1,  bool save = false)
    {
        Schema schema;
        if (localDatabase.TryGetValue(nick, out schema))
        {
            if(level != -1)
            {
                schema.level = level;
            }
            if(exp != -1)
            {
                schema.exp = exp;
            }
            if (hp != -1)
            {
                schema.hp = hp;
            }

            if (x != -1)
            {
                schema.x = x;
            }

            if (y != -1)
            {
                schema.y = y;
            }


            if (save == true)
            {
                SaveSchema(nick);
            }


            return true;
        }
        else
            return false;
    }


    public Schema SelectSchema(string nick)
    {
        Schema schema;
        if (localDatabase.TryGetValue(nick, out schema))
        {
            return schema;
        }
        else
            return null;
    }

    public bool TrySelectSchema(string nick, out Schema schema)
    {
        if (localDatabase.TryGetValue(nick, out schema))
        {
            return true;
        }
        else
            return false;
    }

    public bool InsertSchema(string nick, int level, int exp, int hp, int x, int y) // 없을시 생성.
    {
        string path = $"{Application.dataPath}/Resources/LocalDB/{nick}.txt";

        Schema schema;
        if (localDatabase.TryGetValue(nick, out schema) == false)  //  없는 경우.
        {            
            schema = new Schema {exp = exp, hp = hp, level = level, nickname = nick, x = x, y = y };

            JsonData schameData = JsonMapper.ToJson(schema);

            File.WriteAllText(path , schameData.ToString());

            localDatabase.Add(nick, schema);

            return true;
        }

        return false;
    }

    public bool DeleteSchema(string nick)
    {
        string path = $"{Application.dataPath}/Resources/LocalDB/{nick}.txt";

        if (File.Exists(path) == false)
        {
            File.Delete(path);

            return true;
        }
        else // 존재하는 경우 Update
        {
            return false;
        }
    }

    public void SaveSchema(string nick)
    {
        string path = $"{Application.dataPath}/Resources/LocalDB/{nick}.txt";

        Schema schema;
        if (localDatabase.TryGetValue(nick, out schema) == false)  //  없는 경우.
        {
            JsonData schameData = JsonMapper.ToJson(schema);

            File.WriteAllText(path, schameData.ToString());
        }
        else
        {
            JsonData schameData = JsonMapper.ToJson(schema);

            File.WriteAllText(path, schameData.ToString());
        }
    }

    public void SaveSchemaAsync(string nick)
    {
        //string path = $"{Application.dataPath}/Resources/LocalDB/{nick}.txt";

        //Schema schema;
        //if (localDatabase.TryGetValue(nick, out schema) == false)  //  없는 경우.
        //{
        //    JsonData schameData = JsonMapper.ToJson(schema);

        //    File.WriteAllText(path, schameData.ToString());
        //}
    }
}
