using System;
using System.Runtime.InteropServices;
using UnityEngine;

public struct LoadedPlayerData
{
    public int level;
    public int hp;
    public int exp;
    public int y;
    public int x;
}


public class C2Client : Singleton<C2Client>
{
    public C2Session session;
    [SerializeField] PlayerMovement player;

    public LoadedPlayerData PlayerData { get; set; }

    public string Nickname { get; set; } = "default";

    public C2Client(PlayerMovement playerMovement)
    {
        session = C2Session.Instance;
        session.Client = this;
        player = playerMovement;

    }

    public void Start()
    {
        DontDestroyOnLoad(this);
        if (session == null)
        { 
            session = C2Session.Instance;
            session.Client = this;
        }
    }

    private void Update()
    {
    }

    public void SendMovePakcet(PlayerMovement player)
    {
        cs_packet_move movePayload;//
        movePayload.header.size = (sbyte)Marshal.SizeOf<PacketHeader>();
        movePayload.header.type = PacketType.C2S_MOVE;
        movePayload.move_time = 0;
        movePayload.direction = player.Direction ;// player.direction;

        session.SendPacket<cs_packet_move>(movePayload);
    }

    public void SendAttackPakcet(PlayerMovement player)
    {
        //cs_packet_move movePayload;//
        //movePayload.header.size = (sbyte)Marshal.SizeOf<PacketHeader>();
        //movePayload.header.type = PacketType.C2S_MOVE;
        //movePayload.move_time = 0;
        //movePayload.direction = player.Direction;// player.direction;
        //session.SendPacket<cs_packet_move>(movePayload);
    }

    public void SendPakcet<T>(T packet)
    {
        session.SendPacket<T>(packet);
    }

    public PlayerMovement Player
    {
        get
        {
            return player;
        }
        set
        {
            player = value;
        }
    }



    /// <summary>
    //
    /// </summary>
    public unsafe void Login()
    {
        C2Session c2Session = C2Session.Instance;

        cs_packet_login loginPacket;
        loginPacket.header.type = PacketType.C2S_LOGIN;
        loginPacket.header.size = (sbyte)Marshal.SizeOf(typeof(cs_packet_login));


        byte[] unicodeByte = System.Text.Encoding.Unicode.GetBytes(C2Client.Instance.Nickname);
        int nicknameLength = unicodeByte.Length > (int)Protocol.MAX_ID_LEN ? (int)Protocol.MAX_ID_LEN : unicodeByte.Length;
        Marshal.Copy(unicodeByte, 0, (IntPtr)loginPacket.name, nicknameLength);

        c2Session.SendPacket(loginPacket);
    }
}
