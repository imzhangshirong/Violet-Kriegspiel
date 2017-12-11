using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(UIEventListener))]
public class NClickClose : MonoBehaviour
{
    public GameObject closeTarget;
    public void OnClick()
    {
        closeTarget.SetActive(false);
    }
}