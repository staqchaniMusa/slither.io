using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    [SerializeField]
    Scrollbar H_Bar;
   public  int NumberOfObjects;
   public float Distance;

    public Button LeftBTN;
    public Button RightBTN;
    public float H_Bar_Newvalue;
    public GameObject cantain;
    // Start is called before the first frame update
    void Start()
    {
        H_Bar = GameObject.Find("Scrollbar Horizontal").GetComponent<Scrollbar>();
        cantain = this.gameObject.GetComponent<ScrollView>().cantain;
    }

    // Update is called once per frame
    void Update()
    {
       /* NumberOfObjects = cantain.transform.childCount - 1;
        Distance = 1f / NumberOfObjects;
        H_Bar.value = Mathf.Lerp(H_Bar.value, H_Bar_Newvalue, 0.1f );

       
        if(H_Bar.value < 0.99 && H_Bar.value > 0)
        {
            LeftBTN.interactable = true;
            RightBTN.interactable = true;
        }*/

    }

    public void MoveLeft()
    {
         if (H_Bar.value <= 0.1f)
        {
            LeftBTN.interactable = false;
        }
         else
        { 
        H_Bar_Newvalue = H_Bar.value - Distance;

            while(H_Bar.value > H_Bar_Newvalue)
                H_Bar.value = Mathf.Lerp(H_Bar.value, H_Bar_Newvalue, 0.1f);
        }
        print("MoveLeft");
    }

    public void MoveRight()
    {

        if (H_Bar.value >= 0.99)
        {
            RightBTN.interactable = false;
        }
        else
        {
            H_Bar_Newvalue = H_Bar.value + Distance;
            while (H_Bar.value < H_Bar_Newvalue)
                H_Bar.value = H_Bar.value + 0.01f;

              //  H_Bar.value = Mathf.Lerp(H_Bar.value, H_Bar_Newvalue, 0.1f);
        }
        print("MoveRight");
    }
}
