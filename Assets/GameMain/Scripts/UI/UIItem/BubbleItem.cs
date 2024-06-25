using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleItem : MonoBehaviour
{
    [SerializeField] private Image[] stars;
    [SerializeField] private Text text;

    public void SetData(int score,string comment)
    {
        this.gameObject.SetActive(true);
        this.GetComponent<AudioSource>().Play();
        this.StopAllCoroutines();
        this.Invoke(nameof(HideItem), 2f);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i<score);
        }
        text.text = comment;
    }

    public void HideItem()
    { 
        this.gameObject.SetActive(false);   
    }
}
