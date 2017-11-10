using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Manager : MonoBehaviour, IManager
{
    public virtual void OnManagerDestroy()
    {

    }
    public virtual void OnManagerReady()
    {

    }
    private void OnDestroy()
    {
        OnManagerDestroy();
    }
    private void Awake()
    {
        OnManagerReady();
    }
}