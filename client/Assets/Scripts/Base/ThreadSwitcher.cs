using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

//base on Loom
public class ThreadSwitcher : MonoBehaviour
{
    static int numThreads;

    private static ThreadSwitcher _current;
    private int _count;
    public static ThreadSwitcher Current
    {
        get
        {
            Init();
            return _current;
        }
    }

    void Awake()
    {
        _current = this;
        initialized = true;
    }

    static bool initialized;

    public static void Init()
    {
        if (!initialized)
        {
            if (!Application.isPlaying)
                return;
            initialized = true;
            var g = new GameObject("ThreadSwitcher");
            _current = g.AddComponent<ThreadSwitcher>();
        }

    }

    private List<Action> _actions = new List<Action>();
    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

    List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

    public static void RunOnMainThread(Action action)
    {
        RunOnMainThread(action, 0f);
    }
    public static void RunOnMainThread(Action action, float time)
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

    class Worker {
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

    public static Thread RunAsync(Action a)
    {
        Worker worker = new Worker(a);
        Thread workerThread = new Thread(worker.Run);
        Init();
        workerThread.Start();
        return workerThread;
    }

    void OnDisable()
    {
        if (_current == this)
        {

            _current = null;
        }
    }



    // Use this for initialization
    void Start()
    {

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