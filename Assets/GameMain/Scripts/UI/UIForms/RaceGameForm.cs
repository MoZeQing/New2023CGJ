using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RaceGameForm : MonoBehaviour
{
    [SerializeField] float score;
    [SerializeField] Text scoreTitle;

    [SerializeField] List<Transform> genNodes;
    [SerializeField] List<int> genNodeCount;
    [SerializeField] List<GameObject> barrierList;
    [SerializeField] float barrierGenInterval;//障碍生成间隔
    [Range(0.01f, 0)]
    [SerializeField] float BGIIncreaseRate;//障碍生成间隔增长率
    [SerializeField] float barrierSpeed;//障碍速度
    [Range(1, 0)]
    [SerializeField] float BSIncreaseRate;//障碍速度增长率
    private float m_genTimer;
    private bool m_done;
    private List<GameObject> barriers = new List<GameObject>();

    [SerializeField] GameObject player;
    private int m_playerPosition;

    private void Start()
    {
        m_genTimer = 0;
        m_playerPosition = 1;
        for(int i = 0; i < genNodes.Count; i++)
            genNodeCount.Add(0);
    }

    private void Update()
    {
        scoreTitle.text = ((int)score).ToString();
        if(!m_done)
            score += (1 + barrierSpeed / 5) * Time.deltaTime;

        m_genTimer -= Time.deltaTime;
        if(barrierSpeed <= 30)
            barrierSpeed += Time.deltaTime * BSIncreaseRate;

        if(barrierGenInterval >= 0.2f)
            barrierGenInterval -= Time.deltaTime * BGIIncreaseRate;
        

        if(m_genTimer <= 0 && !m_done)
        {
            var nodeIndex = GenNodeIndex();

            var node = genNodes[nodeIndex];
            var barrier = barrierList[Random.Range(0, barrierList.Count)];

            var go = Instantiate(barrier, node);
            barriers.Add(go);
            go.GetComponent<Rigidbody2D>().velocity = new Vector3(-barrierSpeed * 100, 0, 0);

            m_genTimer = barrierGenInterval;
        }

        MoveVertical();
        if(PlayerCollisionResult() > 0)
        {
            m_done = true;
            foreach(GameObject barrier in barriers)
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
            index = Random.Range(0, genNodes.Count);
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
}
