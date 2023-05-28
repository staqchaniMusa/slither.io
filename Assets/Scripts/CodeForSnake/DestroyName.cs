using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyMyName",15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void DestroyMyName()
    {
        Destroy(this.gameObject);
    }
}
