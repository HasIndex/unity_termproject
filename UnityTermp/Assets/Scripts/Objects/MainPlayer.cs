using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public enum PlayerState
{
    Walk, Attack, Interact, PathFinding
}

public class MainPlayer : Singleton<MainPlayer>
{
    public PlayerState      currentState;
    public float            speed;
    private Rigidbody2D     myRigidbody;
    private Vector3         change;
    private byte            direction;
    private Animator        animator;
    private C2Client        client;

    public int Level { get; set; } = 1;
    public int Exp { get; set; } = 0;
    public sbyte Direction { get; set; } = 0;


    [SerializeField] private Stat       hp;
    [SerializeField] private Stat       exp;
    [SerializeField] private Portrait   portrait;

    void Awake()
    {
        DontDestroyOnLoad(this);

        hp.Initialize(200, 200);
        exp.Initialize(200, 200);
        portrait.SetLevel(1);

        currentState    = PlayerState.Walk;
        animator        = GetComponent<Animator>();
        myRigidbody     = GetComponent<Rigidbody2D>();

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

        // 처음에 안비추고 게임씬에서 비춤.
        //enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.CurrentState != UIState.Play)
            return;

        change = Vector3.zero;
        if(Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            change.y = +1.0f;
            C2Client.Instance.SendMovePacket((sbyte)ServerDirection.Down);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) == true)
        {
            change.y = -1.0f;
            C2Client.Instance.SendMovePacket((sbyte)ServerDirection.Up);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) == true)
        {
            change.x = -1.0f;

            C2Client.Instance.SendMovePacket((sbyte)ServerDirection.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) == true)
        {
            change.x = +1.0f;

            C2Client.Instance.SendMovePacket((sbyte)ServerDirection.Right);
        }


        // path finding 우선.
        if (currentState != PlayerState.Attack && Input.GetButtonDown("attack"))
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.Walk)
        {
            UpdateAnimatorAndMove();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
        if( scene.name == "1_Game_mmo")
        {
            C2Client.Instance.Player = this;
            //NetworkManager.Instance.Player = this;

            // stat
            hp.Initialize(C2Client.Instance.PlayerData.hp, 200);
            //exp.Initialize(C2Client.Instance.PlayerData.exp, 200 * C2Client.Instance.PlayerData.level);
            portrait.SetLevel(C2Client.Instance.PlayerData.level);

            // 좌표
            MoveCharacterUsingServerPosition(C2Client.Instance.PlayerData.y, C2Client.Instance.PlayerData.x);
        }
    }


    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.Attack;

        yield return null;

        animator.SetBool("attacking", false);

        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.Walk;
    }

    private void UpdateAnimatorAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    public void ParseLoginPacket( sc_packet_login_ok payload )
    {

    }

    void MoveCharacter()
    {
        //change.Normalize();
        myRigidbody.MovePosition(transform.position + change);
    }

    public void MoveCharacterUsingServerPosition(int y, int x)
    {
        Vector3 vector = new Vector3();
        vector.x = x;
        vector.y = y;
        Debug.Log($"move server postion x {x}, y {y}");
        myRigidbody.MovePosition(vector);
    }

    public void SetHP(int minHp, int maxHp)
    {
        hp.Initialize(minHp, maxHp);
    }

    public void SetLevel(int level)
    {
        //portrait.
    }


    public void SetStat(int level, int hp, int exp)
    {
        portrait.SetLevel(level);
        this.hp.CurrentValue = hp;
        this.exp.CurrentValue = exp;
    }

}
