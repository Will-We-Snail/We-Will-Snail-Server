using WillWeSnail;
using Lidgren.Network;
class mod : IExtension
{
    private class PlayerData
    {
        public Guid guid;
        public Double posX;
        public Double posY;
        public Boolean lookDir;
        public Int32 room;
        public double hSpeed;
        public double vSpeed;
        public PlayerData(Guid guid, Double posX, Double posY, Double velX, Double velY, Boolean lookDir, Int32 room)
        {
            this.guid = guid;
            this.posX = posX;
            this.posY = posY;
            this.hSpeed = velX;
            this.vSpeed = velY;
            this.lookDir = lookDir;
            this.room = room;
        }
    }

    private Dictionary<Guid, PlayerData> positions = new Dictionary<Guid, PlayerData>();

    public void ManagePacket(NetIncomingMessage message, NetServer server)
    {
        Guid guid = new Guid(message.ReadBytes(16));
        Int16 PacketId = message.ReadInt16();
        switch (PacketId)
        {
            case 1:
                Int32 LevelId = message.ReadInt32();
                Double PosX = message.ReadDouble();
                Double PosY = message.ReadDouble();
                Double velX = message.ReadDouble();
                Double velY = message.ReadDouble();
                Boolean lookDir = message.ReadBoolean();
                positions[guid] = new PlayerData(guid, PosX, PosY, velX, velY, lookDir, LevelId);
                break;
            default:
                Console.WriteLine("Invalid");
                break;
        }
    }

    public void Init() { }
    public Int16 GetID()
    {
        return 0;
    }
    public string GetName()
    {
        return "Movement";
    }

    public Int32 e(Int32 tickNumber, NetServer server)
    {
        //Console.WriteLine($"{positions.Count} players.");
        if (positions.Count == 0)
        {
            return 0;
        }
        NetOutgoingMessage outgoingMessage = server.CreateMessage();
        outgoingMessage.Write(GetID());
        outgoingMessage.Write((Int16)1);
        outgoingMessage.Write((Int16)positions.Count);
        foreach (PlayerData position in positions.Values)
        {
            outgoingMessage.Write(position.guid.ToByteArray());
            outgoingMessage.Write(position.room);
            outgoingMessage.Write(position.posX);
            outgoingMessage.Write(position.posY);
            outgoingMessage.Write(position.hSpeed);
            outgoingMessage.Write(position.vSpeed);
            outgoingMessage.Write(position.lookDir);
        }
        server.SendToAll(outgoingMessage, NetDeliveryMethod.ReliableSequenced);
        positions.Clear();
        return 0;
    }
    public Func<Int32, NetServer, Int32> GetPeriodic()
    {
        return e;
    }
}
