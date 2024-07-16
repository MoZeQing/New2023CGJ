using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace GameMain
{
    public class RaceGameForm : BaseForm
    {
        private float score;
        [SerializeField] private float targetScore;
        [SerializeField] Text scoreTitle;

        [SerializeField] List<Transform> genNodes;
        [SerializeField] List<int> genNodeCount;
        [SerializeField] List<GameObject> barrierList;
        [SerializeField] float barrierGenInterval;//障碍生成间隔
        [Range(0.01f, 0)]
        [SerializeField] float BGIIncreaseRate;//障碍生成间隔增长率
        private float currentBGI;
        [SerializeField] float barrierSpeed;//障碍速度
        private float currentBS;
        [Range(1, 0)]
        [SerializeField] float BSIncreaseRate;//障碍速度增长率
        private float m_genTimer;
        private bool m_done;
        private List<GameObject> barriers = new List<GameObject>();

        [SerializeField] GameObject player;
        private int m_playerPosition;

        //private Action mAction;
        [SerializeField] private CharData charData;
        [SerializeField] private PlayerData playerData;

        [SerializeField] private Button startBtn;
        private bool m_start;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //mAction = BaseFormData.Action;

            startBtn.onClick.AddListener(() => m_start = true);
            m_start = false;

            currentBGI = barrierGenInterval;
            currentBS = barrierSpeed;
            m_genTimer = currentBGI;

            foreach (Transform t in genNodes)
            {
                foreach(Transform child in t)
                {
                    Destroy(child.gameObject);
                }
            }

            m_playerPosition = 1;
            for (int i = 0; i < genNodes.Count; i++)
                genNodeCount.Add(0);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            startBtn.onClick.RemoveAllListeners();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (!m_start)
                return;

            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if(m_done)
            {
                Dictionary<ValueTag, int> dic = new Dictionary<ValueTag, int>();
                float power = score / targetScore;
                int stamina = (int)(charData.stamina * power);
                dic.Add(ValueTag.Stamina, stamina);
                playerData.GetValueTag(dic);

                GameEntry.UI.OpenUIForm(UIFormId.CompleteForm, OnExit, dic);
                return;
            }

            scoreTitle.text = ((int)score).ToString();
            if (!m_done)
                score += (1 + currentBS / 5) * Time.deltaTime;

            m_genTimer -= Time.deltaTime;
            if (currentBS <= 30)
                currentBS += Time.deltaTime * BSIncreaseRate;

            if (currentBGI >= 0.2f)
                currentBGI -= Time.deltaTime * BGIIncreaseRate;


            if (m_genTimer <= 0 && !m_done)
            {
                var nodeIndex = GenNodeIndex();

                var node = genNodes[nodeIndex];
                var barrier = barrierList[UnityEngine.Random.Range(0, barrierList.Count)];

                var go = Instantiate(barrier, node);
                barriers.Add(go);
                go.GetComponent<Rigidbody2D>().velocity = new Vector3(-currentBS * 100, 0, 0);

                m_genTimer = currentBGI;
            }

            MoveVertical();
            if (PlayerCollisionResult() > 0)
            {
                m_done = true;
                foreach (GameObject barrier in barriers)
                {
                    DOTween.To(() => barrier.GetComponent<Rigidbody2D>().velocity, x => barrier.GetComponent<Rigidbody2D>().velocity = x, Vector2.zero, 2f).SetEase(Ease.OutQuad);
                }
                Debug.Log("GameOver");
            }
        }
       
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(collision.gameObject);
        }

        private void MoveVertical()
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                if (m_playerPosition > 0)
                    m_playerPosition--;
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                if(m_playerPosition < 2)
                    m_playerPosition++;
            }
            else { }

            player.transform.DOMoveY(genNodes[m_playerPosition].position.y, 0.2f, true);
        }

        private int GenNodeIndex()
        {
            int index;
            do
            {
                index = UnityEngine.Random.Range(0, genNodes.Count);
                genNodeCount[index]++;
            } while (genNodeCount[index] > 2);

            for(int i = 0; i < genNodeCount.Count; i++)
            {
                if (i != index)
                    genNodeCount[i] = 0;
            }

            return index;
        }

        private int PlayerCollisionResult()
        {
            ContactFilter2D filter2D = new ContactFilter2D();
            filter2D.NoFilter();
            List<Collider2D> result = new List<Collider2D>();
            player.GetComponent<BoxCollider2D>()?.OverlapCollider(filter2D, result);
            return result.Count;
        }

        private void OnExit()
        {
            //mAction();
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}


