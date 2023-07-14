using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework.DataTable;

namespace GameMain
{
    public class levelManager : MonoBehaviour
    {
        private OrderData orderData1;
        private OrderData orderData2;
        private OrderData orderData3;
        private OrderData orderData4;
        private OrderData orderData5;
        private OrderData orderData6;
        private OrderData orderData7;
        private OrderData orderData8;
        private OrderData orderData9;
        private OrderData orderData10;
        private OrderData orderData11;
        private OrderData orderData12;


        [SerializeField] private DialogueGraph dialogueGraph1;
        [SerializeField] private DialogueGraph dialogueGraph2;
        [SerializeField] private DialogueGraph dialogueGraph3;
        [SerializeField] private DialogueGraph dialogueGraph4;
        [SerializeField] private DialogueGraph dialogueGraph5;
        [SerializeField] private DialogueGraph dialogueGraph6;
        [SerializeField] private DialogueGraph dialogueGraph7;
        [SerializeField] private DialogueGraph dialogueGraph8;
        [SerializeField] private DialogueGraph dialogueGraph9;
        [SerializeField] private DialogueGraph dialogueGraph10;
        [SerializeField] private DialogueGraph dialogueGraph11;
        [SerializeField] private DialogueGraph dialogueGraph12;

        //获取到表
        private IDataTable<DROrder> dtOrder = GameEntry.DataTable.GetDataTable<DROrder>();

        private int level;
        // Start is called before the first frame update
        void Start()
        {

            DROrder drOrder = dtOrder.GetDataRow(0);
            orderData1 = new OrderData(drOrder);

            DROrder drOrder1 = dtOrder.GetDataRow(1);
            orderData2 = new OrderData(drOrder1);

            DROrder drOrder2 = dtOrder.GetDataRow(2);
            orderData3 = new OrderData(drOrder2);

            DROrder drOrder3 = dtOrder.GetDataRow(3);
            orderData4 = new OrderData(drOrder3);

            DROrder drOrder4 = dtOrder.GetDataRow(4);
            orderData5 = new OrderData(drOrder4);
            
            DROrder drOrder5 = dtOrder.GetDataRow(5);
            orderData6 = new OrderData(drOrder5);

            DROrder drOrder6 = dtOrder.GetDataRow(6);
            orderData7 = new OrderData(drOrder6);

            DROrder drOrder7 = dtOrder.GetDataRow(7);
            orderData8 = new OrderData(drOrder7);

            DROrder drOrder8 = dtOrder.GetDataRow(8);
            orderData9 = new OrderData(drOrder8);

            DROrder drOrder9 = dtOrder.GetDataRow(9);
            orderData10 = new OrderData(drOrder9);

            DROrder drOrder10 = dtOrder.GetDataRow(10);
            orderData11 = new OrderData(drOrder10);

            DROrder drOrder11 = dtOrder.GetDataRow(11);
            orderData12 = new OrderData(drOrder11);
        }

        //private void OnEnable()
        //{
        //    GameEntry.Event.Subscribe(LevelEventArgs.EventId, Level);
        //}

        //private void OnDisable()
        //{
        //    GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, Level);
        //}
        // Update is called once per frame
        void Update()
        {

        }

        private void Level(object sender, GameEventArgs e)
        {
            OrderManager orderManager = (OrderManager)sender;
            DialogForm dialog = GameObject.FindWithTag("Dialog").GetComponent<DialogForm>();
                level++;
            switch (level)
            {
                case 1:
                    orderManager.SetOrder(orderData1);
                    dialog.SetDialog(dialogueGraph1);
                    break;
                case 2:
                    orderManager.SetOrder(orderData2);
                    dialog.SetDialog(dialogueGraph2);
                    break;
                case 3:
                    orderManager.SetOrder(orderData3);
                    dialog.SetDialog(dialogueGraph3);
                    break;
                case 4:
                    orderManager.SetOrder(orderData4);
                    dialog.SetDialog(dialogueGraph4);
                    break;
                case 5:
                    orderManager.SetOrder(orderData5);
                    dialog.SetDialog(dialogueGraph5);
                    break;
                case 6:
                    orderManager.SetOrder(orderData6);
                    dialog.SetDialog(dialogueGraph6);
                    break;
                case 7:
                    orderManager.SetOrder(orderData7);
                    dialog.SetDialog(dialogueGraph7);
                    break;
                case 8:
                    orderManager.SetOrder(orderData8);
                    dialog.SetDialog(dialogueGraph8);
                    break;
                case 9:
                    orderManager.SetOrder(orderData9);
                    dialog.SetDialog(dialogueGraph9);
                    break;
                case 10:
                    orderManager.SetOrder(orderData10);
                    dialog.SetDialog(dialogueGraph10);
                    break;
                case 11:
                    orderManager.SetOrder(orderData11);
                    dialog.SetDialog(dialogueGraph11);
                    break;
                case 12:
                    orderManager.SetOrder(orderData11);
                    dialog.SetDialog(dialogueGraph11);
                    break;
            }
        }
    }

}