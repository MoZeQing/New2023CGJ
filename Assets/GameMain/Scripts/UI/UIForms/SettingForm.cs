using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingForm : SettleForm
{
    [SerializeField] private Button m_TitleButton;
    [SerializeField] private Button m_BackButton;
    [SerializeField] private Button m_StaffButton;
    [SerializeField] private Button m_StaffBackButton;
    [SerializeField] private Slider m_BGMVolumeSlider;
    [SerializeField] private Slider m_AudioVolumeSlider;
    [SerializeField] private GameObject m_StaffForm;
    // Start is called before the first frame update
    void Start()
    {
        m_StaffButton.onClick.AddListener(() => m_StaffForm.SetActive(true));
        m_StaffBackButton.onClick.AddListener(() => m_StaffForm.SetActive(false));
        m_BackButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        m_TitleButton.onClick.AddListener(Back);
        m_BGMVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        m_AudioVolumeSlider.onValueChanged.AddListener(OnAudioVolumeChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Back()
    {
        this.gameObject.SetActive(false);
        ProcedureMain main = (ProcedureMain)GameEntry.Procedure.CurrentProcedure;
    }

    private void OnBGMVolumeChanged(float volume)
    {
        GameEntry.Sound.SetVolume("BGM", volume);
    }
    private void OnAudioVolumeChanged(float volume)
    {
        GameEntry.Sound.SetVolume("Sound", volume);
    }
}
