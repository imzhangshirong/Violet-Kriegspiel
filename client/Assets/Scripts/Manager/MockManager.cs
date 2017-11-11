using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Google.Protobuf;


public class MockManager : Manager
{
    public const string Name = "MockManager";
    static Dictionary<KeyCode, Action> m_mockActionMap = new Dictionary<KeyCode, Action>();

    public override void OnManagerReady()
    {

    }
    public override void OnManagerDestroy()
    {

    }
    public void RegisteMockAction(KeyCode keyCode,Action action)
    {
        if (!m_mockActionMap.ContainsKey(keyCode))
            m_mockActionMap.Add(keyCode, action);
        else
            m_mockActionMap[keyCode] = action;
    }
    public void MockPush(string rpcName, IMessage data)
    {
        App.NetworkManager.DispatchPush(rpcName, data);
    }
    private void OnGUI()
    {
        Event ev = Event.current;
        if (ev.isKey && Input.anyKeyDown && ev.keyCode != KeyCode.None)
        {
            if (m_mockActionMap.ContainsKey(ev.keyCode)) m_mockActionMap[ev.keyCode]();
        }
    }
    /// <summary>
    /// mock的操作
    /// </summary>
    #region
    void _EnemyStateReady()
    {

    }
    #endregion
}
