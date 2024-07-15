using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameMain
{
    public class FlopCard : MonoBehaviour
    {
        public int ID;
        public bool isFlip = false;
        public bool canClick = true;
        public bool isDone = false;
        [SerializeField] private Sprite back;
        public Sprite front;

        //private Text text;

        private void Start()
        {
            //text = GetComponentInChildren<Text>();
            //text.text = ID.ToString();
            //text.enabled = false;
            this.GetComponent<Image>().sprite = back;
        }

        public void OnClick()
        {
            if (canClick)
            {
                isFlip = true;
                canClick = false;
                GameEntry.Sound.PlaySound(10);
                transform.DOScaleX(-transform.localScale.x, 0.5f);
                DOVirtual.DelayedCall(0.14f, () => this.GetComponent<Image>().sprite = front);
            }
        }

        public void TrunBack()
        {
            isFlip = false;
            canClick = true;
            GameEntry.Sound.PlaySound(10);
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


            transform.DOScaleX(-transform.localScale.x, 0.5f);
            DOVirtual.DelayedCall(0.14f, () => this.GetComponent<Image>().sprite = back);
        }
    }
}



