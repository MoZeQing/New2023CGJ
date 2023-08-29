using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;
using GameFramework.Event;
using System;
using DG.Tweening;

namespace GameMain
{
    public class WorkForm : MonoBehaviour
    {
        [SerializeField] private BaseStage stage;
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private Transform mCanvas;
        [SerializeField] private Transform recipeCanvas;
        [SerializeField] private Button upBtn;
        [SerializeField] private Button downBtn;
        [SerializeField] private Button recipeBtn;

        private void OnEnable()
        {
            upBtn.onClick.AddListener(()=>GamePosUtility.Instance.GamePosChange(GamePos.Up));
            downBtn.onClick.AddListener(() => GamePosUtility.Instance.GamePosChange(GamePos.Down));
            recipeBtn.onClick.AddListener(() => recipeCanvas.gameObject.SetActive(true));
        }

        private void OnDisable()
        {
            upBtn.onClick.RemoveAllListeners();
            downBtn.onClick.RemoveAllListeners();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                recipeCanvas.gameObject.SetActive(false);
            }
        }
    }
}
