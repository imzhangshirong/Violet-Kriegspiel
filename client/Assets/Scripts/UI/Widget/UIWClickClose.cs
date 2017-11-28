using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
[RequireComponent(typeof(UIEventListener))]
public class UIWClickClose : MonoBehaviour
{
    public GameObject closeTarget;
    void OnClick(){
        closeTarget.SetActive(false);
    }
}