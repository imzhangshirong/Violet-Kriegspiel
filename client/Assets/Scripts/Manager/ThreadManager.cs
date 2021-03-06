﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

//线程管理
public class ThreadManager : Manager
{
    public const string Name = "ThreadManager";
    static int numThreads;

    private static List<Thread> m_TreadList = new List<Thread>();

    private static ThreadManager _current;
    public static ThreadManager Current
    {
        get {
            return _current;
        }
    }
    private int _count;
    

    
    public override void OnManagerReady()
    {
        _current = this;
    }
    public override void OnManagerDestroy()
    {
        //自动销毁所有工作中的线程
        for (int i = m_TreadList.Count - 1; i >= 0; i--)
        {
            try{
                m_TreadList[i].Abort();
            }
            catch(Exception e){

            }
        }
        m_TreadList.Clear();
    }

    private List<Action> _actions = new List<Action>();
    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

    List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

    public void RunOnMainThread(Action action)
    {
        RunOnMainThread(action, 0f);
    }
    public void RunOnMainThread(Action action, float time)
    {
        if (time != 0)
        {
            lock (Current._delayed)
            {
                Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
            }
        }
        else
        {
            lock (Current._actions)
            {
                Current._actions.Add(action);
            }
        }
    }

    class Worker
    {
        private Action action;
        public Worker(Action a)
        {
            action = a;
        }
        public void Run()
        {
            action();
        }
    }

    public Thread RunAsync(Action a)
    {
        for(int i = m_TreadList.Count - 1; i >= 0; i--)
        {
            if (!m_TreadList[i].IsAlive)
            {
                m_TreadList.RemoveAt(i);
            }
        }
        Worker worker = new Worker(a);
        Thread workerThread = new Thread(worker.Run);
        m_TreadList.Add(workerThread);
        workerThread.Start();
        return workerThread;
    }
    


    

    List<Action> _currentActions = new List<Action>();

    // Update is called once per frame
    void Update()
    {
        lock (_actions)
        {
            _currentActions.Clear();
            _currentActions.AddRange(_actions);
            _actions.Clear();
        }
        foreach (var a in _currentActions)
        {
            a();
        }
        lock (_delayed)
        {
            _currentDelayed.Clear();
            _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
            foreach (var item in _currentDelayed)
                _delayed.Remove(item);
        }
        foreach (var delayed in _currentDelayed)
        {
            delayed.action();
        }



    }

}