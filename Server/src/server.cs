using Lidgren.Network;
using System.Threading;
namespace WillWeSnail;
class Server
{
    public static int Main()
    {
        TextWriter logError = Console.Error;
        ExtensionLoader extensions = new ExtensionLoader("../mods");
        NetPeerConfiguration config = new NetPeerConfiguration("Will We Snail?") { Port = 42069 };
        NetServer server = new NetServer(config);
        server.Start();
        Boolean running = true;
        #pragma warning disable CS8622
        Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            running = false;
        };
        NetIncomingMessage message;
        long LastTickTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        long CurrentTime;
        long TickRate = 50;
        Int32 tickNumber = 0;
        while (running)
        {
            // Recieved Packet Management
            while ((message = server.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        Int16 ExtensionId = message.ReadInt16();
                        if (extensions.extensions.ContainsKey(ExtensionId))
                            extensions.extensions[ExtensionId].ManagePacket(message, server);
                        else
                            Console.WriteLine($"Unhandled extension id {ExtensionId}");
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine(message.SenderConnection.Status);
                        if(message.SenderConnection.Status == NetConnectionStatus.RespondedAwaitingApproval){
                            message.SenderConnection.Approve();
                        }
                        break;
                    default:
                        Console.WriteLine($"Unhandled message type {message.MessageType}.");
                        Console.WriteLine(message.PeekString());
                        break;
                }
            }


            // Periodic function running.
            foreach (Func<Int32, NetServer, Int32> Function in extensions.PeriodicFunctions)
            {
                Function(tickNumber, server);
            }
            CurrentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            int SleepTime = (int)(TickRate + LastTickTime - CurrentTime);
            LastTickTime = CurrentTime + SleepTime;
            if (SleepTime < 0)
            {
                logError.WriteLine($"Error: Tick has overrun by {-1 * SleepTime}, consider increasing the tick rate.");
            }
            else
            {
                Thread.Sleep(SleepTime);
            }
            tickNumber++;
        }
        return 0;
    }
}
