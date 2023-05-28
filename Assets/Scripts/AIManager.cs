using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static AIManager instance;
    
    public int AICounter=0;
  
    public int[] previousRandomSnake;
  
    public int previousRandomRunningSnake=0;

    public int maxAISnake;
    private void Awake()
    {
        instance = this;
    }

    public List<GameObject> aiSnakes;

    public GameObject aiSnakeHead;
    
    public int small = 20;

    public int medium = 10;

    
    public int big = 5;

    public List<string> snakeNames = new List<string>();
    void Start()
    {
        StartCoroutine(SpawnPopulation());
      
        InvokeRepeating("ChooseAIAttacker",1f,20f);
   
    }
    IEnumerator SpawnPopulation()
    {
        int m = 0;
        for (int i = 0; i < small; i++)
        {
            InstantiateAISnakes(UnityEngine.Random.Range(1000, 3000),-1);
            //if(++m % 4 == 0)
            //InstantiateAISnakes(UnityEngine.Random.Range(8, 150),-1);
            yield return new WaitForSeconds(5f);
        }

        for (int j = 0; j < medium; j++)
        {
            InstantiateAISnakes(UnityEngine.Random.Range(3000, 5000),-1);
            //InstantiateAISnakes(UnityEngine.Random.Range(200, 600),-1);
            yield return new WaitForSeconds(5f);
        }
        for (int k = 0; k < big; k++)
        {
           //InstantiateAISnakes(UnityEngine.Random.Range(1000, 1500),-1);
           InstantiateAISnakes(UnityEngine.Random.Range(5000, 10000),-1);
            yield return new WaitForSeconds(5f);
            // InstantiateAISnakes(100);
        }
    }
    public void InstantiateAISnakes(int snakeSize,int index)
    {
        if (AICounter < maxAISnake &&GameManager.instance.snakeMovementScript)
        {
           
             
            Vector3 distance = GameManager.instance.snakeMovementScript.gameObject.transform.position - Camera.main.transform.position;


          
               var templocalaisnake = Instantiate(aiSnakeHead.transform, new Vector3( (GameManager.instance.snakeMovementScript.gameObject.transform.localPosition.x+30)+Random.Range(-100,100),(GameManager.instance.snakeMovementScript.gameObject.transform.localPosition.y+30)+Random.Range(-100,100), 0), Quaternion.identity);
          
            templocalaisnake.GetComponent<SnakeMovement>().isAI = true;
           

            if (AICounter==0)
            {

                templocalaisnake.GetComponent<SnakeMovement>().isAttackerAI = true;
            }

            var snakelength =  snakeSize;
            
           templocalaisnake.GetComponent<SnakeMovement>().snakeLength =snakelength;
           templocalaisnake.GetComponent<SnakeMovement>().aimodule.isAi=true;
             distance = GameManager.instance.snakeMovementScript.gameObject.transform.position - templocalaisnake.transform.position;
           
				
				
           float Angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
//           Debug.Log("angle is "+ Angle+"snakebody order ");
           templocalaisnake.transform.rotation = Quaternion.Euler(0f, 0f, Angle);

         
           //templocalaisnake.GetComponent<CollisionNetworkScript>().snakeDefaultLength =snakelength ;
           templocalaisnake.GetComponent<SnakeMovement>().SetSkin(Random.Range(0, 6));
           
           if (index != -1)
           {
               Debug.Log("InstantiateAISnakes" + index);
               templocalaisnake.GetComponent<SnakeMovement>().localAiIndex = index;
               templocalaisnake.gameObject.name = snakeNames[index] + "_"+Random.Range(0,999999);
           }
           else
           {
               templocalaisnake.GetComponent<SnakeMovement>().localAiIndex = AICounter;
            templocalaisnake.gameObject.name = snakeNames[AICounter] + "_"+Random.Range(0,999999);
           }
           
            aiSnakes.Add(templocalaisnake.gameObject);
            templocalaisnake.GetComponent<SnakeMovement>().localPlayerScore = snakelength;
           
            UI_Manager.instance.AddLeaderBoard(templocalaisnake.gameObject.name ,snakelength);
            AICounter++;
        }
        else
        {
            /*if (index != -1)
            {
                Debug.Log("InstantiateAISnakes" + index);
                templocalaisnake.GetComponent<SnakeMovement>().localAiIndex = index;
                templocalaisnake.gameObject.name = snakeNames[index] + "_"+Random.Range(0,999999);
            }
            else
            {
                templocalaisnake.GetComponent<SnakeMovement>().localAiIndex = AICounter;
                templocalaisnake.gameObject.name = snakeNames[AICounter] + "_"+Random.Range(0,999999);
            }*/
            Debug.Log("Local");
            UI_Manager.instance.AddLeaderBoard( snakeNames[AICounter] + "_"+Random.Range(0,999999),snakeSize);
        }
         
    }

    
    
    public void ChooseAIAttacker()
    {
        if (GameManager.instance.snakeMovementScript==null )
        {
            return;
        }

        /*if (Vector2.Distance( aiSnakes[previousRandomSnake].GetComponent<SnakeMovement>().transform.position, Camera.main.gameObject.transform.position) <15)
        {
            return;
        }
        else
        {
        }*/
            aiSnakes[previousRandomSnake[0]].GetComponent<SnakeMovement>().isAttackerAI = false;
            aiSnakes[previousRandomSnake[1]].GetComponent<SnakeMovement>().isAttackerAI = false;
            aiSnakes[previousRandomSnake[2]].GetComponent<SnakeMovement>().isAttackerAI = false;
        var randomsnake = Random.Range(0, aiSnakes.Count);
        previousRandomSnake[0] = randomsnake;
        aiSnakes[randomsnake].GetComponent<SnakeMovement>().isAttackerAI = true;

        randomsnake = Random.Range(0, aiSnakes.Count);
        previousRandomSnake[1] = randomsnake; 
        aiSnakes[randomsnake].GetComponent<SnakeMovement>().isAttackerAI = true;

        randomsnake = Random.Range(0, aiSnakes.Count);
        previousRandomSnake[2] = randomsnake;
        
        
        aiSnakes[randomsnake].GetComponent<SnakeMovement>().isAttackerAI = true;
       


    } 
    public void ChooseAIRunnning()
    {
        if (GameManager.instance.snakeMovementScript==null)
        {
            return;
        }

         
        aiSnakes[previousRandomRunningSnake].GetComponent<SnakeMovement>().running = false;
        var randomsnake = Random.Range(0, aiSnakes.Count);
        previousRandomRunningSnake = randomsnake;

        if (aiSnakes[randomsnake].GetComponent<SnakeMovement>().bodyParts.Count >7)
        {
            aiSnakes[randomsnake].GetComponent<SnakeMovement>().running = true;

        }

    }



    public void CancelInvoke()
    {
         
        CancelInvoke("ChooseAIAttacker");
        CancelInvoke("ChooseAIRunnning");
    }
    
}
