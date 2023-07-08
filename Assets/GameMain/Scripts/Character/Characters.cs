using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{


   /* 
    protected virtual void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //ÈËÎïÍÏ×§
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePosition.z = 0;
        float Distance = (this.gameObject.transform.position - MousePosition).magnitude;
        if (Input.GetMouseButtonDown(0))
        {

            CharacterAbility.SetActive(true);
            if (Distance < 1.5f)
            {
                Control = true;
                Mouse = MousePosition;
            }
           
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Control = false;
        }
        if (Control)
        {
            Vector3 DeltaVec = MousePosition - Mouse;
            this.gameObject.transform.position += DeltaVec;
            Mouse = MousePosition;
        }
    }*/
}

