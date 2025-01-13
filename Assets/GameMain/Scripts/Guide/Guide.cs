using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Guide : MonoBehaviour
    {

        [SerializeField] private List<GameObject> materials = new List<GameObject>();

        void Start()
        {
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide1_1);
        }

        // Update is called once per frame

        public void Guide1_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                //��ʼ��
                materials[(int)NodeTag.CoffeeBean].SetActive(true);
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("���`�ҩ`������åȤ򥯥�å�����ȡ����ꤹ����ϥ��`�ɤ����ɤ���ޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-370f, -250f, 0f), Vector3.zero));
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide1_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_2);
            }
        }

        public void Guide1_2(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.CoffeeBean)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ManualGrinder)
                {
                    Position = new Vector3(-1.86f, -3.68f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("���饤����`��ʹ�ä��ƥ��`�ҩ`������줭�Υ��`�ҩ`�ۤˤ��ޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-200f, 100f, 0f), new Vector3(0f,0f,180f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_2);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_3);
            }
        }

        public void Guide1_3(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.CoarseGroundCoffee)
                    return;
                materials[1].SetActive(true);//ˮ
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("ˮ�ۥ���åȤ򥯥�å�����ȡ����ꤹ����ϥ��`�ɤ����ɤ���ޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(450f, 150f, 0f), new Vector3(0f, 0f, 90f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_4);
            }
        }

        public void Guide1_4(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Water)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Heater)
                {
                    Position = new Vector3(2.96f, -3.74f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("�ӟ�����ʹ�ä���ˮ��ӟᤷ�ޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(270f, 100f, 0f), new Vector3(0f, 0f, 180f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_4);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_5);
            }
        }

        public void Guide1_5(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.HotWater)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.FrenchPress)
                {
                    Position = new Vector3(0.21f, -3.66f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("�ᜫ�ȴ��줭���`�ҩ`�ۤ�ե��륿�`�ک`�ѩ`��©�������ơ������ץ�å�������ޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(30f, 100f, 0f), new Vector3(0f, 0f, 180f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_5);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_6);
            }
        }

        public void Guide1_6(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Espresso)
                    return;
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("���ɤ��줿���`�ҩ`���`�ɤ���ȤΌ��ꤹ��ע�ęڤ˥ɥ�å����ޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-450f, 200f, 0f), new Vector3(0f, 0f, 270f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_6);
                GameEntry.Event.Subscribe(OrderEventArgs.EventId, Guide1_7);
            }
        }
        public void Guide1_7(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (args.OrderData.NodeTag != NodeTag.Espresso)
                return;
            GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(false));
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, Guide1_7);
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide2_1);
        }
        //�����ֿ��Ȱ汾������
        public void Guide2_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("�Ǥϡ��¤��������ץ�å�������ޤ��礦", WorkTips.None));
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide2_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_2);
            }
        }

        public void Guide2_2(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Espresso)
                    return;
                materials[2].SetActive(true);
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("ţ�饹��åȤ򥯥�å�����ȡ����ꤹ����ϥ��`�ɤ����ɤ���ޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-220f, -250f, 0f), Vector3.zero));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_2);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_3);
            }
        }

        public void Guide2_3(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Milk)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Stirrer)
                {
                    Position = new Vector3(5.26f, -3.73f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("�ߥ����`��ʹ�ä���ţ��������Ƥޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(510f, 100f, 0f), new Vector3(0f, 0f, 180f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_4);
            }
        }

        public void Guide2_4(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.LowFoamingMilk)
                    return;
                materials[3].SetActive(true);
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("���åץ���åȤ򥯥�å�����ȡ����ꤹ����ϥ��`�ɤ����ɤ���ޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(450f, -25f, 0f), new Vector3(0f, 0f, 90f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_4);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_5);
            }
        }

        public void Guide2_5(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Cup)
                    return;
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("���åפ�ʹ�äƥ����ץ�å��ȥե��`��ߥ륯��M�ߺϤ碌�����ץ��`�Τ�����ޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(false));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_5);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_6);
            }
        }

        public void Guide2_6(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Kapuziner)
                    return;
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("���ץ��`�Τ�ע�Ĥ�������ޤ�", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-450f, 200f, 0f), new Vector3(0f, 0f, 270f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_6);
                GameEntry.Event.Subscribe(OrderEventArgs.EventId, Guide2_7);
            }
        }
        public void Guide2_7(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (args.OrderData.NodeTag != NodeTag.Kapuziner)
                return;
            GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(false));
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, Guide2_7);
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide3_1);
        }

        public void Guide3_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("�Τϼ��줭�����ץ�å�������ޤ��礦���ޤ������줭���`�ҩ`�ۤ�ʂ䤷�ޤ�", WorkTips.None));
                GameEntry.Player.AddRecipe(1);
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide3_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
                Invoke(nameof(Guide3_2), 2f);
            }
        }
        public void Guide3_2()
        {
            GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("���줭���`�ҩ`�ۤȤϡ����줭���`�ҩ`�ۤ򤵤����ĥ������ΤǤ�", WorkTips.None));
        }
        //���ڣ��������ϸ������
        public void Guide3_3(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.FineGroundCoffee)
                    return;
                materials[7].SetActive(true);
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("�����A�������줭�Щ`�����Υ��������ץ��`�Τ���ɤ����ޤ��礦��  <size=24>���쥷�Ԥ��Ɇ���������Ϥϡ����¤����ɥޥ˥奢��򥯥�å����ƴ_�J�Ǥ��ޤ���</size>  ", WorkTips.None));

                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide3_5);
                Invoke(nameof(Guide3_4), 2f);
            }
        }

        public void Guide3_4()
        {
            GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(550f, -250f, 0f), Vector3.zero));
        }

        public void Guide3_5(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.IceKapuziner)
                    return;
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("����Ǥϡ����`�ҩ`�򌝏ꤹ��ע�ęڤ˥ɥ�å����Ƥ�������", WorkTips.None));
                GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(true, new Vector3(-450f, 200f, 0f), new Vector3(0f, 0f, 270f)));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_5);
                GameEntry.Event.Subscribe(OrderEventArgs.EventId, Guide3_6);
            }
        }

        public void Guide3_6(object sender, GameEventArgs e)
        {
            OrderEventArgs args = (OrderEventArgs)e;
            if (args.OrderData.NodeTag != NodeTag.IceKapuziner)
                return;
            GameEntry.Event.FireNow(ArrowEventArgs.EventId, ArrowEventArgs.Create(false));
            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, Guide3_6);
        }
    }
}