using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(UIEventListener))]
public class NButtonScale : MonoBehaviour
{
    public float duration = 0.05f;
    public Vector3 pressed = new Vector3(0.9f, 0.9f, 0.9f);
    Vector3 mScale = Vector3.one;
    private void Start()
    {
        mScale = gameObject.transform.localScale;
    }
    public void OnPress(bool isPressed)
    {
        TweenScale.Begin(gameObject, duration/2, isPressed ? Vector3.Scale(mScale, pressed) : mScale).method = UITweener.Method.EaseInOut;
    }
}
