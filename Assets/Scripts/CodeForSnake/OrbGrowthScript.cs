    using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OrbGrowthScript : MonoBehaviour {

    
    [Header("public variables")]
    public Vector3 startinSize = Vector3.zero;
    public Vector3 currentSize;
    public bool isTrigger;
    
    [Header("orb details from api")]
    public float value;
    public SpriteRenderer myFoodSprite;
    public string myFoodImageName;
    
    private float rotationSensitvity;
    private float wantedRotation;
    
    private float currentRotation;
    private Transform headTransform;

    public bool diedFood;
    void Awake()
    {
        /*transform.localScale = startinSize;
        rotationSensitvity = Random.Range(-120,120);*/
        
    }
    
    public void OrbTriggered(Transform tempHeadTransform)
    {
        headTransform = tempHeadTransform;
        isTrigger = true;
        
        IsTriggered();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.GetComponent<SnakeMovement>() /*&& snakeMovement.bodyParts.Count >= snakeDefaultLength  */ &&  other.gameObject.GetComponent<CollisionNetworkScript>().snakeMovement.isvisible )
        {
            /*if (Vector2.Distance(transform.position,other.transform.position) < 0.8f)
            {*/
             OrbTriggered(other.gameObject.transform);
            Debug.Log("collided" + value);
            
            //OnCollidedWithOrb(other.gameObject);
			 
            //return;
            //}
        }
    }

    private void IsTriggered()
    {
        /*isTrigger = false;

        headTransform.gameObject.GetComponent<CollisionNetworkScript>().OnCollidedWithOrb(this.gameObject);
        // Destroy((this.gameObject));
               
        Debug.Log("is trigger false" + value);*/
        if (isTrigger && this.gameObject.tag=="orb")
        {
            this.transform.DOMove(headTransform.position, 0.1f).OnComplete(()=>
            {
                isTrigger = false;

                headTransform.gameObject.GetComponent<CollisionNetworkScript>().OnCollidedWithOrb(this.gameObject);
               // Destroy((this.gameObject));
               
              Debug.Log("is trigger false" + value);
            });

        }
        else
        {
            isTrigger = false;

       headTransform.gameObject.GetComponent<CollisionNetworkScript>().OnCollidedWithOrb(this.gameObject);
       // Destroy((this.gameObject));
              
       Debug.Log("is trigger false" + value);
        }
    }

    
}
