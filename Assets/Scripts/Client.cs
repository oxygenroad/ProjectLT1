using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace GameServer
{
   public class Client
    {
        public static int dataBufferSize = 4096;

        public int id;
        
        
        public TCP tcp;

        // in instactor be ch hadafiye 
        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }

        public class TCP
        {

            // chera socket public vali stream private 
            public TcpClient socket;
            private readonly int id;
            // mn miam stream ro public mikunam ?! 
            private NetworkStream stream;
            private byte[] receiveBuffer;
            // chera static bayad bashe ta dakhel method static estefade beshe 
            // agar static nabashe dasresi daram behesh too server 
            
            // listId esme ghalatiye PodId doroste
            public  byte ListId  ;
            public int clientsId;

            public byte[] _data; 
            
            // vazit on or off  /// agar static kunim chi mishe ? 
            public  bool OnOrOff  =false ; 

            public TCP(int _id)
            {
                id = _id;
            }

            public void Connect(TcpClient _socket ,  int id )
            {
                // in parametr ro mn ezafe kardam 
                clientsId = id; 
                
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                

                // TODO: send welcome packet
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        // TODO: disconnect
                        return;
                    }
                    // bejaye inja sar class tarif kardam _data ro 

                     _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);
                    
                    ReadAndSetId(_data);
                   // miram to server karaye "hit" khunad ro mikunam  
                    
                    // Console.WriteLine("dataId is {0}from ip {1} and listid {2}", 
                    //     ListId.ToString() , socket.Client.RemoteEndPoint, ListId +2);

                    // TODO: handle data
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error receiving TCP data: {_ex}");
                    // TODO: disconnect
                }
            }
            // i write below

           public void ReadAndSetId(byte[] income)
            {
                // baste moarefi bashe na baste khomosh shudan 
                if (income.Length == 6)
                {
                    ListId = income[4];
                    // jalebe in code dorot kar mikune
                    // yani roye har nemone asar mikune 
                }
                    // yani hit shude va baste khamosh omade 
                 else if (income.Length == 7)
                 {
                     string stateSt = Server.clients[clientsId].tcp.OnOrOff.ToString(); 
                     
                     Debug.Log("hit");
                     Debug.Log(stateSt + clientsId);
                     
                     // az in ziir nemishe taghiir dad 
                     //   Server.clients[clientsId].tcp.OnOrOff = false;
                   //  this.OnOrOff = false; 
                   
                          //   biam khode OnOrOff ro meghdar bedam 
                          OnOrOff = false; 
                          // inam nashud
                          
                   //momkene taghir karde bashe 
                         
                   stateSt = Server.clients[clientsId].tcp.OnOrOff.ToString(); 
                     Debug.Log( stateSt);
                
                     // && income ==  baste khamoshh
                     //avalna  mikham vaziat ro az on off kune vali nemidunam va dasresi be clientkey nadaram 
                
                
                
                 }
                
            } 

           
        }
    }
}
