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
        GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
        Invoke(nameof(OnGameStateChange), 1f);
    }

    private void OnGameStateChange()
    {
        GameEntry.Utils.Location = mOutingSceneState;
        GameEntry.UI.OpenUIForm((UIFormId)(20 + (int)mOutingSceneState), this);
        GameEntry.Dialog.StoryUpdate();
    }
}
