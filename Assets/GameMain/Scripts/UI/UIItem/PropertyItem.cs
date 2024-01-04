using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class PropertyItem : MonoBehaviour
    {
        [SerializeField] private Text text;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(PropertyTag propertyTag, string value)
        {
            text.text = value;
        }
    }
}
