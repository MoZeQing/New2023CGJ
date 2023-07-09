using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.DataTable;
using UnityEngine.UI;


namespace GameMain
{
    public class OrderManager : MonoBehaviour
    {
        private List<DROrder> orders = new List<DROrder>();
        private OrderData orderData;
        private MaterialData materialData=new MaterialData();
        // Start is called before the first frame update
        void Start()
        {
            IDataTable<DROrder> dtOrder = GameEntry.DataTable.GetDataTable<DROrder>();
            DROrder drOrder = dtOrder.GetDataRow(0);
            orderData = new OrderData(drOrder);
            GameEntry.Event.FireNow(this, OrderEventArgs.Create(orderData));
        }

        // Update is called once per frame
        void Update()
        {
            if (materialData.Milk < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Milk)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                }) ;
            }
            if (materialData.Sugaer < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Sugar)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
            }
            if (materialData.Cream < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Cream)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
            }
            if (materialData.CoffeeBean < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CoffeeBean)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
            }
            if (materialData.Water < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Water)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
            }
            if (materialData.ChocolateSyrup < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ChocolateSyrup)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
            }

            if (orderData.Check())
            {
                orderData = new OrderData(orders[Random.Range(0, orders.Count)]);
                GameEntry.Event.FireNow(this, OrderEventArgs.Create(orderData));
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            BaseCompenent baseCompenent = null;
            if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                NodeData nodeData = baseCompenent.transform.parent.GetComponent<BaseNode>().NodeData;
                switch (nodeData.NodeTag)
                {
                    case NodeTag.Espresso:
                        orderData.Espresso -= 1;
                        break;
                    case NodeTag.ConPanna:
                        orderData.ConPanna -= 1;
                        break;
                    case NodeTag.Mocha:
                        orderData.Mocha -= 1;
                        break;
                    case NodeTag.WhiteCoffee:
                        orderData.WhiteCoffee -= 1;
                        break;
                    case NodeTag.CafeAmericano:
                        orderData.CafeAmericano -= 1;
                        break;
                    case NodeTag.Latte:
                        orderData.Latte -= 1;
                        break;
                    default:
                        return;
                }
                GameEntry.Entity.HideEntity(nodeData.Id);
                GameEntry.Event.FireNow(this, OrderEventArgs.Create(orderData));
            }
        }
    }

    public class MaterialData
    { 
        public int Water
        {
            get;
            set;
        }

        public int Sugaer
        {
            get;
            set;
        }

        public int CoffeeBean
        {
            get;
            set;
        }

        public int Milk
        {
            get;
            set;
        }

        public int ChocolateSyrup
        {
            get;
            set;
        }

        public int Cream
        {
            get;
            set;
        }
    }

    public class OrderData
    {
        /// <summary>
        /// 获取浓缩咖啡。
        /// </summary>
        public int Espresso
        {
            get;
            set;
        }

        /// <summary>
        /// 获取拿铁。
        /// </summary>
        public int Latte
        {
            get;
            set;
        }

        /// <summary>
        /// 获取美式咖啡。
        /// </summary>
        public int CafeAmericano
        {
            get;
            set;
        }

        /// <summary>
        /// 获取白咖啡。
        /// </summary>
        public int WhiteCoffee
        {
            get;
            set;
        }

        /// <summary>
        /// 获取摩卡。
        /// </summary>
        public int Mocha
        {
            get;
            set;
        }

        /// <summary>
        /// 获取康宝蓝。
        /// </summary>
        public int ConPanna
        {
            get;
            set;
        }

        public bool Check()
        {
            return (ConPanna + Mocha + WhiteCoffee + CafeAmericano + Latte + Espresso) == 0;
        }

        public OrderData() { }
        public OrderData(DROrder order)
        {
            ConPanna = order.ConPanna;
            Mocha = order.Mocha;
            WhiteCoffee = order.WhiteCoffee;
            Latte = order.Latte;
            CafeAmericano = order.CafeAmericano;
            Espresso = order.Espresso;
        }
    }
}
