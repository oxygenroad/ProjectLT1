using System;



// as you see this code does not use in unity , 
// instead of this we use startServer class and method 

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Title = "Game Server";

            Server.Starting(50, 5000);
            
            if (Console.ReadKey(true).Key == ConsoleKey.I)
            {
               
               Server.WriteData(2 , true);
                Console.WriteLine(Server.clients[2].tcp.ListId);
            }

            

            Console.ReadKey();
        }
    }
}
