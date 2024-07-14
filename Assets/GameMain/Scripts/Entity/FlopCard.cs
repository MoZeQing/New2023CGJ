using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlopCard : MonoBehaviour
{
    public int ID;
    public bool isFlip = false;
    public bool canClick = true;
    public bool isDone = false;
    [SerializeField] Color back;
    [SerializeField] Color front;

    private Text text;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        text.text = ID.ToString();
        text.enabled = false;
        this.GetComponent<Image>().color = back;
    }

    public void OnClick()
    {
        if(canClick)
        {
            isFlip = true;
            canClick = false;
            transform.DOScaleX(-transform.localScale.x, 0.5f).OnComplete(()=>text.enabled = true);
            DOVirtual.DelayedCall(0.14f, () => this.GetComponent<Image>().color = front);
        }
    }

    public void TrunBack()
    {
        isFlip = false;
        canClick = true;
        StartCoroutine(TurnBackCall());
    }

    public void CoolDown()
    {
        canClick = false;
        DOVirtual.DelayedCall(2f, () => canClick = true);
    }

    IEnumerator TurnBackCall()
    {
        yield return new WaitForSeconds(2f);

       
        transform.DOScaleX(-transform.localScale.x, 0.5f).OnComplete(() => text.enabled = false);
        DOVirtual.DelayedCall(0.14f, () => this.GetComponent<Image>().color = back);
    }
}
