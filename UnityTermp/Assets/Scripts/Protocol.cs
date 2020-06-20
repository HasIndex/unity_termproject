

using System;
using System.Runtime.InteropServices;


public enum PacketType : sbyte
{
	PT_NONE,

	S2C_LOGIN_OK = 1,
	S2C_LOGIN_FAIL,
	S2C_MOVE,
	S2C_ENTER,
	S2C_LEAVE,
	S2C_CHAT,
	S2C_STAT_CHANGE,
	PT_MAX,

	C2S_LOGIN = 1,
	C2S_MOVE,
	C2S_ATTACK,
	C2S_CHAT,
	C2S_LOGOUT,
};

public enum Protocol
{
	HEADER_SIZE = 2,
	MAX_ID_LEN = 50,
	MAX_CHAT_LEN = 50,
	MAX_STR_LEN = 255
}


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct PacketHeader
{
    public sbyte		size;
    public PacketType	type;
}


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct cs_packet_login
{
    public PacketHeader header;

    public fixed char name[(int)Protocol.MAX_ID_LEN];
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct cs_packet_move
{
	public PacketHeader header;

	public sbyte		direction;
	public UInt32		move_time;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct cs_packet_atttack
{
	public PacketHeader header;
};


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct cs_packet_chat
{
	public PacketHeader header;

	public	int			id;
	public fixed char	chat[(int)Protocol.MAX_CHAT_LEN];
};


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct cs_packet_logout
{
	public PacketHeader header;
};


[StructLayout(LayoutKind.Sequential, Pack = 1)]

public unsafe struct sc_packet_login_ok // 16
{
	public PacketHeader header;

	public int		id;
	public short	x, y;
	public short	hp;
	public short	level;
	public int		exp;
};


[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct sc_packet_login_fail
{
	PacketHeader header;
};


[StructLayout(LayoutKind.Sequential, Pack = 1)]

public unsafe struct sc_packet_move
{
	public PacketHeader header;

	public Int32 id;
	public short x, y;
	public UInt32 move_time;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct sc_packet_enter // 
{
	public PacketHeader header;

	public int id;
	public fixed char name[(int)Protocol.MAX_ID_LEN]; // wide-char로 변경 예정.
	public sbyte o_type;
	public short x, y;
};


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct sc_packet_leave
{
	public PacketHeader header;

	public int id;
};


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct sc_packet_chat
{
	public PacketHeader header;

	public int id;
	public fixed char chat[(int)Protocol.MAX_CHAT_LEN];
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct sc_packet_stat_change
{
	PacketHeader header;

	short	hp;
	short	level;
	int		exp;
};