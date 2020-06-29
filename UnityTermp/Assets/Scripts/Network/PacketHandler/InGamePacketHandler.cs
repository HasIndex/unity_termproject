


using System;
using System.Runtime.InteropServices;
using UnityEditor.U2D.Animation;

public class InGamePacketHandler : C2PacketHandler
{
    public InGamePacketHandler() : base()
    {
        handlers[(Int32)PacketType.S2C_MOVE]            = OnMove;
        handlers[(Int32)PacketType.S2C_ENTER]           = OnEnter;
        handlers[(Int32)PacketType.S2C_LEAVE]           = OnLeave;
        handlers[(Int32)PacketType.S2C_CHAT]            = OnChat;
        handlers[(Int32)PacketType.S2C_STAT_CHANGE]     = OnStatChange;

    }

    unsafe void OnChat(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
        sc_packet_chat chatPayload;

        payload.Read(out chatPayload);

        char* chatPtr = chatPayload.chat;
        
        var chatString = new String(chatPtr);

        ChatManager.Instance.AddChat(chatString, MessageType.User);
    }



    void OnEnter(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
        sc_packet_enter enterPayload;

        payload.Read(out enterPayload);


    }


    // 이동
    void OnMove(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
        sc_packet_move movePayload;

        payload.Read(out movePayload);

        // 네트웥크 아이디 찾아서 
        //var go;//NetworkManager.Instance. movePayload.id;

        // 움직여줌.
        //go.move_to(movePayload.x, movePayload.y);
    }

    // 로그인 씬에서 나감. 사실상 연결 끊기.
    void OnLeave(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
        sc_packet_leave leavePayload;

        payload.Read(out leavePayload);
    }


    private void OnStatChange(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
        sc_packet_stat_change statChangePayload;

        payload.Read(out statChangePayload);

        // 스탯 업데이트 해줌.
        MainPlayer.Instance.SetStat(statChangePayload.level, statChangePayload.hp, statChangePayload.exp);
    }

    //// 회원가입
    //void ResponseRegistration(PacketHeader header, C2PayloadVector payload, C2Session session)
    //{
    //}

    //// 로그인 씬에서 나감.
    //void ResponseExitLogin(PacketHeader header, C2PayloadVector payload, C2Session session)
    //{
    //}
}
