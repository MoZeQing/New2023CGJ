using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingForm : MonoBehaviour
{
    [SerializeField] private Button mTitleButton;
    [SerializeField] private Button mBackButton;
    [SerializeField] private Button mStaffButton;
    [SerializeField] private Button mStaffBackButton;
    [SerializeField] private GameObject mStaffForm;
    // Start is called before the first frame update
    void Start()
    {
        mStaffButton.onClick.AddListener(() => mStaffForm.SetActive(true));
        mStaffBackButton.onClick.AddListener(() => mStaffForm.SetActive(false));
        mBackButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        mTitleButton.onClick.AddListener(Back);
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
}
