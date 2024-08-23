using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class SceneNode : MonoBehaviour
    {
        public NodeTag nodeTag;
        // Start is called before the first frame update
        void Start()
        {
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, nodeTag)
            {
                Scale = this.transform.localScale,
                Position= this.transform.position
            });
            Destroy(this.gameObject);
        }
    }
}
