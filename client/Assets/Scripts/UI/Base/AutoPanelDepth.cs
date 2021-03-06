﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AutoPanelDepth : MonoBehaviour
{
    [HideInInspector]
    public int offsetDepth = -1;
    private bool init = false;
    UIPanel panel;
    public UIPanel rootPanel;
    private void Awake()
    {
        if (init) return;
        panel = gameObject.GetComponent<UIPanel>();
        offsetDepth = panel.depth;
        init = true;
    }
    private void Start()
    {
        
        AutoDepth();
    }
    private void OnEnable()
    {
        AutoDepth();
    }
    void AutoDepth()
    {
        panel.depth = offsetDepth + rootPanel.depth;
    }
}
