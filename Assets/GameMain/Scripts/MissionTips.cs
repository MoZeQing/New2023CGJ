using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class MissionTips : MonoBehaviour
    {
        [SerializeField] private Text missionText;

        private void Update()
        {
            if (GameEntry.Utils.Day > 14)
            {
                missionText.text = string.Format("想要举办沙龙，你需要邀请足够多的好友，尽可能在沙龙前将好友们的好感度提升到100！\r\n<size=24><color=red>好友的好感度会提升你的工作能力</color></size>\r\n", 35 - GameEntry.Utils.Day);
            }
            else if (GameEntry.Utils.Day < 7)
            {
                missionText.text = string.Format("想办法取得爱丽丝的信任，\n<size=28><color=red>在这周六前将爱丽丝的好感度提升到10以上吧</color></size>");
            }
            else
            { 
                missionText.text= string.Format("<size=28><color=red>尽可能的和咖啡店的老顾客们熟络感情吧！</color></size>\n你可以点击好友按钮来了解他们的好感度");
            }
        }
    }
}

