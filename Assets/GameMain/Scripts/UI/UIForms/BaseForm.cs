using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class BaseForm : UIFormLogic
    {
        [SerializeField] private BuildingData m_BuildingData = null;
        [SerializeField] private Image m_BuildingImage = null;
        [SerializeField] private Text m_TitleText = null;
        [SerializeField] private Text m_ProdureText = null;
        [SerializeField] private Text m_DescriptionText = null;
        [SerializeField] private Button m_UpgradeBtn = null;
        [SerializeField] private Button m_DowngradeBtn = null;
        [SerializeField] private Button m_DispatchBtn = null;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_BuildingData = (BuildingData)userData;
            //m_BuildingImage.sprite = GameEntry.Utils.sprites[(int)m_BuildingData.BuildingTag];
            m_TitleText.text = m_BuildingData.Title.ToString();
            m_ProdureText.text = m_BuildingData.Produre.ToString();
            m_DescriptionText.text = m_BuildingData.Description.ToString();

            if (m_BuildingData.BuildingTag == BuildingTag.Empty)
                m_UpgradeBtn.onClick.AddListener(Building_OnClick);
            else
                m_UpgradeBtn.onClick.AddListener(Upgrade_OnClick);
            m_DowngradeBtn.onClick.AddListener(Downgrade_OnClick);
            m_DispatchBtn.onClick.AddListener(Dispatch_OnClick);
        }

        private void Upgrade_OnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.UpgradeForm, m_BuildingData);
        }

        private void Building_OnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.BuildingForm, m_BuildingData);
        }

        private void Downgrade_OnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.DowngradeForm, m_BuildingData);
        }

        private void Dispatch_OnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.DispatchForm, m_BuildingData);
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


