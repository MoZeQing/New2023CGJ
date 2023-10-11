using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private OutingSceneState mOutingSceneState;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameEntry.Utils.Ap >= 2)
        {
            GameEntry.Utils.Ap -= 2;
        }
        else
        {
            return;
        }
        GameEntry.Utils.Location = mOutingSceneState;
        GameEntry.UI.OpenUIForm((UIFormId)(20 + (int)mOutingSceneState), this);
        GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
        GameEntry.Utils.outingBefore = true;
        GameEntry.Dialog.StoryUpdate();
    }
}
