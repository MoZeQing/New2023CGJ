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
        Invoke(nameof(OnGameOutingChange), 1f);
    }

    private void OnGameOutingChange()
    {
        GameEntry.Utils.Location = mOutingSceneState;
        //GameEntry.UI.OpenUIForm((UIFormId)(12 + (int)mOutingSceneState), this);
        GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Outing));
    }
}
