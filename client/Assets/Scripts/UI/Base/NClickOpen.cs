using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(UIEventListener))]
public class NClickOpen : MonoBehaviour
{
    public GameObject openTarget;
    public void OnClick()
    {
        openTarget.SetActive(true);
    }
}