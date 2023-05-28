using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{
	public Vector3 direction;

	public bool sprint;
	public bool isAi;
	public SnakeMovement head;
	public float snakeParameters=100f;
	
	public void StartRotating()
	{
		 
		StartCoroutine(changeDirectionRandomlyForRoaming());
		StartCoroutine(sprintRandomly());
	 
	}

	private IEnumerator sprintRandomly()
	{
		float waitTime = 6f;
		while (true)
		{
			
			if (UnityEngine.Random.Range(0, 8) == 0 && head.bodyParts.Count>10)
			{

			 
					
				head.running = true;
				 
				 
			}
			else
			{
				head.running = false;
			}
			yield return new WaitForSeconds(waitTime);
		}
	}

	private IEnumerator changeDirectionRandomlyForRoaming()
	{
		float waitTime = 1f;
		Vector3 startingPoint = base.transform.position;
		while (true)
		{
			if (!head.isAttackerAI)
			{


				Vector3 circle = UnityEngine.Random.insideUnitSphere * snakeParameters;
				circle.y = startingPoint.y;
				direction = circle - base.transform.position;

				float Angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				//Debug.Log("angle is "+ Angle+"snakebody order "+ myOrder);
				head.AIrotaion = Quaternion.Euler(0f, 0f, Angle);

			}
			yield return new WaitForSeconds(waitTime);
		}
	}
	
	private void OnTriggerStay(Collider obj)
	{
		if (isAi)
		{
			if ((obj.tag != "orb" && obj.tag != "diedfood" && obj.transform.root != base.transform.root && obj.tag !="RaycastTarget"))
			{

				if (head.bodyParts.Contains(obj.transform))
				{
					return;

				}
				Escape(obj);
			}
		}
	}

	 

	private void Escape(Collider obj)
	{
//			Debug.LogError(obj.transform.name);
	 
		var distance = obj.transform.position - this.transform.position;
		distance = -distance.normalized;
				
				
		float Angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
		//Debug.Log("angle is "+ Angle+"snakebody order "+ myOrder);
		head.AIrotaion = Quaternion.Euler(0f, 0f, Angle);
		if (head.isInSync)
		{
		head.isAttackerAI=false;
			
		}
		/*if (UnityEngine.Random.Range(0, 7) == 0)
		{
			sprint = true;
		}
		else
		{
			sprint = false;
		}*/
	}

	private void Chase(Collider obj)
	{
		direction = obj.transform.position - base.transform.position;
		ref Vector3 reference = ref direction;
		Vector3 position = base.transform.position;
		reference.y = position.y;
	}
}
