using System.Text;
using Bolt;
using UdpKit;
using UnityEngine.Events;

public class RoomInfoToken : IProtocolToken
{
    public string ClientVersion;
    public UdpEndPoint UdpEndPoint;
    public bool IsVisible;
    public string RoomName;
    public BattleTypes BattleType;
    public int Cur_PlayerNumber;
    public int Max_PlayerNumber;
    public bool HasPassword;
    public string Password;
    public Status M_Status;

    public UnityAction OnRoomButtonClick;

    public enum Status
    {
        Waiting = 0,
        Full = 1,
        Playing = 2,
        Closing = 3,
    }

    public void Read(UdpPacket packet)
    {
        ClientVersion = packet.ReadString(Encoding.UTF32);
        uint packedAddress = packet.ReadUInt();
        ushort port = packet.ReadUShort(16);
        UdpEndPoint = new UdpEndPoint(new UdpIPv4Address(packedAddress), port);
        IsVisible = packet.ReadByte(8) == 0x00;
        RoomName = packet.ReadString(Encoding.UTF32);
        BattleType = (BattleTypes) packet.ReadInt(32);
        Cur_PlayerNumber = packet.ReadInt(32);
        Max_PlayerNumber = packet.ReadInt(32);
        HasPassword = packet.ReadByte(8) == 0x00;
        Password = packet.ReadString(Encoding.UTF32);
        M_Status = (Status) packet.ReadInt(32);
    }

    public void Write(UdpPacket packet)
    {
        packet.WriteString(ClientVersion,Encoding.UTF32);
        packet.WriteUInt(UdpEndPoint.Address.Packed);
        packet.WriteUShort(UdpEndPoint.Port);
        packet.WriteByte((byte) (IsVisible ? 0x00 : 0x01));
        packet.WriteString(RoomName, Encoding.UTF32, RoomName.Length);
        packet.WriteInt((int) BattleType);
        packet.WriteInt(Cur_PlayerNumber);
        packet.WriteInt(Max_PlayerNumber);
        packet.WriteByte((byte) (HasPassword ? 0x00 : 0x01));
        packet.WriteString(Password, Encoding.UTF32, Password.Length);
        packet.WriteInt((int) M_Status);
    }
}