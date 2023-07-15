using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;

namespace GameMain
{
    public class RecipeForm :MonoBehaviour
    {
        [SerializeField] private Button mUpBtn;
        [SerializeField] private Button mDownBtn;
        [SerializeField] private Text mNum;
        [SerializeField] private List<Sprite> mSprites= new List<Sprite>();

        private SpriteRenderer m_SpriteRenderer = null;
        private int m_Index = 0;
        // Start is called before the first frame update
        void Start()
        {
            m_SpriteRenderer= GetComponent<SpriteRenderer>();

            mUpBtn.onClick.AddListener(Up);
            mDownBtn.onClick.AddListener(Down); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Up()
        {
            m_Index++;
            if(m_Index>=mSprites.Count)
                m_Index= 0;
            m_SpriteRenderer.sprite = mSprites[m_Index];
        }

        private void Down() 
        { 
            m_Index--;
            if (m_Index < 0)
                m_Index = mSprites.Count-1;
            m_SpriteRenderer.sprite = mSprites[m_Index];
        }
    }
}
