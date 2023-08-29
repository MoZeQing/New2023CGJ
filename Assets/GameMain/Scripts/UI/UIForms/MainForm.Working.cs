using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public partial class MainForm : UIFormLogic
    {
        //[Header("¹¤×÷ÇøÓò")]
        //[SerializeField] private Text EspressoText;
        //[SerializeField] private Text ConPannaText;
        //[SerializeField] private Text MochaText;
        //[SerializeField] private Text WhiteCoffeeText;
        //[SerializeField] private Text CafeAmericanoText;
        //[SerializeField] private Text LatteText;

        //private void Debug()
        //{
        //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CafeAmericano)
        //    {
        //        Position = new Vector3(0, -4.8f, 0)
        //    });
        //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Latte)
        //    {
        //        Position = new Vector3(0, -4.8f, 0)
        //    });
        //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ConPanna)
        //    {
        //        Position = new Vector3(0, -4.8f, 0)
        //    });
        //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Espresso)
        //    {
        //        Position = new Vector3(0, -4.8f, 0)
        //    });
        //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Mocha)
        //    {
        //        Position = new Vector3(0, -4.8f, 0)
        //    });
        //    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.WhiteCoffee)
        //    {
        //        Position = new Vector3(0, -4.8f, 0)
        //    });
        //}
        //private void Recipe()
        //{
        //    mRecipeForm.gameObject.SetActive(!mRecipeForm.gameObject.activeSelf);
        //}

        //private void UpdateOrder(object sender, GameEventArgs e)
        //{
        //    OrderEventArgs args = (OrderEventArgs)e;
        //    if (args.OrderData.Check())
        //        return;
        //    OrderData orderData = args.OrderData;
        //    EspressoText.text = orderData.Espresso.ToString();
        //    ConPannaText.text = orderData.ConPanna.ToString();
        //    MochaText.text = orderData.Mocha.ToString();
        //    WhiteCoffeeText.text = orderData.WhiteCoffee.ToString();
        //    CafeAmericanoText.text = orderData.CafeAmericano.ToString();
        //    LatteText.text = orderData.Latte.ToString();
        //}
    }
}