using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class MusicHallChangePoster : MonoBehaviour
    {
        // Start is called before the first frame update
        private int itemId;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(GameEntry.Utils.musicHallItemID == 0)
            {
                DrawLots();
            }
            if ((((GameEntry.Utils.PlayerData.day + 20) %7) == 0 )&& GameEntry.Utils.musicChangeFlag== true)
            {
                DrawLots();
            }
            if (((GameEntry.Utils.PlayerData.day + 20) %7) != 0 )
            {
                GameEntry.Utils.musicChangeFlag = true;
            }
        }

        private void DrawLots()
        {
            itemId = Random.Range(40, 43);
            while (itemId == GameEntry.Utils.changeMusicHallItemID)
            {
                itemId = Random.Range(40, 43);
            }
            GameEntry.Utils.changeMusicHallItemID = itemId;
            GameEntry.Utils.musicHallItemID = itemId;
            GameEntry.Utils.musicChangeFlag = false;
        }
    }
}

