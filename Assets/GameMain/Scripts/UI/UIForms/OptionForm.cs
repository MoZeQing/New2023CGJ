using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace GameMain
{
    public class OptionForm : BaseForm
    {
        [SerializeField] private Button exit;
        [SerializeField] private Button main;
        [SerializeField] private Transform canvas;
        [SerializeField] private Transform staffCanvas;
        [SerializeField] private Button staffBtn;
        [SerializeField] private Button staffExitBtn;
        [SerializeField] private Scrollbar voiceScrollbar;
        [SerializeField] private Scrollbar wordScrollbar;
        [SerializeField] private Text voiceText;
        [SerializeField] private Text wordText;
        [SerializeField] private Text testText;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            staffCanvas.gameObject.SetActive(false);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
            voiceScrollbar.onValueChanged.AddListener(OnVoiceChanged);
            wordScrollbar.onValueChanged.AddListener(OnWordChanged);

            voiceText.text = $"{GameEntry.Utils.voice * 5}";
            wordText.text = $"{GameEntry.Utils.word * 10}";
            testText.text = string.Empty;
            testText.DOText($"²âÊÔÖÐ²âÊÔÖÐ²âÊÔÖÐ¡­¡­¡­¡­¡­¡­", GameEntry.Utils.word* "²âÊÔÖÐ²âÊÔÖÐ²âÊÔÖÐ¡­¡­¡­¡­¡­¡­".Length);

            exit.onClick.AddListener(()=> GameEntry.UI.CloseUIForm(this.UIForm));
            main.onClick.AddListener(() => 
            {
                GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Menu));
                GameEntry.UI.CloseUIForm(this.UIForm);
            });
            staffBtn.onClick.AddListener(() => staffCanvas.gameObject.SetActive(true));
            staffExitBtn.onClick.AddListener(() => staffCanvas.gameObject.SetActive(false));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            staffBtn.onClick.RemoveAllListeners();
            staffExitBtn.onClick.RemoveAllListeners();
            exit.onClick.RemoveAllListeners();
            main.onClick.RemoveAllListeners();
            voiceScrollbar.onValueChanged.RemoveAllListeners();
            wordScrollbar.onValueChanged.RemoveAllListeners();
        }

        private void OnVoiceChanged(float value)
        { 
            GameEntry.Utils.voice=value;
            voiceText.text = $"{value * 5}";
            GameEntry.Sound.SetVolume("BGM", GameEntry.Utils.voice);
            GameEntry.Sound.SetVolume("Sound", GameEntry.Utils.voice);
            GameEntry.Sound.SetVolume("Voice", GameEntry.Utils.voice);
            GameEntry.Sound.SetVolume("UI", GameEntry.Utils.voice);
        }

        private void OnWordChanged(float value)
        {
            GameEntry.Utils.word=0.05f-0.005f*value;
            wordText.text = $"{value * 10}";
            testText.text = string.Empty;
            testText.DOKill();
            testText.DOText($"²âÊÔÖÐ²âÊÔÖÐ²âÊÔÖÐ¡­¡­¡­¡­¡­¡­", GameEntry.Utils.word * "²âÊÔÖÐ²âÊÔÖÐ²âÊÔÖÐ¡­¡­¡­¡­¡­¡­".Length).OnComplete(()=> testText.text = string.Empty).SetLoops(-1);
        }
    }
}

