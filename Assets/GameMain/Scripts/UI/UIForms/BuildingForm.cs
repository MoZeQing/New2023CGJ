using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using GameFramework.DataTable;

namespace GameMain
{
    public class BuildingForm : UIFormLogic
    {
        [SerializeField] private Text m_TitleText = null;
        [SerializeField] private Text m_ProdureText = null;
        [SerializeField] private Text m_DescriptionText = null;
        [SerializeField] private Button m_BuildingBtn = null;
        [SerializeField] private Transform m_Content = null;

        private BuildingData m_BuildingData = null;
        private BuildingTag m_BuildingTag;
        private BuildingTag[] m_BuildingTags = new BuildingTag[]
        {
            BuildingTag.Electricity1,
            BuildingTag.Training1,
            BuildingTag.Dorm1,
            BuildingTag.Garden1,
            BuildingTag.Workbench1,
        };

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_BuildingData =(BuildingData)userData;
            for (int i = 0; i < m_Content.transform.childCount; i++)
            {
                Destroy(m_Content.GetChild(i).gameObject);
            }
            foreach (BuildingTag buildingTag in m_BuildingTags)
            {
                GameObject go = new GameObject();
                go.name = buildingTag.ToString();
                go.transform.parent = m_Content.transform;
                go.AddComponent<Image>();

                go.GetComponent<RectTransform>().sizeDelta = new Vector2(192 * 4, 108 * 4);
                go.GetComponent<RectTransform>().localRotation = Quaternion.identity;
                go.AddComponent<BuildingItem>();
                go.AddComponent<Button>();
                go.GetComponent<BuildingItem>().OnInit(this, buildingTag);
            }
            Debug.Log(this.UIForm.PauseCoveredUIForm);
            m_BuildingBtn.onClick.AddListener(Building_OnClick);
        }

        public void SetBuilding(object sender)
        {             
            m_BuildingTag = (BuildingTag)sender;
            IDataTable<DRBuilding> dtBuilding = GameEntry.DataTable.GetDataTable<DRBuilding>();
            DRBuilding drBuilding = dtBuilding.GetDataRow((int)m_BuildingTag);

            m_TitleText.text = drBuilding.Title.ToString();
            m_ProdureText.text = drBuilding.Produre.ToString();
            m_DescriptionText.text = drBuilding.Description.ToString();
        }

        private void Building_OnClick()
        {
            if (m_BuildingData.BuildingTag != BuildingTag.Empty)
                return;
            if (m_BuildingData.Pos == null)
                return;
            GameEntry.UI.CloseUIForm(this.UIForm);
            //GameEntry.UI.CloseUIForm(UIFormId.BaseForm);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetMouseButtonDown(1))
                GameEntry.UI.CloseUIForm(this.UIForm);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}

