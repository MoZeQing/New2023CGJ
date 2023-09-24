using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class Teach : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private TeachingForm teachForm;
        [SerializeField] private Image character;
        [SerializeField] private Button sleepBtn;
        [SerializeField] private string key;
        [SerializeField] private ActionGraph action;

        private void Start()
        {
            if (Resources.Load<ActionGraph>("ActionData/" + GameEntry.Utils.actionName) != null)
                teachForm.mActionGraph = Resources.Load<ActionGraph>("ActionData/" + GameEntry.Utils.actionName);
            if (GameEntry.Utils.CheckFlag(key))
            {
                teachForm.mActionGraph = action;
                canvas.SetActive(false);
                character.color = Color.clear;
                sleepBtn.gameObject.SetActive(true);
            }
        }
    }
}

