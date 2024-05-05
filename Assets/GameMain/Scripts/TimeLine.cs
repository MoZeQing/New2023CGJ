using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class TimeLine : MonoBehaviour
    {
        [SerializeField] private Text moneyText;
        [SerializeField] private Text energyText;
        [SerializeField] private Text favorText;
        // Start is called before the first frame update

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);
            GameEntry.Event.Subscribe(CharDataEventArgs.EventId, CharDataEvent);
            GameEntry.Utils.UpdateData();
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);
            GameEntry.Event.Unsubscribe(CharDataEventArgs.EventId, CharDataEvent);
        }

        private void PlayerDataEvent(object sender, GameEventArgs e)
        {
            PlayerDataEventArgs playerDataEvent = (PlayerDataEventArgs)e;
            PlayerData playerData = playerDataEvent.PlayerData;
            //rentText.transform.parent.gameObject.SetActive(GameEntry.Utils.Rent != 0);
            //rentText.text = string.Format("距离下一次欠款缴纳还有{0}天\r\n下一次交纳欠款：{1}",6-(playerData.day + 20) % 7, GameEntry.Utils.Rent.ToString());
            energyText.text = string.Format("{0}/{1}", playerData.energy, playerData.maxEnergy);
            moneyText.text = string.Format("{0}", playerData.money.ToString());
            //timeText.text = string.Format("{0}月{1}日 星期{2}", (4 + (playerData.day + 19) / 28) % 12 + 1, (playerData.day + 19) % 28 + 1, AssetUtility.GetWeekCN((playerData.day + 20) % 7));
        }
        private void CharDataEvent(object sender, GameEventArgs e)
        {
            CharDataEventArgs charDataEvent = (CharDataEventArgs)e;
            CharData charData = charDataEvent.CharData;
            //favorText.text = charData.favor.ToString();
        }
    }
}
