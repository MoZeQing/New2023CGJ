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
            switch (GameEntry.Player.Day)
            {
                case 2:
                case 3:
                    this.gameObject.SetActive(true);
                    missionText.text = "现在先培养爱丽丝吧！";
                    break;
                case 4:
                    this.gameObject.SetActive(true);
                    missionText.text = "<color=red>外出</color>去服装店拜访露希尔吧";
                    break;
                case 5:
                case 6:
                case 7:
                    this.gameObject.SetActive(true);
                    if (GameEntry.Player.Money<500)
                        missionText.text = "准备<color=red>500</color>元付房租吧！";
                    else
                        missionText.text = "准备500元付房租吧！";
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                    this.gameObject.SetActive(true);
                    if (GameEntry.Player.Money < 500)
                        missionText.text = "准备<color=red>800</color>元付房租吧！";
                    else
                        missionText.text = "准备800元付房租吧！";
                    break;
                case 12:
                case 13:
                case 14:
                case 15:
                    this.gameObject.SetActive(true);
                    if (GameEntry.Player.Money < 500)
                        missionText.text = "准备<color=red>1100</color>元付房租吧！";
                    else
                        missionText.text = "准备1100元付房租吧！";
                    break;
                case 16:
                case 17:
                case 18:
                case 19:
                    this.gameObject.SetActive(true);
                    if (GameEntry.Player.Money < 500)
                        missionText.text = "准备<color=red>1500</color>元付房租吧！";
                    else
                        missionText.text = "准备1500元付房租吧！";
                    break;
                default:
                    this.gameObject.SetActive(false);
                    break;
            }
        }
    }
}

