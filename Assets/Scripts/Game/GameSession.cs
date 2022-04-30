using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameServer.Game
{
    [Serializable]
    public class GameSession
    {
        private GameSetupData _gameSetupData;
        [SerializeField]private bool _isGameActive = false;
        [SerializeField]private float _timePassed = 0;
        [SerializeField]private List<PlayerInstance> _playerInstances = new List<PlayerInstance>();
        [SerializeField]private List<PodInstance> _podInstances;
        
        
        
        public GameSession(GameSetupData gameSetupData)
        {
            _timePassed = 0f;
            _gameSetupData = gameSetupData;
            _playerInstances = new List<PlayerInstance>();
            _podInstances = new List<PodInstance>();
            for (int i = 0; i < gameSetupData.PodCount; i++)
            {
                _podInstances.Add(new PodInstance()
                {
                    IsOn = false,
                    TimePassed = 0
                });
            }
            for (int i = 0; i < gameSetupData.PlayerData.Count; i++)
            {
                
                _playerInstances.Add(new PlayerInstance()
                {
                    PlayerData =  gameSetupData.PlayerData[i],
                    HitCount = 0,
                    TimePassed = 0
                });
            }

        }


        public void StartGame()
        {
            _isGameActive = true;
            _timePassed = 0f;
            switch (_gameSetupData.CompetitionMode)
            {
                case ECompetitionMode.Regular:
                    
                    break;
                case ECompetitionMode.FirstToHit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void UpdateGame()
        {
            if(!_isGameActive)
                return;
            switch (_gameSetupData.CompetitionMode)
            {
                case ECompetitionMode.Regular:
                {
                    _timePassed += Time.deltaTime;


                    // activity duration

                    #region check if game is finished based on activity duration mode

                    
                    switch (_gameSetupData.ActivityDurationMode)
                    {
                        
                        case EActivityDurationMode.Time:
                        {
                            
                            if (_timePassed >= _gameSetupData.ActivityDurationTimeSeconds)
                            {
                                EndGame();
                                return;
                            }
                            break;
                        }
                        case EActivityDurationMode.HitCountOrTime:
                        {
                            if (_timePassed >= _gameSetupData.ActivityDurationTimeSeconds)
                            {
                                EndGame();
                                return;
                            }

                            for (int i = 0; i < _playerInstances.Count; i++)
                            {
                                if (_playerInstances[i].HitCount >= _gameSetupData.ActivityDurationHitCount)
                                {
                                    EndGame();
                                    return;
                                }
                            }
                            
                            break;
                        }
                        case EActivityDurationMode.HitCount:
                        {
                            for (int i = 0; i < _playerInstances.Count; i++)
                            {
                                if (_playerInstances[i].HitCount >= _gameSetupData.ActivityDurationHitCount)
                                {
                                    EndGame();
                                    return;
                                }
                            }
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                  

                    #endregion
                    
                    for (int i = 0; i < _playerInstances.Count; i++)
                    {
                        var playerInstance = _playerInstances[i];
                        
                        #region Turn On Pod for player if has no pod

                        if (playerInstance.ActivePod == -1)
                        {
                             
                            playerInstance.ActivePod = GetRandomPod();
                            
                            var randomColor = playerInstance.PlayerData
                                .Colors[Random.Range(0, playerInstance.PlayerData.Colors.Count)];
                            
                            //function to turn on pod
                            TurnPodOn(playerInstance.ActivePod,true,randomColor);
                            //podInstance.CurrentColor = randomColor;
                            //podInstance.IsOn = true;
                        }


                        #endregion

                        #region increase pod passed time for timeout light 

                        

                        _podInstances[playerInstance.ActivePod].TimePassed += Time.deltaTime;

                        #endregion

                        #region Check if Pod is hit increase hit count and change active pod

                        
                        switch (_gameSetupData.LightsOutMode)
                        {
                            case ELightsOutMode.Hit:
                            case ELightsOutMode.HitOrTime:
                            {
                                var podInstance = _podInstances[playerInstance.ActivePod];
                                if (podInstance.IsHit)
                                {
                                    playerInstance.HitCount++;
                                    TurnPodOn(playerInstance.ActivePod,false, Color.black);
                                    //podInstance.IsOn = false;
                                    playerInstance.ActivePod = -1;
                                    
                                    //function to turn pod off
                                }
                                
                                
                                break;
                            }
                            
                            case ELightsOutMode.TimeOut:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        

                        #endregion


                        #region if light off is timeout mode check time out time for pod

                        
                        switch (_gameSetupData.LightsOutMode)
                        {
                            case ELightsOutMode.Hit:
                            {
                                break;
                            }
                            case ELightsOutMode.TimeOut:
                            case ELightsOutMode.HitOrTime:
                            {
                                if (_podInstances[_playerInstances[i].ActivePod].TimePassed >=
                                    _gameSetupData.LightsOutModeTimeOutSeconds)
                                {
                                    TurnPodOn(playerInstance.ActivePod,true,Color.black);
                                    //_podInstances[_playerInstances[i].ActivePod].IsOn = false;
                                    //turn off pod
                                    _playerInstances[i].ActivePod = -1;
                                    _playerInstances[i].MissCount++;

                                }
                                break;
                            }
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        
                        

                        #endregion
                        
                        
                    }
                    
                    break;
                }
                case ECompetitionMode.FirstToHit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void EndGame()
        {
            _isGameActive = false;
            for (int i = 0; i < _playerInstances.Count; i++)
            {
                _playerInstances[i].ActivePod = -1;
            }

            for (int i = 0; i < _podInstances.Count; i++)
            {
                TurnPodOn(i,false,Color.black);
                //_podInstances[i].IsOn = false;
                //turn off pod
            }
        }

        private List<int> _availablePodsTemp = new List<int>();
        public int GetRandomPod()
        {
            if (_availablePodsTemp == null)
                _availablePodsTemp = new List<int>();
            else
                _availablePodsTemp.Clear();


            for (int i = 0; i < _podInstances.Count; i++)
            {
                if(!_podInstances[i].IsOn)
                    _availablePodsTemp.Add(i);
            }

            return _availablePodsTemp[Random.Range(0, _availablePodsTemp.Count)];
        }
 

        //receive hit from pod
        public void PodHit(int podId)
        {
            _podInstances[podId].IsHit = true;
        }



        public void TurnPodOn(int podId, bool isOn,Color color)
        {
            _podInstances[podId].IsOn = isOn;
            _podInstances[podId].CurrentColor = color;
            //do turn on/off pod and set color
        }
    }
}