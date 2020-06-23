using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class InitialiPacketHandler : C2PacketHandler
{
    public InitialiPacketHandler() : base()
    {
        handlers[(Int32)PacketType.S2C_LOGIN_OK] = OnLoginOk;
        handlers[(Int32)PacketType.S2C_LOGIN_FAIL] = OnLoginFail;

    }

    private void Chat(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
        throw new NotImplementedException();

        sc_packet_chat chatPayload;

        payload.Read(out chatPayload);
    }


    // 로그인 확인.
    void OnLoginOk(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
        sc_packet_login_ok loginOkPayload;

        payload.Read(out loginOkPayload);

        C2Session.Instance.uniqueSessionId = (Int64)loginOkPayload.id;

        C2Client.Instance.Player.MoveCharacterUsingServerPostion(loginOkPayload.y, loginOkPayload.x);
        C2Client.Instance.Player.Level = loginOkPayload.level;
        C2Client.Instance.Player.SetHP(loginOkPayload.hp, loginOkPayload.level * 2);
        C2Client.Instance.Player.Exp = loginOkPayload.exp;
    }

    void OnLoginFail(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
        sc_packet_login_fail loginFailPayload;

        payload.Read(out loginFailPayload);

        //C2Session.Instance.uniqueSessionId = (Int64)loginOkPayload.id;
    }



    void Enter(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
        sc_packet_enter enterPayload;
        payload.Read(out enterPayload);


        
    }


    // 로그인 씬에서 나감. 사실상 연결 끊기.
    void Leave(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
        sc_packet_leave leavePayload;

        payload.Read(out leavePayload);
    }

    // login server to my client
    void ResponseLogin(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
    }

    // 회원가입
    void ResponseRegistration(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
    }

    // 로그인 씬에서 나감.
    void ResponseExitLogin(PacketHeader header, C2PayloadVector payload, C2Session session)
    {
    }

}