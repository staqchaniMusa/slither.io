 using System;
using System.Collections;
using System.Collections.Generic;
 
 using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public int gameWinningKills=3; //number of kills to win the game
    public int localPlayerKills;
    public AIManager AIManager;
    public int localPlayerOrbcounter;
    public int isWinner;
    public int myViewId;
   
    
    
    
    public List<GameObject> players;
    public List<GameObject> playerParent;

    public bool gameOver;


    public bool joinGame=true;
    public enum GameStatus
    {
        Online,
        Offline
    }


    public GameStatus gameStatus;
    public bool internetstatus=true;
    
    public CollisionNetworkScript mySnakeCollisonNetworkScript;
    public SnakeMovement snakeMovementScript;
    void Start()
    {

        /*if (MyInfo.IsOnline())
        {
          CheckInternetInvoke();
        }*/
         
    }

  public void  CheckInternetInvoke()
    {
        InvokeRepeating("CheckInternet",1f,2f);
    }
    public void CheckInternet()
    {
        StartCoroutine(CheckInternetConnection(isConnected =>
        {
          
            
            if (isConnected)
            {
                
                internetstatus = true;
               UI_Manager.instance.internnetPopUPpanel.SetActive(false);
               
              // Debug.Log("Internet Available!");
            }
            else
            {


                if (!internetstatus)
                {return;
                    
                }

                internetstatus = false;
                UI_Manager.instance.internnetPopUPpanel.SetActive(true);
  
                 


                
                    

                    if (GameManager.instance.snakeMovementScript.foodCounter==5000)
                    {
                    mySnakeCollisonNetworkScript.KillSnake("Internet", true);
                        
                    }
                
                    snakeMovementScript.StopCoroutine();
                
               
            
                 
                LeaveFromRoom(0.1f);
              
            }
        }));
        
    }


 
    
    
   
    public void LeaveFromRoom(float time)
    {
       Debug.Log("leave room");


       if (!joinGame)
       {
           Debug.Log("not joined game");
           UI_Manager.instance.SetPanelActive(8);
           UI_Manager.instance.screens.SetActive(true);

       }
       
      
        UI_Manager.instance.StartCoroutine(UI_Manager.instance.WaitForGivenTime(time));

    }
    
    IEnumerator CheckInternetConnection(Action<bool> action)
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
        if (request.error != null) {
          //  Debug.Log ("Error");
            action (false);
        } else{
//            Debug.Log ("Success");
            action (true);
        }
    }
    
    private void Awake()
    {
        instance = this;
    }
    public void CheckWIn()
    {
        if (gameWinningKills<=localPlayerKills && !UI_Manager.instance.timerIsRunning)
        {
            UI_Manager.instance.gameLosePanel.SetActive(false);

            UI_Manager.instance.gameWinPanel.SetActive(true);
           // Debug.Log("winner");
            isWinner = 1;
        }
        else
        {
            UI_Manager.instance.gameWinPanel.SetActive(false);

            UI_Manager.instance.gameLosePanel.SetActive(true);

            //Debug.Log("loser");
            isWinner = 0;
        }
    }
}
 