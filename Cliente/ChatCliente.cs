using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2.Client;
using WampSharp.V2;

namespace Cliente
{
    internal class ChatCliente
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Chat cliente #1");

            const string serverAddress = "ws://127.0.0.1:8080/ws";
            const string realmName = "realm1";
            const string topicName = "com.myapp.chat";

            DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();
            IWampChannel channel = channelFactory.CreateJsonChannel(serverAddress, realmName);

            await channel.Open().ConfigureAwait(false);

            Console.WriteLine("Conectando al servidor...");

            IWampRealmProxy realm = channel.RealmProxy;
            ISubject<string> subject = realm.Services.GetSubject<string>(topicName);

            Console.WriteLine("Ingrese un usuario");
            string username = Console.ReadLine();

            subject.OnNext($"Ha iniciado sesion {username}");

            IDisposable subscription = subject.Subscribe(message =>
            {
                Console.WriteLine(message);
            });

            Console.WriteLine($"{username} escriba un mensaje o 'exit' para salir del chat.");

            string input;
            do
            {
                input = Console.ReadLine();

                if (input != "exit")
                {
                    string message = $"{username}: {input}";
                    subject.OnNext(message);
                }

            } while (input != "exit");

            subject.OnNext($"Se ha desconetado el usuario: {username}");

            subscription.Dispose();
            channel.Close();

            Console.WriteLine("Desconexion del chat");
        }
    }
}
