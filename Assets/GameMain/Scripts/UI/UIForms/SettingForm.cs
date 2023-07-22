using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingForm : MonoBehaviour
{
    [SerializeField] private Button m_TitleButton;
    [SerializeField] private Button m_BackButton;
    [SerializeField] private Button m_StaffButton;
    [SerializeField] private Button m_StaffBackButton;
    [SerializeField] private Slider m_BGMVolumeSlider;
    [SerializeField] private GameObject m_StaffForm;
    // Start is called before the first frame update
    void Start()
    {
        m_StaffButton.onClick.AddListener(() => m_StaffForm.SetActive(true));
        m_StaffBackButton.onClick.AddListener(() => m_StaffForm.SetActive(false));
        m_BackButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        m_TitleButton.onClick.AddListener(Back);
        m_BGMVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Back()
    {
        this.gameObject.SetActive(false);
        ProcedureMain main = (ProcedureMain)GameEntry.Procedure.CurrentProcedure;
        main.BackGame();
    }

    private void OnBGMVolumeChanged(float volume)
    {
        GameEntry.Sound.SetVolume("BGM", volume);
    }
}
