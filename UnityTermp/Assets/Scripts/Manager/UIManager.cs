using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{ 
    Login, Play, Chat
}


public class UIManager : Singleton<UIManager>
{
    [SerializeField] PlayerMovement player;

    public UIState CurrentState { get; set;  } = UIState.Login;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Update()
    {

    }

}
