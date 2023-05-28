using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SnakeBody : MonoBehaviour {

	[SerializeField]private int myOrder;
	public SnakeMovement head;

	public bool isInSyncwithSnake;

	public SnakeMovement snakeParameters;
	
	public Transform reference;
	
	public float pieceDistanceOffset = 1f;

	private float pieceYDistanceOffset = 0.01f;

	public Collider2D collider;
	private Vector3 movementVelocity;
	[Range(0.0f,1.0f)]
	public float overTime = 0.5f;

	private  void SetBodyPartSkin()
	{
		this.gameObject.GetComponent<SpriteRenderer>().sprite = UI_Manager.instance.snakeBodySkin[head.snakeSkinIndex];
		this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = UI_Manager.instance.snakeGlowSkin[head .snakeSkinIndex];

	}
	
	public void InitializePiece(int index, SnakeMovement parameters)
	{
		myOrder = index;
		snakeParameters = parameters;
		head = parameters;
		 
//		Debug.Log("Initializing"+ index + parameters.transform.name);
		reference = snakeParameters.bodyParts[index - 1];
		Transform transform = base.transform;
		Vector3 position = reference.transform.position;
		Vector3 forward = reference.transform.forward;
		float num = pieceDistanceOffset;
		Vector3 localScale = snakeParameters.transform.localScale;
		transform.position = position - forward * (num * localScale.x) - Vector3.up * pieceYDistanceOffset;
		
		 collider.enabled =true;
		
		SetBodyPartSkin();
	}


 

 
	 
}
