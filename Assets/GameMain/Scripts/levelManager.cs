using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;

namespace GameMain
{
    public class levelManager : MonoBehaviour
    {
        private OrderData orderData1;
        private OrderData orderData2;
        private OrderData orderData3;
        private OrderData orderData4;
        private OrderData orderData5;

        [SerializeField] private DialogueGraph dialogueGraph1;
        [SerializeField] private DialogueGraph dialogueGraph2;
        [SerializeField] private DialogueGraph dialogueGraph3;
        [SerializeField] private DialogueGraph dialogueGraph4;
        [SerializeField] private DialogueGraph dialogueGraph5;

        private int level;
        // Start is called before the first frame update
        void Start()
        {
            orderData1 = new OrderData();
            orderData1.CafeAmericano = 1;
            orderData1.Dialog = "plot_cat_1";

            orderData2 = new OrderData();
            orderData2.Mocha = 1;
            orderData2.Espresso = 1;
            orderData2.Dialog = "plot_cat_2";

            orderData3 = new OrderData();
            orderData3.ConPanna = 1;
            orderData3.Latte = 1;
            orderData3.WhiteCoffee = 1;
            orderData3.Dialog = "plot_cat_3";

            orderData4 = new OrderData();
            orderData4.CafeAmericano = 1;
            orderData3.ConPanna = 1;
            orderData3.Latte = 1;
            orderData3.WhiteCoffee = 1;
            orderData4.Dialog = "plot_cat_4";

            orderData5 = new OrderData();
            orderData5.CafeAmericano = 1;
            orderData2.Mocha = 1;
            orderData2.Espresso = 1;
            orderData3.ConPanna = 1;
            orderData3.Latte = 1;
            orderData3.WhiteCoffee = 1;
            orderData5.Dialog = "plot_cat_5";
        }

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(LevelEventArgs.EventId, Level);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, Level);
        }
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
            }
        }
    }

}