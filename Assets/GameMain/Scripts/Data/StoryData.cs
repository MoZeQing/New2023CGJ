using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryData
{
    public string storyName;
    public bool isRemove;
    public OutingSceneState outingSceneState;
    public GameState gameState;
    public ParentTrigger trigger;
    public string dialogName;
    public List<EventData> eventDatas = new List<EventData>();

    public StoryData() { }

    public StoryData(DRStory story)
    {
        storyName = story.StoryName;
        outingSceneState = (OutingSceneState)story.OutingSceneState;
        gameState=(GameState)story.GameState;
        trigger = new ParentTrigger(story.Trigger);
        dialogName = story.DialogName;
        if (!string.IsNullOrEmpty(story.EventText))
        {
            string[] strings = story.EventText.Split('-');
            foreach (string text in strings)
            {
                eventDatas.Add(new EventData(text));
            }
        }

    }

    public StoryData(StorySO story)
    {
        storyName = story.name;
        outingSceneState= story.outingSceneState;
        gameState = story.gameState;
        trigger = story.trigger;
        dialogName = story.dialogueGraph?.name;
        eventDatas=story.eventDatas;
    }
}
