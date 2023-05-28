using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;

using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SnakeMovement : MonoBehaviour {

	public List<Transform> bodyParts = new List<Transform>();



	public Transform newBodyPartParent;
	public float velocity;
	private Vector3 lastFrame;

	public GameObject username;
	public GameObject arrowmode;
	public GameObject miniMapSnakePointer;

	public Transform arrowInitialPositon;
	public bool isvisible;

	[Header("eyeBalls")]
	public GameObject eyeBallsParent;

	[Header("Ai Check")]
	public bool isAI;
	public bool isAttackerAI;
	public int localAiIndex;

	public int AIMaxScore = 0;

	[Header("Ai eat died food")]
	public bool eatDiedFood = false;
	public Vector3 currentDiedFoodTarget;
	public AI aimodule;


	public float timeToVisible;

	public int snakeSkinIndex = 0;
	public int snakeScore = 0;

	public int maxFood = 50;


	public float localPlayerScore = 0;
	public int foodCounter = 0;

	bool initialized;

	private void RotateTowardsCenter()
	{
		Quaternion b = Quaternion.LookRotation(Vector3.zero - base.transform.position);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, this.rotatingSpeed * Time.deltaTime);
	}
	private void IaRotate()
	{
		if (aimodule.direction != Vector3.zero)
		{
			Quaternion b = Quaternion.LookRotation(aimodule.direction);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, this.rotatingSpeed * Time.deltaTime * 2f);
		}
	}

	private IEnumerator Start()
	{
		//InvokeRepeating("SpawnOrbManager",2f,3f);
		headcolliders = gameObject.GetComponents<Collider>();

		//code for ai.

		minimappointerScaleratio = 15;


		if (isAI)
		{
			this.gameObject.GetComponent<CollisionNetworkScript>().AddBodyPartsOnStart(snakeLength);
			//this.gameObject.GetComponent<CollisionNetworkScript>().AddBodyPartsOnStart(Random.Range(5000,10000));

			aimodule.StartRotating();//asdf
			timeToVisible = 1f;
			//StartCoroutine(SetAfterFewSeconds());

			//transform.rotation = AIrotaion;
			InvokeSetAfterFewSeconds(timeToVisible);



			//InvokeRepeating("AIRotation", 5f, 3f);

			//InvokeRepeating("IsRunningForAI",10f,50f); //running ai 
			InvokeRepeating("AIAddToLeaderBard", 10f, 15f); //rotate ai 

		}
		else
		{

			GameManager.instance.snakeMovementScript = this;
			GameManager.instance.mySnakeCollisonNetworkScript = this.GetComponent<CollisionNetworkScript>();
			this.gameObject.GetComponent<CollisionNetworkScript>().AddBodyPartsOnStart(10000);//anshal
																							  //this.gameObject.GetComponent<CollisionNetworkScript>().AddBodyPartsOnStart(100);
			isInSync = true;
			UI_Manager.instance.myScore += 10000;
			UI_Manager.instance.myscoreText.text = UI_Manager.instance.myScore.ToString();
			GameManager.instance.AIManager.gameObject.SetActive(true);


			EnableForJoyStick();


			for (int i = 0; i < 5; i++)
			{

				var x = 10;
				//					Debug.Log(" max percentage "+ x);

				if (x == 0)
				{
					continue;

				}
				int maxindexfood = maxFood * x;

				int m = 0;
				//Debug.Log("current maxfood is "+ maxindexfood);
				for (int j = 0; j < maxindexfood; j++)
				{
					//					       Debug.Log("food name" + API_Manager.instance._root.food_data[i].type + " food quantity" + API_Manager.instance._root.food_data[i].percentage);
					Vector3 randomNewOrbPosition = new Vector3(Random.Range(-170, 170), Random.Range(-170, 170), 0);
					SpawnOrbManager(i, randomNewOrbPosition, false, 0);
					if (++m % 5 == 0) yield return null;
					//SpawnOrbManager(2, randomNewOrbPosition);
					foodCounter++;
				}
			}

			localPlayerScore = 8;
			timeToVisible = 0.1f;//play with ai
								 //StartCoroutine(SetAfterFewSeconds());
			InvokeSetAfterFewSeconds(timeToVisible);



		}
		initialized = true;
	}





	void IsRunningForAI()
	{

		if (running)
		{
			running = false;
		}

	}
	void AIAddToLeaderBard()//update ai leaderboard
	{
		if (!isInSync)
		{

			if (localPlayerScore > AIMaxScore)
			{
				return;
			}
			///            Debug.Log("add to local player"+ localPlayerScore);
			/*var random = Random.Range(0, 5);
			localPlayerScore += API_Manager.instance._root.food_data[random].value;*/
			var random = Random.Range(5, 50);
			localPlayerScore += random;

			UI_Manager.instance.UpdateLeaderBoard(localPlayerScore, this.GetComponent<SnakeMovement>().gameObject.name);

			if (bodyParts.Count < 301) //commented
			{


				this.GetComponent<CollisionNetworkScript>().AddBodyPartOfSnake(1, gameObject.transform.name, 1f, true);
			}
		}

	}


	public void InvokeSetAfterFewSeconds(float timeToVisible)
	{

		Invoke("SetAfterFewSeconds", timeToVisible);

	}


	public void SetSkin(int index)
	{
		snakeSkinIndex = index;
		gameObject.GetComponent<SpriteRenderer>().sprite = UI_Manager.instance.snakeHeadSkin[snakeSkinIndex];
		for (int j = 0; j < bodyParts.Count; j++)
		{
			bodyParts[j].GetComponent<SpriteRenderer>().sprite = UI_Manager.instance.snakeHeadSkin[snakeSkinIndex];
		}
	}


	private void EnableForJoyStick()
	{
		if (UI_Manager.instance.gameType == UI_Manager.GameMode.JoyStick)
		{


			var tempColor = UI_Manager.instance.joyStickHandler.GetComponent<Image>().color;
			tempColor.a = 1f;
			UI_Manager.instance.joyStickHandler.GetComponent<Image>().color = tempColor;
			var tempColor2 = UI_Manager.instance.joyStickBackGround.GetComponent<Image>().color;
			tempColor2.a = 1f;
			UI_Manager.instance.joyStickBackGround.GetComponent<Image>().color = tempColor2;


			UI_Manager.instance.joyStick.gameObject.SetActive(true);

		}
	}








	public void StopCoroutine()
	{

		//		Debug.LogError("stop ");
		CancelInvoke("SetAfterFewSeconds");
		//	StopCoroutine("SetAfterFewSeconds");
	}

	public void SetAfterFewSeconds()
	{




		//yield return new WaitForSeconds(timeToVisible);
		for (int i = 0; i < newBodyPartParent.childCount; i++)
		{
			newBodyPartParent.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
			//newBodyPartParent.GetChild(i).GetComponent<SphereCollider>().enabled = true;
			newBodyPartParent.GetChild(i).GetComponent<CircleCollider2D>().enabled = true;
		}
		//gameObject.GetComponent<SphereCollider>().enabled = true;
		gameObject.GetComponent<CircleCollider2D>().enabled = true;
		this.gameObject.GetComponent<SpriteRenderer>().enabled = true;

		eyeBallsParent.SetActive(true);

		//this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

		isvisible = true;

		//		Debug.LogError("isvisible==" +this.GetComponent<SnakeMovement>().isvisible+" name= "+ this.gameObject.name);

		if (!isAI)
		{
			miniMapSnakePointer.gameObject.SetActive(true);
			miniMapSnakePointer.GetComponent<SpriteRenderer>().color = Color.red;

			StartCoroutine(NetworkManagerSript.instance.WaitForGivenTime(0.5f));

		}
		else
		{
			miniMapSnakePointer.gameObject.SetActive(true);
		}





	}


	[Header("SyncValues")]
	public bool isInSync = false;
	public bool isColliderSync;
	public int syncDistance;
	public bool rotatingInCircular;


	private Vector3 movementVelocity;
	[Range(0.0f, 1.0f)]
	public float overTime = 0.05f;
	public float overTimeHead = 0.05f;

	private int i;
	private Collider[] headcolliders;

	bool dead = false;
	public void Die()
	{
		dead = true;
	}
	private void FollowBodyParts()
	{



		for (i = 0; i < bodyParts.Count; i++)
		{
			//Debug.Log("i"+ i + "name:"+ transform.gameObject.name);
			if (i == 0)
			{
				//	Debug.Log("i"+ i);
				bodyParts[i].position = Vector3.SmoothDamp(bodyParts[i].position, transform.position,
					ref movementVelocity, overTimeHead);
				bodyParts[i].rotation = this.transform.rotation;
				//transform.LookAt(head.transform.position);//anshal	
			}
			else
			{
				//	Debug.Log("i"+ i);

				bodyParts[i].position = Vector3.SmoothDamp(bodyParts[i - 1].position, bodyParts[i].position,
					ref movementVelocity, overTime);
				bodyParts[i].rotation = this.transform.rotation;
				//transform.LookAt(head.transform.position);//anshal
			}

		}






	}

	public float speed = 1f;
	public float aiSpeed = 0.2f;
	public float delayTime = 1f; // The delay time in seconds
	private float timer = 0.0f;
	void FixedUpdate() {

		if (!initialized && !isAI || dead) return;
		if (newBodyPartParent)
		{

			if ((Vector2.Distance(transform.position, Camera.main.gameObject.transform.position) < 25 ||
			     isAttackerAI)  && !isInSync)
			{
				MoveForward();
				//Debug.Log("follow ny head"+ this.transform.gameObject);
				FollowBodyParts();
			}

		





			if (isInSync)
			{




				MoveForward();
				Scaling();

				ApplyingStuffForBody();

				
					FollowBodyParts(); // Call your function here
					if (!isAI)
					{
						MouseRotationSnake();
						CameraFollow();
						Running();
					}
					else
					{



						if (!rotatingInCircular)
						{
							//		Raycast();

						}



						AiFoodRelease();

						//	transform.rotation = Quaternion.Slerp(transform.rotation, AIrotaion, 10 / 360f);
						transform.rotation = Quaternion.Lerp(transform.rotation, AIrotaion, 2f * Time.deltaTime * 2f);





					}

				

			}

			else
			{

				if ( isAI)
				{
					if (isAttackerAI)
					{

						MoveForward();
					}
				}
				

			}



			
				if (Vector2.Distance(transform.position, Camera.main.gameObject.transform.position) < syncDistance)
				{

					//	Debug.Log("is not in sync= " + Vector2.Distance(transform.position, Camera.main.gameObject.transform.position));
					isInSync = true;


				}
				else
				{
					isInSync = false;

				}



		 

			if (isInSync)
			{






				//	ColorMySnake();


				if (running && localPlayerScore > snakeDefaultLength)
				{
					/*if (!runningCalled)
					{
						runningCalled = true;*/
					MakeOurSnakeGlow(true);
					//}
				}
				else
				{
					/*if (runningCalled)
					{
						runningCalled = false;*/

					MakeOurSnakeGlow(false);
					//	}
				}
			}
			else
			{
				hitBorder = false;

				/*
				var colliders = GetComponents<Collider>();
	
				foreach (var item in colliders)
				{
					item.enabled = false;
				}
				*/


			}
			/*else if (GetComponent<OnlineSyncScript>().isMovingLocally)
			{
			MoveForward();	
			}*/
		}

	}
	public float referenceScale;
	public float rotatingSpeed = 1.7f;
 
	 
	private Collider[] colliders;
	private void EnableBodyPartColliders(int j)
	{
		if (bodyParts[j].GetComponent<SnakeBody>().isInSyncwithSnake)
		{
		
			isColliderSync = true;




			if (this.GetComponent<SnakeMovement>().bodyParts.Count >= snakeDefaultLength && this.GetComponent<SnakeMovement>().isvisible)
			{
				//Debug.Log("enable bodnewBodyParty part" + this.gameObject.name);
			 


					//Debug.Log("is not in sync");
					  colliders = bodyParts[j].gameObject.GetComponents<Collider>();
					
					  for (int k = 0; k < colliders.Length; k++)
					  {
						  colliders[k].enabled = false;
					  }

					bodyParts[j].GetComponent<SnakeBody>().isInSyncwithSnake = false;

				 
			}
		}
	}
	public int bodyPartColliderOnStart = 1;
	public void BodyPartColliders(int j)
	{

		if (j<bodyPartColliderOnStart || j%2==0)
		{
			return;
		}

			if (!bodyParts[j].GetComponent<SnakeBody>().isInSyncwithSnake)
			{
 
				 colliders = bodyParts[j].gameObject.GetComponents<Collider>(); // GC.alloc

				 for (int k = 0; k < colliders.Length; k++)
				 {
					 colliders[k].enabled = true;
				 }
			 


				bodyParts[j].GetComponent<SnakeBody>().isInSyncwithSnake = true;
			}

		 
		 
		/*else
		{
			
			this.GetComponent<SnakeMovement>().bodyParts[i].GetComponent<SpriteRenderer>().enabled = false;
			
		 
			
			
			
			Debug.Log("is not in sync");
			var colliders = bodyParts[i].gameObject.GetComponents<Collider>();

			foreach (var item in colliders)
			{
				item.enabled = false;
			}
			var headcolliders =  gameObject.GetComponents<Collider>();

			foreach (var item in headcolliders)
			{
				item.enabled = false;
			}
			this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}*/
	}

	public Sprite purple, orange;
	
	void ColorMySnake(){
		for(int i = 0; i < bodyParts.Count; i++){
			if(i % 2 == 0){
				bodyParts[i].GetComponent<SpriteRenderer>().sprite = purple;
			}
			else{
				
		 				bodyParts[i].GetComponent<SpriteRenderer>().sprite = orange;
			}
		}
	}

	public float spawnOrbEveryXSeconds = 5;
	public GameObject orbPrefab;
	public void SpawnOrbManager(int index, Vector3 randomOrbPosition,bool diedFood,float orbValue)
	{
		float radiusSpawn = 5;
		int randomNum = Random.Range(1,100000);
		 
			SpawnOrbFromHead(randomOrbPosition,randomNum,index,diedFood,orbValue);
		 
		
	}
	 
 
	void SpawnOrbFromHead(Vector3 finalPosition_, int randomNum_,int i,bool diedfood,float orbvalue){



		//GameObject newOrb = Instantiate(orbPrefab, finalPosition_, Quaternion.identity) as GameObject;
		GameObject newOrb = UI_Manager.instance.GetOrbs(finalPosition_);
		newOrb.transform.name =  this.gameObject.name+"randomnumber"+randomNum_.ToString()+ "value"+ orbvalue;

		newOrb.transform.position = new Vector3(newOrb.transform.position.x, newOrb.transform.position.y, 0);
		//newOrb.transform.SetParent(UI_Manager.instance.orbParent.transform);
		newOrb.transform.SetParent(null);
		
		newOrb.GetComponent<OrbGrowthScript>().myFoodSprite.sprite = UI_Manager.instance.orbSprites[i];
		
		newOrb.GetComponent<OrbGrowthScript>().diedFood = diedfood;


			//if (MyInfo.IsOnline())
			{

				if (diedfood)
				{
					
					Debug.Log("spawn from head"+ orbvalue);
					newOrb.GetComponent<OrbGrowthScript>().value = orbvalue;
					/*newOrb.GetComponent<OrbGrowthScript>().myFoodImageName = API_Manager.instance._root.food_data[0].type;
					*/


					newOrb.GetComponent<Animator>().enabled = false;
					//	newOrb.transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
					newOrb.GetComponent<OrbGrowthScript>().tag = "diedfood";


				

				}
				else
				{
					
					newOrb.GetComponent<OrbGrowthScript>().value = 1;
				//	newOrb.GetComponent<OrbGrowthScript>().myFoodImageName = API_Manager.instance._root.food_data[i].type;
				}
			}

			//if (this.gameObject.name==MyInfo.localPlayerName)
		/*{
			newOrb.GetComponent<OrbGrowthScript>().value = API_Manager.instance._root.food_data[i].value;
			newOrb.GetComponent<OrbGrowthScript>().myFoodImageName = API_Manager.instance._root.food_data[i].type;
		}*/
		
	
		
	}

	private Vector3 pointInWorld;
	private Vector3 mousePosition;
	private float radius = 3.0f;
	private Vector3 direction;
	[SerializeField]
	private bool ismovingSnakeOnTouch=false;
	public  int currentTouchIndex=0;
	public  int currenttouchcounter=0;
	private  Vector2 initialjsmovement = Vector2.zero;
	private IEnumerator RotateAfterFewSeconds(float angle)
	{
		yield return new WaitForSeconds(0.1f);
		
		
		Debug.Log("arrow"+ arrowInitialPositon.transform.rotation);
		StopCoroutine(EyeRotateAngle(angle));
		StartCoroutine(EyeRotateAngle(angle));
		
		 transform.rotation = Quaternion.Lerp( transform.rotation , Quaternion.Euler(0f, 0f, angle), 10 / 100f);

		 
		 yield return new WaitForSeconds(0.3f);

		 arrowInitialPositon.rotation=new Quaternion(0,0,0,0);
		 


	}
	public GameObject arrow=null;
	private Vector3 mouseInitialPosition;
	public bool canRotateInTouch=true;
	public IEnumerator EyeRotateAngle(float angle)
	{
		
		float Distance = UI_Manager.instance.distance;
				
			 


		eyeBallsParent.transform.localPosition = new Vector3(UI_Manager.instance.map(Distance, 0f, 1000f, -0.01f, 0.01f),UI_Manager.instance.map(Distance, 0f, 1000f, -0.01f, 0.01f),0);
		yield return new WaitForSeconds(0.7f);
		//canRotateInTouch = false;
		

	}
	Ray ray ;
	RaycastHit hit1;
	
	//joystick variables
	private Vector2 JS_Movement;
	private float Angle;
	
	
	//arrow variable
	private float AngleForArrow;
	private float keepRotateSpeed;
	private Vector3 targetPosition;
	private Vector3 difference;
	private float rotationZ;

	private float Distance;
	private float previousDistance=-1;
	//touch variables
	private Touch touch;
	
	void MouseRotationSnake()
	{
		
		if (UI_Manager.instance. gameType == UI_Manager.GameMode.JoyStick)
		{
			JS_Movement = new Vector2(UI_Manager.instance.joyStick.Horizontal, UI_Manager.instance.joyStick.Vertical);
			  Angle = Mathf.Atan2(JS_Movement.y, JS_Movement.x) * Mathf.Rad2Deg;

			if (JS_Movement != Vector2.zero)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, Angle), 10 / 100f);
				StopCoroutine(EyeRotateAngle(Angle));
				StartCoroutine(EyeRotateAngle(Angle));
			 
				
			}
		}
		else if (UI_Manager.instance. gameType == UI_Manager.GameMode.Arrow)
		{
			 
			
			if (!UI_Manager.instance.IsPointerUp  && Input.touchCount==1 )
			{
				
				return;
			}
		
			if (UI_Manager.instance.arrowAssigned)
			{
				
				if (arrowInitialPositon.transform.GetChild(0).transform.childCount>0)
				{
					for (int j = 0; j <arrowInitialPositon.transform.GetChild(0).transform.childCount; j++)
					{
						Destroy(arrowInitialPositon.transform.GetChild(0).transform.GetChild(j).gameObject);
					}
				}
				
				arrow = Instantiate(arrowmode, arrowInitialPositon);
				arrow.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
				arrow.transform.SetParent(arrowInitialPositon.transform.GetChild(0));
				mouseInitialPosition = Vector3.zero;
 
			    UI_Manager.instance.arrowAssigned = false;
			   
			   
			}
			if (UI_Manager.instance.arrowDestroy)
			{
				arrowInitialPositon.rotation=new Quaternion(0,0,0,0);
				 
				UI_Manager.instance.arrowDestroy = false;
				Destroy(arrow.gameObject); 
			}
			if (Input.GetMouseButton(0))
			{ 
				
				arrowInitialPositon.gameObject.SetActive(true);
				var mouseCurrentPostion = Input.mousePosition;
			 
				UI_Manager.instance.joyStick.gameObject.SetActive(true);
			
				 
				Distance = UI_Manager.instance.distance;
					 
 
				    if (Distance>15 )
				    {
					    if (Distance != previousDistance)
					    {



						    previousDistance = Distance;
						    if (arrow)
						    {
							    arrow.transform.localPosition = new Vector3(UI_Manager.instance.map(Distance, 0f, 250, -1f, 0f), 0, 0);
						    }

						    arrowInitialPositon.gameObject.SetActive(true);
						    JS_Movement = new Vector2(UI_Manager.instance.joyStick.Horizontal, UI_Manager.instance.joyStick.Vertical);

						    AngleForArrow = Mathf.Atan2(JS_Movement.y, JS_Movement.x) * Mathf.Rad2Deg;


						    initialjsmovement = JS_Movement;

						    arrowInitialPositon.transform.rotation = Quaternion.Lerp(arrowInitialPositon.transform.rotation, Quaternion.Euler(0f, 0f, AngleForArrow), 10 / 500f);
					    }
					    StartCoroutine(RotateAfterFewSeconds(AngleForArrow));
					}
					
						
				 
				 
			}
			 
			 
		}
		else if (UI_Manager.instance.gameType == UI_Manager.GameMode.Touch)
		{
			
		 
			if (Input.touchCount > 0)
			{
				
				if (Input.touchCount==1)
				{
					currentTouchIndex = 0;
				}
				  touch = Input.GetTouch(currentTouchIndex);
				 
				 
				if (!UI_Manager.instance.IsPointerUp  && Input.touchCount==1 )
				{

					currenttouchcounter = 0;
					 
					 
					ismovingSnakeOnTouch = false;
					
					return;
				}
				 
				 {
					if (!UI_Manager.instance.IsPointerUp && Input.touchCount>1 && !ismovingSnakeOnTouch)
					{
						touch = Input.GetTouch(1);

						currentTouchIndex = 1;
					 
						ismovingSnakeOnTouch = true;
					}
					else if (UI_Manager.instance.IsPointerUp )
					{
						if (currenttouchcounter>5)
						{
							currentTouchIndex = 0;
							touch = Input.GetTouch(0);
							ismovingSnakeOnTouch = true;
							 
						}

						currenttouchcounter++;

					}
					
					  ray = Camera.main.ScreenPointToRay(touch.position);
					  
					  keepRotateSpeed = 10f;
					if (Physics.Raycast(ray, out hit1) && ismovingSnakeOnTouch )
					{
						 
						   targetPosition = hit1.point;

						  difference = targetPosition - transform.position;
						  rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
						
					 

						transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, rotationZ), 10 / 100f);

 

					}
				}
			}
			else
			{
				currenttouchcounter = 0;
				ismovingSnakeOnTouch = false;
			}
		 
		}			
		 
		
	}



	 	
	void AiFoodRelease()
	{
		if(running == true){

		
		  if (bodyParts.Count > 7)
			{
					
				 
				speed = speedWhileRunnin;
				 
				bodyPartOverTimeFollow = bodyPartFollowTimeRunning;
			 
				StartCoroutine("LoseBodyParts");
			}
		}
		else{
			speed = speedWhileWalking;
			bodyPartOverTimeFollow = bodyPartFollowTimeWalking;
		}
	}
	
	
	
	
	
	[Header("raycast variables")]
	public int range;
	public bool hitBorder;
	 
	public int raycasthitCounter=0;
	private Vector3 dir;
	private int layerMask;
	RaycastHit hit;
	void Raycast()
	{
		// Bit shift the index of the layer (8) to get a bit mask
		  layerMask = 1 << 8;

		// This would cast rays only against colliders in layer 8.
		// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
		layerMask = ~layerMask;
	 
	 
		// Does the ray intersect any objects excluding the player layer
		if (Physics.Raycast (transform.position,transform.TransformDirection(Vector3.right), out hit, range,layerMask))
		{
			Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
			//Debug.Log("Did not Hit "+ transform.gameObject.name+ "hit name"+ hit.transform.tag);
			
			
			if (hit.transform.tag=="Border"/* && !hitBorder*/)
			{
				
				
				running = false;
			}
			if (hit.transform.parent!=newBodyPartParent )
			{
		    
				
				hitBorder = true;
 
				  dir = hit.transform.position - transform.position;
			 
				dir = -dir.normalized;
				
				float Angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

				
				 AIrotaion= Quaternion.Euler(0,0,Angle);
				 
				 
				
				 raycasthitCounter++;
				 if (raycasthitCounter == 6)
				 { 
					 StopCoroutine(HitCounterReset());
					 StartCoroutine(HitCounterReset());
					 running = true;
					 
					 CancelInvoke("IsRunningForAI");
					 Invoke("IsRunningForAI",2f);
				 }
				 
				 if (raycasthitCounter==12)
				 {
					 StopCoroutine(HitCounterReset());
					 StartCoroutine(HitCounterReset());
					 this.GetComponent<CollisionNetworkScript>().angle = Angle;
					 StartCoroutine(this.GetComponent<CollisionNetworkScript>().Rotates(0.2f));
				 }
				
				 
			}
			
		}
		else
		{
			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * range, Color.green);
		 
			hitBorder = false;
		}
	}

	private IEnumerator HitCounterReset()
	{
		yield return new WaitForSeconds(5f);
		raycasthitCounter = 0;
	}
	
	public Quaternion AIrotaion;
	public float AIrotaionz=-1;
	public void AIRotation()
	{

		if (!hitBorder && !isAttackerAI && isInSync)
		{
			
		AIrotaion = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
		}
 
	}
	 

	void MoveForward()
	{
		if (isAI  && isvisible )
		{
			if ( !isInSync )
			{

				if ( GameManager.instance.snakeMovementScript==null )
				{
					return;
				}
				var distance = GameManager.instance.snakeMovementScript.gameObject.transform.position - this.transform.position;

				transform.position += distance * aiSpeed * Time.deltaTime;
				float Angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
//				Debug.Log("angle is " + Angle);
				this.transform.rotation = Quaternion.Euler(0f, 0f, Angle);
				
				
			}
			else if (eatDiedFood)
			{
				var distance = currentDiedFoodTarget - this.transform.position;

				transform.position += distance * 0.19f * Time.deltaTime;
				float Angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
				//Debug.Log("angle is " + Angle);
				AIrotaion = Quaternion.Euler(0f, 0f, Angle);
				eatDiedFood = false;
			}
			else
			{
				transform.position += transform.right * speed * Time.deltaTime;
			}
			 
				
			 
		}
		else if (!isAI)
		{
		 transform.position += transform.right * speed * Time.deltaTime;
			
		}
	 
		
	}


	[Range(0.0f,1.0f)]
	public float cameraFollowTime = 0.5f;
	
	Transform camera ;
	Vector3 cameraVelocity = Vector3.zero;
	void CameraFollow()
	{

		camera = Camera.main.gameObject.transform;
		camera.position = Vector3.SmoothDamp(camera.position, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10), ref cameraVelocity, cameraFollowTime);
	}

	
	
	[Header("Orb Status ")]
	[Space(4)]
	public int orbCounter=0;
	public int orbEaten=0;
	
	private int currentOrb;
	public int[] growOnThisOrb;
	[SerializeField]private Vector3 currentSize =new Vector3(1f,1f,1f);
	[SerializeField] private float bodyPartOverTimeFollow = 0.19f;
	bool SizeUp(int x){
		try{
			if(x == growOnThisOrb[currentOrb]){
				currentOrb ++;
				return false;
			}
			else{
				return false;
			}
		}
		catch(System.Exception e){
			//print ("No more grow from this point(add more rows). + " + e.StackTrace.ToString());
		}

		return false;
	}
 
	public bool running;
	public bool runningCalled;
	[SerializeField]private bool localSnakeRunning;
	public float speedWhileRunnin = 6.5f;
	public float speedWhileWalking = 3.5f;
	public float bodyPartFollowTimeWalking = 0.19f;
	public float bodyPartFollowTimeRunning = 0.1f;
	public int snakeDefaultLength=8;
	public int snakeLength;
	
	
	
	void Running(){


		if(localPlayerScore >snakeDefaultLength)
		{


		 
				if (!UI_Manager.instance.IsPointerUp)
				{
					speed = speedWhileRunnin;
					running = true;
					bodyPartOverTimeFollow = bodyPartFollowTimeRunning;
				}
				if (UI_Manager.instance.IsPointerUp)
				{
					speed = speedWhileWalking;
					running = false;
					bodyPartOverTimeFollow = bodyPartFollowTimeWalking;
				}
			 
		}
		else{
			speed = speedWhileWalking;
			running = false;
			bodyPartOverTimeFollow = bodyPartFollowTimeWalking;
		}

		if(running == true){

			 
				StartCoroutine(nameof(LoseBodyParts));
			 
		}
		else{
			bodyPartOverTimeFollow = bodyPartFollowTimeWalking;
		}
	}
 
	void MySnakeRunning(bool localrunning)
	{
		
//		Debug.Log("local cnale running");
		
		localSnakeRunning = localrunning;


	}
	
	
	IEnumerator LoseBodyParts(){
		yield return new WaitForSeconds(0.5f);

 
			if (!isAI)
			{
				//var losevalue = (float)UI_Manager.instance.myScore / bodyParts.Count;
				var losevalue = 1;
				
				localPlayerScore -= losevalue;
				
				UI_Manager.instance.myScore -= losevalue;
				localPlayerScore = UI_Manager.instance.myScore;
			
				UI_Manager.instance.UpdateLeaderBoard(UI_Manager.instance.myScore ,MyInfo.localPlayerName);
				
				UI_Manager.instance.myscoreText.text = UI_Manager.instance.myScore.ToString();
			UI_Manager.instance.UpdateLeaderBoard(UI_Manager.instance.myScore, MyInfo.localPlayerName);

			int randomName = Random.Range(1,10000);
				
				LoseNetworkBodyPart(randomName,losevalue);
			}
			else
			{
				//var losevalue = (float)localPlayerScore / bodyParts.Count;
				var losevalue = 1;
				
				localPlayerScore -= losevalue;
				
				UI_Manager.instance.UpdateLeaderBoard(this.localPlayerScore , this.gameObject.name);

			    int randomName = Random.Range(1,10000);
		    	
			    LoseNetworkBodyPart(randomName,losevalue);
			}
		
	 
		
		/*
		int lastIndex = bodyParts.Count -1;
		Transform lastBodyPart = bodyParts[lastIndex].transform;

		Instantiate(orbPrefab, lastBodyPart.position, Quaternion.identity);

		bodyParts.RemoveAt(lastIndex);
		Destroy(lastBodyPart.gameObject);

		orbCounter--;
*/
		StopCoroutine("LoseBodyParts");

	}


	public int currentReleasedOrbCounter = 0;
	
	
	 
	void LoseNetworkBodyPart(int _randomName,int value)
	{
		int lastIndex = bodyParts.Count -1;
		Transform lastBodyPart = bodyParts[lastIndex].transform;

		GameObject orbAthTheMoment = Instantiate(orbPrefab, lastBodyPart.position, Quaternion.identity) as GameObject;
		orbAthTheMoment.transform.SetParent(UI_Manager.instance.orbParent.transform);
		orbAthTheMoment.name = _randomName.ToString();
		orbAthTheMoment.transform.position= new Vector3(orbAthTheMoment.transform.position.x,orbAthTheMoment.transform.position.y,0) ;
		orbAthTheMoment.GetComponent<OrbGrowthScript>().value = value;


		if (orbCounter<=1200)
		{
			currentReleasedOrbCounter++;
			if (orbCounter%4==0 && orbCounter>8)
			{
				

				currentReleasedOrbCounter = 0;
				bodyParts.RemoveAt(lastIndex);
				Destroy(lastBodyPart.gameObject);
			}
	
		}
		orbCounter--;
		orbEaten--;
		Debug.Log("lose body parts INSTANTIATE SNAKE"+ this.gameObject.name);
		
		
	
	}
 
	 
	void UpdateMyScore(float _randomName){
		
		Debug.Log("my current score is "+ _randomName);
		localPlayerScore =_randomName;
	}

	void MakeOurSnakeGlow(bool areWeRunning){


		for (int j = 1; j < bodyParts.Count; j++)
		{
			bodyParts[j].GetChild(0).gameObject.SetActive(areWeRunning);
		}
		
	 

	}

	private Vector3 headV;
	void ApplyingStuffForBody(){
		transform.localScale = Vector3.SmoothDamp(transform.localScale, currentSize, ref headV, 0.5f);
		
		Vector3 scaleTmp = miniMapSnakePointer.transform.localScale;
		scaleTmp.x = minimappointerScaleratio/transform.localScale.x;
		scaleTmp.y = minimappointerScaleratio/transform.localScale.y;
		scaleTmp.z = minimappointerScaleratio/transform.localScale.z;
	 
	   
		miniMapSnakePointer.transform.localScale = scaleTmp ;
		
		
		for (int j = 0; j < bodyParts.Count; j++)
		{
			bodyParts[j].localScale = transform.localScale;
			//bodyPart_x.GetComponent<SnakeBody>().overTime = bodyPartOverTimeFollow;
			 overTime = bodyPartOverTimeFollow;
		}
		
		
		
	}

	public List<bool> scalingTrack;
	public float howBigAreWe_questionMark;
	public float followTimeSensitvity;
	public float scaleSenstivity;
	public float scaledivideratio=100f;
	public float minimappointerScaleratio;
	public float howbigarewescale=0.25f;
	 
	void Scaling(){
		scalingTrack = new List<bool>(new bool[growOnThisOrb.Length]);

		howBigAreWe_questionMark = 0;

		for(int i = 0; i < growOnThisOrb.Length; i++){
			if(bodyParts.Count >= growOnThisOrb[i]){
				scalingTrack[i] = !scalingTrack[i];
				howBigAreWe_questionMark +=howbigarewescale;
			}
		}

		currentSize = new Vector3(
			1f + (howBigAreWe_questionMark * scaleSenstivity),
			1f + (howBigAreWe_questionMark * scaleSenstivity),
			1f+ (howBigAreWe_questionMark * scaleSenstivity)
			);
			bodyPartFollowTimeWalking = (howBigAreWe_questionMark /scaledivideratio) + followTimeSensitvity;
			if (bodyParts.Count>85)
			{
				followTimeSensitvity = 0.012f;

			}
			else if(bodyParts.Count>150)
			{
				followTimeSensitvity = 0.011f;
			}
			else
			{
				followTimeSensitvity = 0.014f;
			}
		//	followTimeSensitvity = 0.012f;
	 	bodyPartFollowTimeRunning = bodyPartFollowTimeWalking + 0.007f;
	    
	    
	    
	}
	
	
	
	
}
