using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API_Manager : MonoBehaviour
{

    public Root _root;
    public Dashboard dashBoardData;
    public LoginInfo loginInfo;
    public static API_Manager instance;

    public string BaseUrl;

    public string walletAddress;
    
    private void Awake()
    {
        instance = this;
    }

     

 
  
    
    
    
    
    

     
 
    
    
}


[System.Serializable]
public class FoodDatum
{
    public int id;
    public string type;
    public int value;
    public int percentage;
}

[System.Serializable]
public class RoomData
{
    public int id;
    public string name;
    public int variable_stake;
    public int max_players;
    public long tokens;
    public long tokens_per_instance;
    public int min_minutes_to_win;
    public int min_kills_to_win;
    public double min_usd_to_join;
    public double max_usd_to_join;
    public DateTime created_at;
}

[System.Serializable]
public class Root
{
    public bool status;
    public RoomData room_data;
    public List<FoodDatum> food_data;
    public string message;
}

[System.Serializable]
public class LoginData
{
    public int id;
    public string public_address;
    public string is_admin;
    public DateTime created_at;
    public DateTime updated_at;
    public int game_mode_id;
}


[System.Serializable]
public class LoginInfo
{
    public bool status;
    public string message;
    public LoginData data;
    public string session_token;

}

[System.Serializable]
public class DashBoardData
{
    public int totalWinMatches ;
    public int totalLossMatches ;
    public int bestCareerHigh ;
    public int earnings ;
    public int losses ;
    public int recentRankOnline;
    public int recentRankOffline;
    public string nickname;
    public string snake_color;
    public int bestCareerHighScore;
}


[System.Serializable]
public class Dashboard
{
    public bool status ;
    public string message ;
    public DashBoardData data ;
}




