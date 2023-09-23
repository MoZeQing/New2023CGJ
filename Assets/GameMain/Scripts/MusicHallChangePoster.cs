using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class MusicHallChangePoster : MonoBehaviour
    {
        // Start is called before the first frame update
        private bool flag = true;
        private int itemId;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (GameEntry.Utils.Week == Week.Monday && flag == true)
            {
                DrawLots();
                flag = false;
            }
            if (GameEntry.Utils.Week != Week.Monday)
            {
                flag = true;
            }
        }

        private void DrawLots()
        {
            itemId = Random.Range(15, 18);
            while (itemId == GameEntry.Utils.changeMusicHallItemID)
            {
                itemId = Random.Range(15, 18);
            }
            GameEntry.Utils.changeMusicHallItemID = itemId;
            GameEntry.Utils.musicHallItemID = itemId;
        }
    }
}

