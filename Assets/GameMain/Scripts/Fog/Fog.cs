using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//只要能糊弄玩家就行了，我死后哪管他洪水滔天！
public class Fog : MonoBehaviour
{
    private Transform mMask;
    private Transform mBG;

    private void Start()
    {
        mMask = this.transform;
        mBG = this.transform.Find("BG").transform;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = MouseToWorld(Input.mousePosition);
        if (Mathf.Abs(mousePos.x) < 7.18f &&
            mousePos.y < 2.84f && mousePos.y > -4.76f)
        {
            mMask.transform.position = MouseToWorld(Input.mousePosition);
        }
        mBG.transform.position = Vector3.zero;
    }

    private Vector3 MouseToWorld(Vector3 mousePos)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.z = screenPosition.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
