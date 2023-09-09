using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private Text headerField;
        [SerializeField] private Text contentField;
        void Start()
        {

        }

        void Update()
        {

        }
        public void SetText(string content, string header="")
        {
            if(string.IsNullOrEmpty(header))
            {
                headerField.gameObject.SetActive(false);
            }
            else
            {
                headerField.gameObject.SetActive(true);
                headerField.text = header;
            }
            contentField.text = content;

        }
    }
}
