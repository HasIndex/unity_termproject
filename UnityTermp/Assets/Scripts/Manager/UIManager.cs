using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{ 
    Play, Chat
}


public class UIManager : Singleton<UIManager>
{
    [SerializeField] PlayerMovement player;

    public UIState CurrentState { get; set;  } = UIState.Play;





}
