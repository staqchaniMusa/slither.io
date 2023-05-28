/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public Transform Player;

    private void FixedUpdate()
    {
        if (Player != null)
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -GameManager.instance.CamaraDistance), Player.transform.GetChild(0).position, 2 * Time.deltaTime);

        GetComponent<Camera>().orthographicSize = GameManager.instance.CamaraDistance;
    }

}*/
