using UnityEngine;

namespace GameServer.Game.Mono
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField]private GameSetupData _gameSetup = new GameSetupData();
        [SerializeField]private GameSession _gameSession = null;


        // called from ui to use game setup configed in ui
        public void SetGameSetupData(GameSetupData gameSetupData)
        {
            _gameSetup = gameSetupData;
        }
        
        public void StartGame()
        {
            _gameSession = new GameSession(_gameSetup);
        }
    }
}