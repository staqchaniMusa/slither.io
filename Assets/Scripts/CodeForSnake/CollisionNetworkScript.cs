using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Threading;
using DG.Tweening;
using JetBrains.Annotations;
 
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class CollisionNetworkScript : MonoBehaviour
{



	public SnakeMovement snakeMovement;
	public int mySnakeMaxLength;
	 
	private void Start()
	{
		
		 

			if (!this.GetComponent<SnakeMovement>().isAI)
			{


				MyInfo.localPlayerName = UI_Manager.instance.yourAmountInputFieldAI.text.ToString();
				ChangeMyNameAndSkin(MyInfo.localPlayerName, UI_Manager.instance.currentSelectedSlitherIndex, 0);
				/*if (!gameObject.GetComponent<SnakeMovement>().isAI)
				{
					
				}*/
			}
		 

	}


	 
	void ChangeMyNameAndSkin(string myNewName,int index,int snakeSize)
	{
		gameObject.transform.name = myNewName;
		snakeMovement.SetSkin(index);
		snakeMovement.snakeLength=snakeSize;
	 
		
		
	}

	public int snakeDefaultLength=7;
	public Transform bodyObject;
	public Transform parentBody;
	public bool collidedwithenemy;
	 

	public void AddBodyPartsOnStart(int snakelength)
	{
 
			 
			AddBodyPartOfSnake(snakelength,gameObject.transform.name,0.2f,true);
		 
	}


	//add bodyparts on snake
	private int toAddSnakelength;
	private string snakename;
	
	//counter of remaining snake
	private int remainingCounter;
 
	public void AddBodyPartOfSnake(int snakelength, string name,float time,bool isfirst)
	{



		if (isfirst)
		{
			snakeMovement.orbCounter+=snakelength;
			snakeMovement.orbEaten+=snakelength;
		}
	

		toAddSnakelength = snakelength;
		
		//remainingCounter=snakelength%4;
		remainingCounter=snakelength ;

		//Debug.LogError("add "+snakelength + this.gameObject.name);
		
		snakename = name;
		AddBodyParts();
		//StartCoroutine(AddBodyParts());
			 
	}

	private IEnumerator AddBodyPartsCoroutine()
    {
		int m = 0;
		for (int i = 1; i <= toAddSnakelength; i++)
		{
			if (i < 9)
			{
				//Debug.Log("add body parts" + this.gameObject.name);
				AddThisSnakeNewBodyPart(snakename);

			}
			else if (i % 4 == 0 && snakeMovement.bodyParts.Count < mySnakeMaxLength + 1)
			{
				//Debug.Log("add body parts"+ this.gameObject.name);
				if(++m % 3 == 0)
				yield return null;
				AddThisSnakeNewBodyPart(snakename);

			}



		}
	}
	private void AddBodyParts()
	{

		//yield return new WaitForSeconds(0.5f);
		if(gameObject != null)
		StartCoroutine(AddBodyPartsCoroutine());
	}
	

 
 
	public int collisioncounter=0;
 
//on collieded with orb
	public void OnCollidedWithOrb(GameObject other)
	{

		 
			//UI_Manager.instance.UpdateLeaderBoard(UI_Manager.instance.myScore ,MyInfo.localPlayerName);//orb value
			if (!snakeMovement.isAI)
			{

				UI_Manager.instance.myScore += other.GetComponent<OrbGrowthScript>().value;
				UI_Manager.instance.myscoreText.text = UI_Manager.instance.myScore.ToString();
				
				
				if (collisioncounter==0)
				{
					collisioncounter+= remainingCounter;
					UI_Manager.instance.timerIsRunning = true;
				}
				snakeMovement.localPlayerScore+= other.GetComponent<OrbGrowthScript>().value;
				 
					UI_Manager.instance.UpdateLeaderBoard(UI_Manager.instance.myScore ,MyInfo.localPlayerName);


			}
			else
			{
				//if (collisioncounter!=0)
				{
				//	Debug.Log("add to local player "+ this.gameObject.name);
					snakeMovement.localPlayerScore+= other.GetComponent<OrbGrowthScript>().value;
					UI_Manager.instance.UpdateLeaderBoard(snakeMovement.localPlayerScore ,snakeMovement.gameObject.name);
				}
				 

			}
		 


			DeleteOrbForOthers(other.gameObject);
			var a = snakeMovement.bodyParts.Count;
			
			/*snakeMovement.orbEaten++;
			snakeMovement.orbCounter++;*/

			if (a< mySnakeMaxLength && a>8) //commented//my snake max length
			{
				
			 
				snakeMovement.runningCalled = false;
				//if (snakeMovement.orbEaten %4 == 0)
				{
					 
				AddBodyPartOfSnake(1,gameObject.transform.name,0.2f,false);
					
				}
				/*else
				{
					
				}*/
				
				
			//AddThisSnakeNewBodyPart(gameObject.transform.name);
		
			}
			
		 
		
	}

	public bool killed;
	
	void OnTriggerEnter (Collider other)
	{
		if (!snakeMovement.isvisible || Vector2.Distance(transform.position, Camera.main.gameObject.transform.position) > 30 )
		{
			if (other.gameObject.tag=="Portal" ||other.gameObject.tag=="Border")
			{


				if (!killed)
				{


					killed = true;
					AIManager.instance.AICounter--;

					int index = snakeMovement.localAiIndex;
					AIManager.instance.aiSnakes.Remove(this.gameObject);

					Debug.Log(index);

					AIManager.instance.InstantiateAISnakes(UnityEngine.Random.Range(1000, 3000), index);
					//AIManager.instance.InstantiateAISnakes(UnityEngine.Random.Range(8, 70), index);
					Destroy(this.gameObject);
				}
			}

		 
			
			return;
		}
	 
		/*if ((other.gameObject.tag == "orb" || other.gameObject.tag == "diedfood") && snakeMovement.bodyParts.Count >= snakeDefaultLength   &&  snakeMovement.isvisible )
		{
			/*if (Vector2.Distance(transform.position,other.transform.position) < 0.8f)
			{#1#
			
			Debug.Log("collided");
				 other.gameObject.GetComponent<OrbGrowthScript>().OrbTriggered(this.gameObject.transform);
			//OnCollidedWithOrb(other.gameObject);
			 
				//return;
			//}
		}*/
		//Debug.Log("collided food" + other.gameObject.tag);
		if (other.transform.tag == "AI" || other.transform.tag=="RaycastTarget")
		{
			return;
		}
		
		if (other.transform.tag != "orb" && other.transform.tag != "diedfood" && other.transform.tag != "Border"&& other.transform.tag != "Portal"&&other.transform.tag != "Player")
		{
		//	Debug.Log(" other Gameobject name	"+other.gameObject.name+" tag "+ other.transform.tag+" my name "+ this.gameObject.name);
			
			if (other.gameObject.GetComponent<SnakeBody>()!=null && other.gameObject.GetComponent<SnakeBody>().head.transform == this.gameObject.transform)
			{ 
			//	 Debug.Log("return");
				return;
			}
			 
		} 
		
	 
		
//		Debug.LogError("TriggerEnter"+ other.gameObject.transform.name);
		
		    
			 
				if (other.transform.tag != "orb" && other.transform.tag != "diedfood" && !killed)
				{
					if (!snakeMovement.isAI)
					{
						if (snakeMovement.bodyParts.Contains(other.transform))
						{
							return;
							
						}
						
						if (collidedwithenemy && other.transform.tag != "Border")
						{
							return;
						}

						if (other.transform.gameObject.GetComponent<SnakeMovement>()!=null)
						{
							Debug.Log("hit  by  head" + transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore +" other player score  "+other.transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore);
							if (snakeMovement.localPlayerScore>other.transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore)
							{
								var headcolliders = other.transform.gameObject.GetComponents<Collider>();
								foreach (var item in headcolliders)
								{
									item.enabled = false;
								}
								for (int i = 0; i < other.transform.gameObject.GetComponent<SnakeMovement>().bodyParts.Count; i++)
								{
									var colliders = other.transform.gameObject.GetComponent<SnakeMovement>().bodyParts[i].gameObject.GetComponents<Collider>();
									foreach (var item in colliders)
									{
										item.enabled = false;
									}
									
								}

								 
								
								
								
								return;
							}
							else if(transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore==other.transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore)
							{
								Debug.Log("same length");
 return;
								//do something
							}
						}
						
//						Debug.Log("collision tag "+ other.transform.parent.name+" tag "+ this.transform.name);
						
						Debug.LogError("CALLED enemy");
						
						collidedwithenemy = true;
						if (	other.transform.tag == "Portal")
						{
							
							GameManager.instance.CheckWIn();
						}
 
						
						var killername =  other.gameObject.name;;
						
						if (other.gameObject.GetComponent<SnakeBody>())//get killer name and ai will eat my food after i die
						{
							killername = other.gameObject.GetComponent<SnakeBody>().head.transform.name;
							other.gameObject.GetComponent<SnakeBody>().head.GetComponent<SnakeMovement>().currentDiedFoodTarget = this.gameObject.transform.position;
							other.gameObject.GetComponent<SnakeBody>().head.GetComponent<SnakeMovement>().eatDiedFood =true;
						}
						 
					 
						killed = true;
						
						KillSnake(killername, false);

						UI_Manager.instance.StartCoroutine(UI_Manager.instance.WaitForGivenTime(3f));//Uncomment for show screen

						
						

					}
					else if (snakeMovement.isAI)
					{

						if (killed)
						{
							return;
						}
						
						if (other.transform.gameObject.GetComponent<SnakeMovement>()!=null)
						{
							
							Debug.Log("hit ai by ai head");

							if (transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore>other.transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore)
							{
								
								for (int i = 0; i < other.transform.gameObject.GetComponent<SnakeMovement>().bodyParts.Count; i++)
								{
									var colliders = other.transform.gameObject.GetComponent<SnakeMovement>().bodyParts[i].gameObject.GetComponents<Collider>();
									foreach (var item in colliders)
									{
										item.enabled = false;
									}
								}

								
								
								return;
							}
							else if(transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore==other.transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore)
							{
								Debug.Log("same length");
								//do something
							}
							 
						}
						
//						Debug.Log("collision tag"+ other.transform.parent.name+" tag  "+ this.transform.name);
   
				    	
					   Debug.Log("ismoving locally "+this.gameObject.GetComponent<SnakeMovement>().name  + " killer name "+other.gameObject.name);
						var killername =other.gameObject.name;
						if (other.gameObject.GetComponent<SnakeBody>())
						{
							killername = other.gameObject.GetComponent<SnakeBody>().head.transform.name;
						}

						if (!killed)
						{


							int index = snakeMovement.localAiIndex;
							AIManager.instance.AICounter--;
							AIManager.instance.aiSnakes.Remove(this.gameObject);
							AIManager.instance.InstantiateAISnakes(UnityEngine.Random.Range(1500, 4000), index);
							//AIManager.instance.InstantiateAISnakes(UnityEngine.Random.Range(8, 70), index);


							Debug.Log(index);
							killed = true;
							KillSnake(killername, true);

						}

					}
				}
			 

	}
		 
	void OnTriggerEnter2D(Collider2D other)
    {
		if (!snakeMovement.isvisible || Vector2.Distance(transform.position, Camera.main.gameObject.transform.position) > 30)
		{
			if (other.gameObject.tag == "Portal" || other.gameObject.tag == "Border")
			{


				if (!killed)
				{


					killed = true;
					AIManager.instance.AICounter--;

					int index = snakeMovement.localAiIndex;
					AIManager.instance.aiSnakes.Remove(this.gameObject);

					Debug.Log(index);

					AIManager.instance.InstantiateAISnakes(UnityEngine.Random.Range(1000, 3000), index);
					//AIManager.instance.InstantiateAISnakes(UnityEngine.Random.Range(8, 70), index);
					Destroy(this.gameObject);
				}
			}



			return;
		}

		/*if ((other.gameObject.tag == "orb" || other.gameObject.tag == "diedfood") && snakeMovement.bodyParts.Count >= snakeDefaultLength   &&  snakeMovement.isvisible )
		{
			/*if (Vector2.Distance(transform.position,other.transform.position) < 0.8f)
			{#1#
			
			Debug.Log("collided");
				 other.gameObject.GetComponent<OrbGrowthScript>().OrbTriggered(this.gameObject.transform);
			//OnCollidedWithOrb(other.gameObject);
			 
				//return;
			//}
		}*/
		//Debug.Log("collided food" + other.gameObject.tag);
		if (other.transform.tag == "AI" || other.transform.tag == "RaycastTarget")
		{
			return;
		}

		if (other.transform.tag != "orb" && other.transform.tag != "diedfood" && other.transform.tag != "Border" && other.transform.tag != "Portal" && other.transform.tag != "Player")
		{
			//	Debug.Log(" other Gameobject name	"+other.gameObject.name+" tag "+ other.transform.tag+" my name "+ this.gameObject.name);

			if (other.gameObject.GetComponent<SnakeBody>() != null && other.gameObject.GetComponent<SnakeBody>().head.transform == this.gameObject.transform)
			{
				//	 Debug.Log("return");
				return;
			}

		}



		//		Debug.LogError("TriggerEnter"+ other.gameObject.transform.name);



		if (other.transform.tag != "orb" && other.transform.tag != "diedfood" && !killed)
		{
			if (!snakeMovement.isAI)
			{
				if (snakeMovement.bodyParts.Contains(other.transform))
				{
					return;

				}

				if (collidedwithenemy && other.transform.tag != "Border")
				{
					return;
				}

				if (other.transform.gameObject.GetComponent<SnakeMovement>() != null)
				{
					Debug.Log("hit  by  head" + transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore + " other player score  " + other.transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore);
					if (snakeMovement.localPlayerScore > other.transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore)
					{
						var headcolliders = other.transform.gameObject.GetComponents<Collider>();
						foreach (var item in headcolliders)
						{
							item.enabled = false;
						}
						for (int i = 0; i < other.transform.gameObject.GetComponent<SnakeMovement>().bodyParts.Count; i++)
						{
							var colliders = other.transform.gameObject.GetComponent<SnakeMovement>().bodyParts[i].gameObject.GetComponents<Collider>();
							foreach (var item in colliders)
							{
								item.enabled = false;
							}

						}





						return;
					}
					else if (transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore == other.transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore)
					{
						Debug.Log("same length");
						return;
						//do something
					}
				}

				//						Debug.Log("collision tag "+ other.transform.parent.name+" tag "+ this.transform.name);

				Debug.LogError("CALLED enemy");

				collidedwithenemy = true;
				if (other.transform.tag == "Portal")
				{

					GameManager.instance.CheckWIn();
				}


				var killername = other.gameObject.name; ;

				if (other.gameObject.GetComponent<SnakeBody>())//get killer name and ai will eat my food after i die
				{
					killername = other.gameObject.GetComponent<SnakeBody>().head.transform.name;
					other.gameObject.GetComponent<SnakeBody>().head.GetComponent<SnakeMovement>().currentDiedFoodTarget = this.gameObject.transform.position;
					other.gameObject.GetComponent<SnakeBody>().head.GetComponent<SnakeMovement>().eatDiedFood = true;
				}


				killed = true;

				KillSnake(killername, false);

				UI_Manager.instance.StartCoroutine(UI_Manager.instance.WaitForGivenTime(3f));//Uncomment for show screen




			}
			else if (snakeMovement.isAI)
			{

				if (killed)
				{
					return;
				}

				if (other.transform.gameObject.GetComponent<SnakeMovement>() != null)
				{

					Debug.Log("hit ai by ai head");

					if (transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore > other.transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore)
					{

						for (int i = 0; i < other.transform.gameObject.GetComponent<SnakeMovement>().bodyParts.Count; i++)
						{
							var colliders = other.transform.gameObject.GetComponent<SnakeMovement>().bodyParts[i].gameObject.GetComponents<Collider>();
							foreach (var item in colliders)
							{
								item.enabled = false;
							}
						}



						return;
					}
					else if (transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore == other.transform.gameObject.GetComponent<SnakeMovement>().localPlayerScore)
					{
						Debug.Log("same length");
						//do something
					}

				}

				//						Debug.Log("collision tag"+ other.transform.parent.name+" tag  "+ this.transform.name);


				Debug.Log("ismoving locally " + this.gameObject.GetComponent<SnakeMovement>().name + " killer name " + other.gameObject.name);
				var killername = other.gameObject.name;
				if (other.gameObject.GetComponent<SnakeBody>())
				{
					killername = other.gameObject.GetComponent<SnakeBody>().head.transform.name;
				}

				if (!killed)
				{


					int index = snakeMovement.localAiIndex;
					AIManager.instance.AICounter--;
					AIManager.instance.aiSnakes.Remove(this.gameObject);
					AIManager.instance.InstantiateAISnakes(UnityEngine.Random.Range(1500, 4000), index);
					//AIManager.instance.InstantiateAISnakes(UnityEngine.Random.Range(8, 70), index);


					Debug.Log(index);
					killed = true;
					KillSnake(killername, true);

				}

			}
		}


	}

	public void KillSnake(string killername,bool islocally)
	{
 
			StartCoroutine(KillBodies(gameObject.name,killername,islocally));
		 
	}

	 

 

	public GameObject orbPrefabFromRes;
 
	IEnumerator KillBodies(string killed,string killer,bool locally){


		Debug.Log("killer name "+killer);
		yield return null;
		UI_Manager.instance.OnDeleteFromLeaderBoard(killed);
		
		Transform wantedPlayer = GameObject.Find(killed.ToString()).transform;
		SnakeMovement sM = wantedPlayer.GetComponent<SnakeMovement>();





		int m = 0;
		SendMessage("Die", SendMessageOptions.DontRequireReceiver);
			if (GameManager.instance.snakeMovementScript && (Vector2.Distance(transform.position, GameManager.instance.snakeMovementScript.gameObject.transform.position) < 12))
			{

				var count =  sM.localPlayerScore;
				if (sM.isAI)
				{
					count =  sM.bodyParts.Count;
				}


				for (int i = 0; i < count; i++)
				{

					//if (i % 2 == 0) //kill 4 per bodies
					{
//					Debug.Log("spawn orb");

						var losevalue = 1;
						//wantedPlayer.GetComponent<SnakeMovement>().SpawnOrbManager(1, sM.bodyParts[Random.Range(1,sM.bodyParts.Count-1)].gameObject.transform.position+ new Vector3(Random.Range(0f,0.5f),Random.Range(0f,0.5f),0f), true, losevalue);
						wantedPlayer.GetComponent<SnakeMovement>().SpawnOrbManager(1, sM.bodyParts[i].gameObject.transform.position+ new Vector3(Random.Range(0f,0.5f),Random.Range(0f,0.5f),0f), true, losevalue);
					UI_Manager.instance.DespawnBodyParts(sM.bodyParts[i].gameObject);
					//if (++m % 1000 == 0) yield return null;
						/*//var orb=PhotonNetwork.Instantiate(orbPrefabFromRes.name, sM.bodyParts[i].gameObject.transform.position, Quaternion.identity, 0);
						
						wantedPlayer.GetComponent<SnakeMovement>().SpawnOrbManager(1,sM.bodyParts[i].gameObject.transform.position,true,10);*/
						
						
						//var orb=PhotonNetwork.Instantiate(orbPrefabFromRes.name, sM.bodyParts[i].gameObject.transform.position, Quaternion.identity, 0);
						
					}
				

				}
				
				if (MyInfo.localPlayerName == killer)
				{
					//Debug.Log("killed by me");

					GameManager.instance.localPlayerKills++;
					UI_Manager.instance.killsText.text = GameManager.instance.localPlayerKills.ToString();
				}
			}

			Destroy(sM.newBodyPartParent.gameObject);
			Destroy(sM.gameObject);
			
			
		 

		 

		//sM.bodyParts.Clear();
		//PhotonNetwork.Destroy(gameObject);
		
		
	
	
	}


	IEnumerator DestroyAfterSeconds(SnakeMovement sM)
	{
		yield return new WaitForSeconds(2f);

		Destroy(sM.gameObject);
		if (sM.newBodyPartParent)
		{
		Destroy(sM.newBodyPartParent.gameObject);
			
		}
	}

	 
	void DeleteOrbForOthers(GameObject orb){
		collisioncounter++;
		snakeMovement.orbEaten++;
		snakeMovement.orbCounter++;
		//GameObject orb = GameObject.Find(go);
		if (orb!=null)
		{
			UI_Manager.instance.DespawnOrb(orb);
		//Destroy(GameObject.Find(go).gameObject);
		}

	}
	 
	public  float angle;
	public float time;
	public int counter=0;
	public float angleduraction;
	 
	 
	
	public IEnumerator Rotates(float time)
	{
		yield return new WaitForSeconds(time);
		
		this.GetComponent<SnakeMovement>().hitBorder = true;

	 
		this.GetComponent<SnakeMovement>().AIrotaion= Quaternion.Euler(0,0,transform.rotation.z+angle);
	
	 
		//	Debug.Log("snake rotaion in positive " +angle + "local rotation is" + transform.rotation);

			angle+=angleduraction;
			
		 
		var onefourthBodypart = this.GetComponent<SnakeMovement>().bodyParts.Count / 4;

		if (onefourthBodypart<20)
		{
			onefourthBodypart = 20;
		}
		if (counter>=onefourthBodypart)
		{
			this.GetComponent<SnakeMovement>().raycasthitCounter = 0;
			this.GetComponent<SnakeMovement>().rotatingInCircular = true;

		}
		
		if (counter<this.GetComponent<SnakeMovement>().bodyParts.Count)
		{

			counter++;
		StartCoroutine(Rotates(0.2f));
			
		}
		else
		{
			counter = 0;


			this.GetComponent<SnakeMovement>().rotatingInCircular = false;
			this.GetComponent<SnakeMovement>().raycasthitCounter = 0;
		}
		 
	}


	private int ordercount = 999;
	
	public void AddThisSnakeNewBodyPart(string gO)
	{
		Transform wantedPlayer = GameObject.Find(gO.ToString()).transform;
		SnakeMovement snakeComponent = wantedPlayer.GetComponent<SnakeMovement>();
		
	 
		if(snakeComponent.bodyParts.Count == 0)
		{
			Vector3 currentPos = wantedPlayer.position;
			//Transform newBodyPart = Instantiate(bodyObject, currentPos, Quaternion.identity) as Transform;
			Transform newBodyPart = UI_Manager.instance.GetBodyPart(currentPos).transform;
			snakeComponent.newBodyPartParent = Instantiate(parentBody, currentPos, Quaternion.identity) as Transform;
			newBodyPart.SetParent(snakeComponent.newBodyPartParent);
			snakeComponent.newBodyPartParent.name = gO + "parent";
			/*snakeComponent.orbCounter++;
			snakeComponent.orbEaten++;*/
			
			
			
			snakeComponent.bodyParts.Add(snakeComponent.transform);
			//newBodyPart.GetComponent<SnakeBody>().head = wantedPlayer;
//			Debug.Log("head ");
			//newBodyPart.GetComponent<SnakeBody>().head = wantedPlayer;
			newBodyPart.GetComponent<SpriteRenderer>().sortingOrder = ordercount;
			ordercount--;
			 
			GameManager.instance.playerParent.Add(snakeComponent.newBodyPartParent.gameObject);

		 
				if (snakeComponent.isAI)
				{
					newBodyPart.gameObject.tag = "Enemy";
				}
		 
			 
			snakeComponent.bodyParts.Add(newBodyPart);
			SnakeBody component = newBodyPart.GetComponent<SnakeBody>();
			component.InitializePiece(snakeComponent.bodyParts.Count - 1, snakeComponent);
			
		}
		else{
			
			
			Vector3 currentPos =snakeComponent.bodyParts[snakeComponent.bodyParts.Count-1].position;
			//Transform newBodyPart = Instantiate (bodyObject, currentPos, Quaternion.identity) as Transform;
			Transform newBodyPart = UI_Manager.instance.GetBodyPart(currentPos).transform;
			newBodyPart.SetParent(snakeComponent.newBodyPartParent);
			
		//	newBodyPart.GetComponent<SnakeBody>().head = wantedPlayer;
			
			newBodyPart.GetComponent<SpriteRenderer>().sortingOrder = ordercount;
			/*snakeComponent.orbCounter++;
			snakeComponent.orbEaten++;*/
		 
			//Debug.Log("bodypart order is "+ ordercount);

			ordercount--;
			 
				if (snakeComponent.isAI)
				{
					newBodyPart.gameObject.tag = "Enemy";
				}
				//wantedPlayer.GetComponent<SnakeMovement>().bodyParts.Add(newBodyPart);
			 
			snakeComponent.bodyParts.Add(newBodyPart);
			SnakeBody component = newBodyPart.GetComponent<SnakeBody>();
			component?.InitializePiece(snakeComponent.bodyParts.Count - 1, snakeComponent);
			
		}
		

	}

    private void OnDisable()
    {
		StopAllCoroutines();
    }
}

 