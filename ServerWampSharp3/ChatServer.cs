using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.Realm;

namespace ServerWampSharp3
{
    internal class ChatServer
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Iniciando servidor espere un momento....");
            const string serverAddress = "ws://127.0.0.1:8080/ws";
            const string realmName = "realm1";
            const string topicName = "com.myapp.chat";

            DefaultWampHost host = new DefaultWampHost(serverAddress);
            host.Open();

            Console.WriteLine("Server started.");

            IWampHostedRealm realm = host.RealmContainer.GetRealmByName(realmName);
            ISubject<string> subject = realm.Services.GetSubject<string>(topicName);

            IDisposable subscription = subject.Subscribe(message =>
            {
                Console.WriteLine(message);
            });

            Console.WriteLine("Servidor iniciado en la ubicacion: "+serverAddress+" presione enter para terminar");

            Console.ReadLine();

            subscription.Dispose();
            host.Dispose();

            Console.WriteLine("Servidor detenido");
        }
    }
}
