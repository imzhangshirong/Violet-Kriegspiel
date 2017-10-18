using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AutoPanelDepth : MonoBehaviour
{
    public UIPanel rootPanel;
    private void Start()
    {
        UIPanel panel = gameObject.GetComponent<UIPanel>();
        panel.depth += rootPanel.depth;
    }
}
