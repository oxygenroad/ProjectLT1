using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using  UnityEngine ;
using System.Collections;
using Random = UnityEngine.Random;


namespace GameServer
{
  public  class Server: MonoBehaviour
    {
       // private Client clientfile;
       
        public static int MaxPlayers { get; private set; }
        
        public static int Port { get; private set; }

       // in Client classiye ke khodesh filesh ro zade 
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        private static TcpListener tcpListener;
        public int Podsnumber=1;
    
         
          bool statePod1;
          // =clients[1].tcp.OnOrOff ; 
        

        //static chera ? 
         void Start()
        {
          //  clientfile = GameObject.Find("Client").GetComponent<Client>();
        }

        void update ()
         {
             statePod1 = clients[1].tcp.OnOrOff ; 

            
        }

        public static void Starting(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;
            

            //Console.WriteLine("Starting server...");
            Debug.Log( "Starting server...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            //Console.WriteLine($"Server started on port {Port}.");
            Debug.Log( $"Server started on port {Port}.");
            
        }

        // static chera ?
        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
           // Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");
            Debug.Log( $"Incoming connection from {_client.Client.RemoteEndPoint}...");
            

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client , i);
                    
                   // Console.WriteLine("listid after connect {0} clientdicId {1} - ip {2}" ,
                    //    clients[i].tcp.ListId, i , _client.Client.RemoteEndPoint );
                    Debug.Log( "listid after connect {0} clientdicId {1} - ip {2}" 
                       // , clients[i].tcp.ListId, i , _client.Client.RemoteEndPoint
                        );
                    // fek kunam gozashte ta avalin socket khali ro ke por kard az for beznebiroon 
                    return;
                }
            }
            
        }
        
        

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                // in Client classiye ke khodesh filesh ro zade 
                clients.Add(i, new Client(i));
            }
        }
        
        // az play mode kharej shim port baz mimune 
        public void StopServer()
        {
            
           

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket != null)
                {
                    clients[i].tcp.socket.Close() ; 
                    
                   // Console.WriteLine("listid after connect {0} clientdicId {1} - ip {2}" ,
                    //    clients[i].tcp.ListId, i , _client.Client.RemoteEndPoint );
                    Debug.Log( "client" + i + "is closed" );
                    // in return be che kar miad khodesh mire sare for dg  // baraye biron kardan az fore 
                     // return; // inja ehtiaji nist  
                     // vali for man ta tah max ro mire 
                }

                else if (clients[i].tcp.socket==null )
                 {
                  Debug.Log( "there is no socket from client : " + i   );
                  return ;
                }
            }


             tcpListener.Stop();
            // bayad stream ha va socket haro ham End kunim 
            // to do 
            // akharesh stop kardam 
        }
    NetworkStream stream2 ;

        public void TurnOn(int clinetKey)
        {
            byte[] on = { 0x42, 0x72, 0x41, 0x74, 2, 0x10, 0xFF, 0xFF, 0xFF, 0x0A };
            // bayad az khat zir estefade beshe 
            //  on[4] = clients[numberOfPod].tcp.ListId;
          
            // byte forTest = 11;
            // on[4] = forTest; 
          
            // in stream nabayad close she >? 
           // NetworkStream
             stream2 =  clients[clinetKey].tcp.socket.GetStream();
            Debug.Log("stream1 created ") ;
            stream2.Write(on, 0, on.Length);
            // vaziat ro on mikunim 
            clients[clinetKey].tcp.OnOrOff = true; 
            statePod1 = true ;
            //in stream baz mimone 
        }

       

        //baayada az LisId estefade she  
        // ma key dictinary ro midim khodesh ListId dara khode value.listid
        public void TurnOffWithId(int i )
        {
            byte[] off = {0x42, 0x72, 0x41, 0x74, 0, 0x25, 0x0A};
            //badan az in estefade mishe dar version badi pods 
            //off[4]= clients[i].tcp.ListId;
            byte testListId = 2;
            off[4] = testListId; 
            NetworkStream OffStream = clients[i].tcp.socket.GetStream(); 
            OffStream.Write( off , 0 , off.Length);
            clients[i].tcp.OnOrOff = false; 
            
        }


        // dorost kar mikune Vali 
        // vali barname gir mikune too in loop hich kare dg E nemishe kard ta tamom she 
        public void play3()
        {
            int random2 = Random.Range(1, Podsnumber+1);
            TurnOn(random2);
           // Debug.Log (podState) ;
            int GameRepeat = 1; 
            bool podState = clients[random2].tcp.OnOrOff ;
            
            
            while(GameRepeat<3)
            {
                // in pod state ke update nemishe 
                if (clients[random2].tcp.OnOrOff == false)
                {
                    random2 = Random.Range(1, Podsnumber+1); 
                    TurnOn(random2);
                    GameRepeat++; 
                    continue; 
                } 
            }
        }

         public IEnumerator Play4() 
         {
                    int random2 = Random.Range(1, Podsnumber+1);
                    int GameRepeat = 1; 
                    

                    Debug.Log ("game start ") ;

                     TurnOn(random2);
                     bool podState = clients[random2].tcp.OnOrOff ;
                     Debug.Log (" before while "+podState) ;

                     while (GameRepeat<3) 
                     {
                         Debug.Log("in while loop "+podState);
                         
                         if (clients[random2].tcp.OnOrOff  == false)
                         {
                             Debug.Log("first of if "+podState);
                            random2 = Random.Range(1, Podsnumber+1); 
                            TurnOn(random2);
                            // IN TURNON YE STREAM DG MISAZE HAR DAFE  IN KHUB NIST 
                             podState = clients[random2].tcp.OnOrOff ; 
                              GameRepeat++; 
                              Debug.Log("End of if "+podState);
                            // in continue nabashe inja ham okeye 
                            continue; 
                          }
                           yield return null ;
                     }

        }


// yek pod ro roshan mikune bazikun zarbe bezane pod badi ro roshan mikune 
        public IEnumerator Play6() {
            int hitsNumber = 0 ; 
            int random2 = Random.Range(1, Podsnumber+1);  
           
            while ( hitsNumber<10) {
               // yield return null ; 
               // shart in IF optional ast ya mitune 2s begzare 
               // to tanzimat bayad bege hit ya zaman 

              // bekhatere hamin ma faghat bayad ye bool taht onvane SHART bedim be if 
              statePod1 = clients[random2].tcp.OnOrOff;
                if (statePod1==false) {

                    hitsNumber++ ; 
                    TurnOn(random2) ; 
                    
                }

                // else if ( i>=6){
                //     Debug.Log ("hi mohi") ;
                //     i++ ;
                // }
               yield return null ; 
                //yield return new WaitForSeconds(1f) ;
            }

            
        }

            public IEnumerator Play7() {
            int i = 1 ; 
            int random2 = Random.Range(1, Podsnumber+1);

            while ( i<10) {

                if (i<6) {

                    TurnOn(1) ; 
                    i++ ; 
                }

                else if ( i>=6){
                    Debug.Log ("hi mohi") ;
                    i++ ;
                }
                yield return new WaitForSeconds(1f) ;
            }

            
        }

        public void Play5() {
            StartCoroutine(Play6()) ; 
        }
        
        
        
        
        
        
        
        
        
        
        
        

        
        
        //***Ignore lines below***
        
        // mn zadam
        // public Client Cli1 = new Client(1);
        // private Client cli2;
        
        // این زیریا چرک نویسه 
        
        // private void SortClient()
        // {
        //     clients[0] = client
        // }
        // // من نوشتم 
            
        // clients[1] = clients[2];
        // clients[Client.TCP.ListId].tcp.Connect(_client);
        // // chera tcp ro nrmitunam estefade kunam 
        // Client.tcp.Connect(_client);
        // clients[Client.TCP.ListId] = _client ;
        // // khodam zadam

        //  Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        // do debuglog
        
        
        public static void WriteData(int j , bool x)
        {
            // Console.WriteLine("listid after connect {0} clientdicId {1} - ip " ,
            //     clients[j].tcp.ListId, j );
            if (x == true)
            {
                byte[] on = { 0x42, 0x72, 0x41, 0x74, 11, 0x10, 0xFF, 0xFF, 0xFF, 0x0A };
                on[4] = clients[j].tcp.ListId;
                NetworkStream stream1 =  clients[j].tcp.socket.GetStream();
                //  NetworkStream st1 = clients[1].tcp.stream  in karo nabayad kard 
                           
                stream1.Write(on, 0, on.Length); 
            }
            
        }
        
        public  void Testjust(int numberOfPod)
        {
            byte[] on = { 0x42, 0x72, 0x41, 0x74, 2, 0x10, 0xFF, 0xFF, 0xFF, 0x0A };
            // bayad az khat zir estefade beshe 
            //  on[4] = clients[numberOfPod].tcp.ListId;
          
            // byte forTest = 11;
            // on[4] = forTest; 
          
            // in stream nabayad close she >? 
            NetworkStream stream1 =  clients[numberOfPod].tcp.socket.GetStream();
                           
            stream1.Write(on, 0, on.Length); 
        }
        
        // in karo nakunim choon dastgah hang mikune 
        // دستگاه هنگ میکنه به خاطر لوپ . استفاده ای هم نداره این منطق 
        // **** ignore this method 
        public void TurnOnWithoutId()
        {
            byte[] on = { 0x42, 0x72, 0x41, 0x74, 11, 0x10, 0xFF, 0xFF, 0xFF, 0x0A };
            int r;
            byte test = 1;
           
            foreach (var item  in clients)
            {
                NetworkStream stream1 =  item.Value.tcp.socket.GetStream();
                
                for ( r = 1; r < 12; r++)
                {
                    
                    on[4] = test;
                    stream1.Write(on, 0, on.Length); 
                    test++;
                }
                stream1.Close();
            }
            
        }
        public void play2()
        {
            // kare dorostiye ? jaye dorostiye ? 
           // Server nemone = new Server();
          //  int i = 1;
            //int random1 = Random.Range(1, Podsnumber);
            //nemone.TurnOn
            
            StartCoroutine(Wait2Seconds());
            
           // new WaitForSeconds(2);
            
           //StartCoroutine(Wait2Seconds()); 


            // khub in shart dar in lahze nist 
            // dar vaghe ma bayad montazer zarbe bemonim >>>> bia az while estefade kunim 




            // while (true)
            // {
            //     if (clients[random1].tcp.OnOrOff == false)
            //     {
            //         random1 = Random.Range(1, Podsnumber);
            //        TurnOn(random1);
            //         //i++;
            //         continue;
            //     }
            // }
        }

        public IEnumerator Wait2Seconds()
        {
            
            int random1 = Random.Range(1, Podsnumber);
            bool onOrOffState = clients[random1].tcp.OnOrOff; 
            
            TurnOn(random1);
            yield return new WaitForSeconds(2.0f);
            if (onOrOffState==true)
            {
                TurnOffWithId(random1);
            }
            
           // in ziriya comment soal dor badi bazi che joori ejra she ?  
            yield return new WaitForSeconds(2.0f); 
            TurnOn(random1);
        }
        
        
        // public void SetOffState(byte[] income)
        // {
        //     
        //     
        //     if (clients[i].tcp._data.Length ==7)
        //     {
        //          
        //     }
        //     // baste moarefi bashe na baste khomosh shudan 
        //
        //     // yani hit shude va baste khamosh omade 
        //      if (income.Length == 7)
        //     {
        //         string stateSt = Server.clients[clientsId].tcp.OnOrOff.ToString();
        //         Debug.Log("hit");
        //         Debug.Log(stateSt + clientsId);
        //         // az in ziir nemishe taghiir dad 
        //         Server.clients[clientsId].tcp.OnOrOff = false;
        //         //  this.OnOrOff = false; 
        //         Debug.Log( stateSt);
        //         // && income ==  baste khamoshh
        //         //avalna  mikham vaziat ro az on off kune vali nemidunam va dasresi be clientkey nadaram 
        //     }
        //         
        // }
    }
}
