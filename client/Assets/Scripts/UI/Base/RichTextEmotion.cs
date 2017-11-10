using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RichTextEmotion : MonoBehaviour
{
    public float tickTime = 0.2f;
    UISprite mSprite;
    Dictionary<int, string> mDict = new Dictionary<int, string>();
    int max = 0;
    int cur = 0;
    void Start()
    {
        mSprite = GetComponent<UISprite>();
        List<UISpriteData> all = mSprite.atlas.spriteList;
        mDict.Add(0, mSprite.spriteName);
        int length = mSprite.spriteName.Length;
        for (int i = 0; i < all.Count; i++)
        {
            if (all[i].name.Length <= length) continue;
            if (all[i].name.Substring(0, length) == mSprite.spriteName)
            {
                string num = all[i].name.Substring(length, all[i].name.Length - length);
                int numi = 0;
                Int32.TryParse(num, out numi);
                if (numi > 0)
                {
                    if (numi > max) max = numi;
                    if (!mDict.ContainsKey(numi))
                    {
                        mDict.Add(numi, all[i].name);
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        StartCoroutine(ChangeSprite());
    }

    IEnumerator ChangeSprite()
    {
        while (isActiveAndEnabled)
        {
            cur++;
            cur %= (max + 1);
            if (mDict.ContainsKey(cur))
            {
                mSprite.spriteName = mDict[cur];
            }
            yield return new WaitForSeconds(tickTime);
        }
    }

}
