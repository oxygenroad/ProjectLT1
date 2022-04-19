using System.Collections;
using System.Collections.Generic;
using GameServer;
using UnityEngine;
using UnityEngine.UI ; 
//myObject.GetComponent<MyScript>().MyFunction();

public class startServer : MonoBehaviour
{
   private Server server;
   //chera mige estefade nashude ? 
   
   public Button startTheServer;
    // Start is called before the first frame update
    void Start()
    
    {
        // Find what ? 
        server = GameObject.Find("Server").GetComponent<Server>();
        startTheServer = GetComponent<Button>();
        startTheServer.onClick.AddListener(StartServer);





    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartServer()
    {
        
        Server.Starting(50, 5000);
    }
    
}
