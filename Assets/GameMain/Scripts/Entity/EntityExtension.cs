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

        public static void ShowCoffeeBeanNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(CoffeeBeanNode), "Coffee", 90, compenentData);
        }
        public static void ShowCoarseGroundCoffeeNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(CoarseGroundCoffeeNode), "Coffee", 90, compenentData);
        }
        public static void ShowMidGroundCoffeeNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(MidGroundCoffeeNode), "Coffee", 90, compenentData);
        }
        public static void ShowFineGroundCoffeeNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(FineGroundCoffeeNode), "Coffee", 90, compenentData);
        }
        public static void ShowWaterNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(WaterNode), "Coffee", 90, compenentData);
        }
        public static void ShowHotWaterNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(HotWaterNode), "Coffee", 90, compenentData);
        }
        public static void ShowMilkNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(MilkNode), "Coffee", 90, compenentData);
        }
        public static void ShowHotMilkNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(HotMilkNode), "Coffee", 90, compenentData);
        }
        public static void ShowCreamNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(CreamNode), "Coffee", 90, compenentData);
        }
        public static void ShowChocolateSyrupNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(ChocolateSyrupNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(IceNode), "Coffee", 90, compenentData);
        }
        public static void ShowSugarNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(SugarNode), "Coffee", 90, compenentData);
        }
        public static void ShowSaltNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(SaltNode), "Coffee", 90, compenentData);
        }
        public static void ShowCondensedMilkNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(CondensedMilkNode), "Coffee", 90, compenentData);
        }
        public static void ShowLowFoamingMilkNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(LowFoamingMilkNode), "Coffee", 90, compenentData);
        }
        public static void ShowHighFoamingMilkNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(HighFoamingMilkNode), "Coffee", 90, compenentData);
        }
        public static void ShowManualGrinderNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(ManualGrinderNode), "Coffee", 90, compenentData);
        }
        public static void ShowExtractorNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(ExtractorNode), "Coffee", 90, compenentData);
        }
        public static void ShowElectricGrinderNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(ElectricGrinderNode), "Coffee", 90, compenentData);
        }
        public static void ShowHeaterNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(HeaterNode), "Coffee", 90, compenentData);
        }
        public static void ShowSyphonNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(SyphonNode), "Coffee", 90, compenentData);
        }
        public static void ShowFrenchPressNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(FrenchPressNode), "Coffee", 90, compenentData);
        }
        public static void ShowKettleNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(KettleNode), "Coffee", 90, compenentData);
        }
        public static void ShowFilterBowlNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(FilterBowlNode), "Coffee", 90, compenentData);
        }
        public static void ShowCupNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(CupNode), "Coffee", 90, compenentData);
        }
        public static void ShowStirrerNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(StirrerNode), "Coffee", 90, compenentData);
        }
        public static void ShowEspressoNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(EspressoNode), "Coffee", 90, compenentData);
        }
        public static void ShowHotCafeAmericanoNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(HotCafeAmericanoNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceCafeAmericanoNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(IceCafeAmericanoNode), "Coffee", 90, compenentData);
        }
        public static void ShowHotLatteNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(HotLatteNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceLatteNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(IceLatteNode), "Coffee", 90, compenentData);
        }
        public static void ShowHotMochaNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(HotMochaNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceMochaNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(IceMochaNode), "Coffee", 90, compenentData);
        }
        public static void ShowKapuzinerNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(KapuzinerNode), "Coffee", 90, compenentData);
        }
        public static void ShowFlatWhiteNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(FlatWhiteNode), "Coffee", 90, compenentData);
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


