using GameFramework.DataTable;
using GameFramework.Entity;
using System;
//using TreeEditor;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine.Tilemaps;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static void ShowCharacter(this EntityComponent entityComponent, CharacterData data)
        {
            entityComponent.ShowEntity(typeof(BaseCharacter), "Dialog", 90, data);
        }
        public static void ShowNode(this EntityComponent entityComponent, NodeData data)
        {
            entityComponent.ShowEntity(typeof(BaseNode), "Coffee", 90, data);
        }

        public static void ShowComponent(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(BaseCompenent), "Coffee", 90, compenentData);
        }
        public static void ShowToolComponent(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(ToolCompenent), "Coffee", 90, compenentData);
        }
        public static void ShowOrder(this EntityComponent entityComponent, OrderItemData data)
        {
            entityComponent.ShowEntity(typeof(OrderItem), "Order", 90, data);
        }
        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority, data);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}


