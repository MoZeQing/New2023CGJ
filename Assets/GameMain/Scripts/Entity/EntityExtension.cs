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
            entityComponent.ShowEntity(typeof(BaseCharacter), "Coffee", 90, data);
        }
        public static void ShowNode(this EntityComponent entityComponent, NodeData data)
        {
            entityComponent.ShowEntity(typeof(BaseNode), "Coffee", 90, data);
        }

        public static void ShowCoffeeBeanNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(CoffeeBeanNode), "Coffee", 90, compenentData);
        }

        public static void ShowWaterNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(WaterNode), "Coffee", 90, compenentData);
        }
        public static void ShowSugarNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(SugarNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(IceNode), "Coffee", 90, compenentData);
        }
        public static void ShowGroundCoffeeNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(GroundCoffeeNode), "Coffee", 90, compenentData);
        }
        public static void ShowBurnisherNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(BurnisherNode), "Coffee", 90, compenentData);
        }
        public static void ShowCupNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(CupNode), "Coffee", 90, compenentData);
        }
        public static void ShowEspressoNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(EspressoNode), "Coffee", 90, compenentData);
        }
        public static void ShowCoffeeNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(CoffeeNode), "Coffee", 90, compenentData);
        }
        public static void ShowKettleNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(KettleNode), "Coffee", 90, compenentData);
        }
        public static void ShowFilterBowlNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(FilterBowlNode), "Coffee", 90, compenentData);
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
        public static void ShowCoffeeLiquidNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(CoffeeLiquidNode), "Coffee", 90, compenentData);
        }
        public static void ShowCafeAmericanoNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(CafeAmericanoNode), "Coffee", 90, compenentData);
        }
        public static void ShowWhiteCoffeeNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(WhiteCoffeeNode), "Coffee", 90, compenentData);
        }
        public static void ShowLatteNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(LatteNode), "Coffee", 90, compenentData);
        }
        public static void ShowMochaNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(MochaNode), "Coffee", 90, compenentData);
        }
        public static void ShowConPannaNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(ConPannaNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceCafeAmericanoNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(ConPannaNode), "Coffee", 90, compenentData);
        }
        public static void ShowSweetCafeAmericanoNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(ConPannaNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceEspressoNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(IceEspressoNode), "Coffee", 90, compenentData);
        }
        public static void ShowSweetEspressoNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(SweetEspressoNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceWhiteCoffeeNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(IceWhiteCoffeeNode), "Coffee", 90, compenentData);
        }
        public static void ShowSweetWhiteCofffeeNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(SweetWhiteCoffeeNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceConPannaNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(IceConPannaNode), "Coffee", 90, compenentData);
        }
        public static void ShowSweetConPannaNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(SweetConPannaNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceLatteNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(IceLatteNode), "Coffee", 90, compenentData);
        }
        public static void ShowSweetLatteNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(SweetLatteNode), "Coffee", 90, compenentData);
        }
        public static void ShowIceMochaNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(IceMochaNode), "Coffee", 90, compenentData);
        }
        public static void ShowSweetMochaNode(this EntityComponent entityComponent, CompenentData compenentData)
        {
            entityComponent.ShowEntity(typeof(SweetMochaNode), "Coffee", 90, compenentData);
        }
        public static void ShowCat(this EntityComponent entityComponent, EntityData data)
        {
            entityComponent.ShowEntity(typeof(Cat), "Cat", 90, data);
        }
        public static void ShowLittleChar(this EntityComponent entityComponent, EntityData data)
        {
            entityComponent.ShowEntity(typeof(LittleCat), "Cat", 90, data);
        }
        public static void ShowDialogStage(this EntityComponent entityComponent, EntityData data)
        {
            entityComponent.ShowEntity(typeof(DialogStage), "Dialog", 90, data);
        }
        public static void ShowTeachStage(this EntityComponent entityComponent, EntityData data)
        {
            entityComponent.ShowEntity(typeof(TeachStage), "Dialog", 90, data);
        }
        public static void ShowCharacter(this EntityComponent entityComponent, EntityData data)
        {
            entityComponent.ShowEntity(typeof(BaseCharacter), "Dialog", 90, data);
        }
        public static void ShowOrder(this EntityComponent entityComponent, EntityData data)
        { 
            //entityComponent.ShowOrder(typeof())
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


