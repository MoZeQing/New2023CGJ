using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharSO : ScriptableObject
{
    public bool isMain;
    public Vector3 offset;
    public float scale=1f;
    public List<Sprite> diffs = new List<Sprite>();
    public Sprite sprite;
    public string charName;
    public int favorCoffee;
    public string text;
}



