using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsorbSlot : BaseCompenent
{
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        CompenentData data = (CompenentData)userData;
        GameEntry.Entity.AttachEntity(this.Id, data.OwnerId);

    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
