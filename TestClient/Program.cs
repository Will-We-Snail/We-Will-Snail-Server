using Lidgren.Network;
namespace client;
class Client{
    public static void Main(){
        NetPeerConfiguration config = new NetPeerConfiguration("Will We Snail?");
        NetClient client = new NetClient(config);
        client.Start();
        client.Connect("127.0.0.1", 42069);
        while (true){
            NetIncomingMessage message;
            while ((message = client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // Decoding x y z and room position

                        break;
                    default:
                        Console.WriteLine($"Unhandled message type {message.MessageType}.");
                        Console.WriteLine(message.PeekString());
                        break;
                }
            }
                var SendingMessage = client.CreateMessage();
                Int16 num = 0;
                SendingMessage.Write(num);
                SendingMessage.Write((Int16) 32);

                client.SendMessage(SendingMessage, NetDeliveryMethod.ReliableOrdered);
                Console.WriteLine("Sending");
        }
    }
}
