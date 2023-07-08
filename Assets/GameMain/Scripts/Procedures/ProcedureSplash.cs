using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Localization;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Procedure;
using GameFramework.Resource;

namespace GameMain
{
    public class ProcedureSplash : ProcedureBase
    {
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            // TODO: 这里可以播放一个 Splash 动画
            // ...
            ChangeState<ProcedureInitResources>(procedureOwner);
            //if (GameEntry.Base.EditorResourceMode)
            //{
            //    // 编辑器模式
            //    Log.Info("Editor resource mode detected.");
            //    //ChangeState<ProcedurePreload>(procedureOwner);
            //}
            //else if (GameEntry.Resource.ResourceMode == ResourceMode.Package)
            //{
            //    // 单机模式
            //    Log.Info("Package resource mode detected.");

            //}
            //else
            //{
            //    // 可更新模式
            //    Log.Info("Updatable resource mode detected.");
            //    //ChangeState<ProcedureCheckVersion>(procedureOwner);
            //}
        }
    }
}
