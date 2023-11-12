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

        // Start is called before the first frame update
        private void OnEnable()
        {
            cupboradBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.CupboradForm));
            closetBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.ClosetForm));
        }

        private void OnDisable()
        {
            cupboradBtn.onClick.RemoveAllListeners();
            closetBtn.onClick.RemoveAllListeners();
        }
    }
}
