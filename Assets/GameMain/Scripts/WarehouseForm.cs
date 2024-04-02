using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain 
{
    public class WarehouseForm : MonoBehaviour
    {
        [SerializeField] private Button cupboradBtn;
        [SerializeField] private Button closetBtn;
        [SerializeField] private Button instrumentBtn;

        // Start is called before the first frame update
        private void OnEnable()
        {
            cupboradBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.CupboradForm));
            closetBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.ClosetForm));
            instrumentBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.InstrumentForm));
        }

        private void OnDisable()
        {
            cupboradBtn.onClick.RemoveAllListeners();
            closetBtn.onClick.RemoveAllListeners();
            instrumentBtn.onClick.RemoveAllListeners();
        }
    }
}
