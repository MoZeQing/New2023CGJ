using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingItem : MonoBehaviour
{
    private BuildingTag m_BuildingTag;
    private BuildingForm m_BuildingForm;

    public void OnInit(BuildingForm buildingForm,BuildingTag buildingTag)
    { 
        m_BuildingForm= buildingForm;
        m_BuildingTag= buildingTag;
        this.GetComponent<Button>().onClick.AddListener(() => m_BuildingForm.SetBuilding(buildingTag));
    }
}
