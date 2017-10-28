using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class RichTextItem
{
    public Rect area;
    public float scale;
    public string sequence;
    public string spriteName;
    public float baseLine;
    public float textLineHeight;
    public int type;
}


[Serializable]
public class RichTextPrefabItem
{
    public string sequence;
    public float width;
    public float height;
    public GameObject prefabObject;
}

[Serializable]
public class RichTextEmotionItem
{
    public string sequence;
    public string spriteName;
}
