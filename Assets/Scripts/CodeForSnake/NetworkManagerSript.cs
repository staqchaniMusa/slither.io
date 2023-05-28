using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
 
 
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;

public class NetworkManagerSript :MonoBehaviour
{
 

	public GameObject ourSnakeHead;
	public GameObject usernamePrefab;
	public string usernameHeadText;
	public static NetworkManagerSript instance;
	public PlayerState playerState;
	
	public enum PlayerProperties
	{
	updateScore,
	score
	}
	public enum PlayerState
	{
		playing,
	paused
	
	}
	 

	 
	void Awake()
	{
		instance = this;
	}

 
 
	public void CreateRoom()
	{
		
		
		
		
		
		if (UI_Manager.instance.usernameText.text=="" )
		{
			UI_Manager.instance.enterNameError.text="Please Enter UserName";

			UI_Manager.instance.enterNameError.gameObject.SetActive(true);
			return;
		}
		else if (UI_Manager.instance.yourAmountInputField.text=="")
		{
			UI_Manager.instance.enterBetErrorAI.gameObject.SetActive(true);
			 
			return;
		}
		else if  (int.Parse(UI_Manager.instance.yourAmountInputField.text) < 8)
		{
			UI_Manager.instance.enterBetErrorAI.text = "Please Enter Amount greater than 7";
			UI_Manager.instance.enterBetErrorAI.gameObject.SetActive(true);

			 
			return;
			
		}
		
		
		
		usernameHeadText = UI_Manager.instance.usernameText.text;
		UI_Manager.instance.SetPanelActive(5);
		
		
	//	UI_Manager.instance.playNowButton.gameObject.SetActive(false);
	//	UI_Manager.instance.searchingRoomText.gameObject.SetActive(true);
		UI_Manager.instance.myScore = float.Parse(UI_Manager.instance.yourAmountInputField.text);



		/*if (roomsList.Count>0)
		{
			PhotonNetwork.JoinRoom(roomsList[0].Name);
			
			Debug.Log("join room called from create room" +roomsList[0].Name + roomsList.Count());
			return;

		 
		
		/*if(roomsList.Count==0)
		  {
			 
		  }
		  else
		  {			  
			  for(int i = 0; i < roomsList.Count; i++)
			  {	  //if (roomsList[i].Name=="Slitherroom2")
				  {
					  
				//  PhotonNetwork.JoinRoom(roomsList[i].Name);
				 
				  }
					 
			  }
		  }*/
	}

























	private GameObject tempsnake;
     
	public void InstantiateSnake()
	{
		
		GameManager.instance.gameOver = false;

	 
		 	
		//UI_Manager.instance.minimapCamera.rect= new Rect (0f, 0f, 1f, 1);

			UI_Manager.instance.minimapCamera.orthographicSize = 250;
		//	UI_Manager.instance.minimap.anchoredPosition= new Vector2(-150,145);
		//	UI_Manager.instance.minimap.sizeDelta= new Vector2(187	,187);
			//UI_Manager.instance.rawimage.uvRect= new Rect (0f, 0f, 1f, 1);

			//GameManager.instance.CheckInternetInvoke();

			UI_Manager.instance.leftBorder.transform.position = new Vector3(-560, UI_Manager.instance.leftBorder.transform.position.y, UI_Manager.instance.leftBorder.transform.position.z);
			UI_Manager.instance.rightBorder.transform.position = new Vector3(560, UI_Manager.instance.rightBorder.transform.position.y, UI_Manager.instance.rightBorder.transform.position.z);
			UI_Manager.instance.topBorder.transform.position = new Vector3(  UI_Manager.instance.topBorder.transform.position.x,450, UI_Manager.instance.topBorder.transform.position.z);
			UI_Manager.instance.bottomBorder.transform.position = new Vector3(  UI_Manager.instance.bottomBorder.transform.position.x,-450, UI_Manager.instance.bottomBorder.transform.position.z);
			
			if (UI_Manager.instance.yourAmountInputFieldAI.text=="" )
			{
				UI_Manager.instance.enterNameErrorAI.text="Please Enter UserName";
				UI_Manager.instance.enterNameErrorAI.gameObject.SetActive(true);
				return;
			}
			
			MyInfo.localPlayerName = UI_Manager.instance.yourAmountInputFieldAI.text;
			
			//Debug.Log("Ai game"+MyInfo.localPlayerName);
			
		//	var templocalsnake = Instantiate(ourSnakeHead.transform, new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), 0), Quaternion.identity);
			
		var templocalsnake = Instantiate(ourSnakeHead.transform, new Vector3(0, 0,0), Quaternion.identity);

			templocalsnake.gameObject.tag = "Player";
			
			UI_Manager.instance.AddLeaderBoard(MyInfo.localPlayerName ,UI_Manager.instance.myScore);
			
			this.tempsnake = templocalsnake.gameObject;
			
			UI_Manager.instance	.SetPanelActive(5);
			 
			//StartCoroutine(WaitForGivenTime(10f));
			
		 
	}
	
	
	
	 
	 
	
	public IEnumerator WaitForGivenTime(float time)
	{
		
		yield return new WaitForSeconds(time);
		 
			UI_Manager.instance.myScore += 8;

			
		/*UI_Manager.instance.UpdateLeaderBoard(UI_Manager.instance.myScore ,MyInfo.localPlayerName);*/
		 
		UI_Manager.instance.screens.SetActive(false);
		//UI_Manager.instance.timerIsRunning = true;
		/*tempsnake.GetComponent<SpriteRenderer>().enabled = true;
		tempsnake.GetComponent<CollisionNetworkScript>().AddBodyPartsOnStart();
		tempsnake.GetComponent<Rigidbody>().isKinematic = false;*/
		 
	}

	 


}
 