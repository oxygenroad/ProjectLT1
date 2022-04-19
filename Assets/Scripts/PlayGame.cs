using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI ; 

namespace  GameServer 
{
    public class PlayGame : MonoBehaviour
    {
        public int numberOfPods = 1; 
        public Button playGame;
        // Start is called before the first frame update
        void Start()
        {
            playGame = GetComponent<Button>();
            playGame.onClick.AddListener(play);
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        public void play()
        {
            // kare dorostiye ? jaye dorostiye ?  in ba public Server nemone2 ; che farghi dare ? 
            Server nemone = new Server();
            int i = 1; 
            int random = Random.Range(1, numberOfPods);
            nemone.TurnOn(random);
            
            // khub in shart dar in lahze nist 
            // dar vaghe ma bayad montazer zarbe bemonim >>>> bia az while estefade kunim 
            while (true)
            {
                if ( Server.clients[random].tcp.OnOrOff == false )
                {
                    random = Random.Range(1, numberOfPods);
                    nemone.TurnOn(random);
                    //i++;
                    continue;
                }
            }
            
            // while ( i < 20)
            // {
            //     if ( Server.clients[random].tcp.OnOrOff == false )
            //     {
            //         random = Random.Range(1, numberOfPods);
            //         nemone.TurnOn(random);
            //         i++;
            //         continue;
            //     }
            //     // else  
            // }
        }

        // chejoori zakhirash kunam ke khate paeen estefade kunam azash 
        private int RandomGenarate()
        {
            int random = Random.Range(1, 4);
            return random; 
        }
    }
    
}

