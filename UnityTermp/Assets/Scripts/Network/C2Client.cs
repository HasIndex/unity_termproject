using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class C2Client : Singleton<C2Client>
{
    public C2Session session;
    public string nickname = "default";
    [SerializeField] PlayerMovement player;

    public C2Client(PlayerMovement playerMovement)
    {
        session = C2Session.Instance;
        session.Client = this;
        player = playerMovement;
    }

    public void Start()
    {
        if (session == null)
        {
            session = C2Session.Instance;
            session.Client = this;
        }


        nickname = "default";
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
}
