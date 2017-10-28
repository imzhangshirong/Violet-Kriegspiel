using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//配合UIRoot PixelPerfect使用，minHeight和maxHeight设为和baseHeight相同，不勾选Shrink
public class ScreenFixed : MonoBehaviour
{
    public float baseWidth = 1280;
    public float baseHeight = 720;
    public Orientation mode = Orientation.Horizontal;
    public enum Orientation
    {
        Vertical,
        Horizontal
    }
    public bool always = false;
    private void Fixed()
    {
        float h = (float)Screen.height;
        float w = (float)Screen.width;
        float height = h;
        Camera camera = Camera.current;
        if (camera != null && camera.orthographic)
        {
            if (mode == Orientation.Vertical)
            {
                if ((float)h / w > baseHeight / baseWidth)
                {

                    height = baseHeight / baseWidth * w;//基于宽度
                }
                else//NGUI已适配，还原成1
                {
                    height = h;
                }
            }
            else//NGUI原来是基于高度适配的，还原成1
            {
                height = h;
            }
            camera.orthographicSize = h / height;
        }
        
    }
    void Start()
    {
        Fixed();
    }
    private void FixedUpdate()
    {
        if(always)Fixed();
    }
}