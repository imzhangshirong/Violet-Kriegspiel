using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

public class MEvent : UnityEvent<object>
{
}

public class MEvent<T> : UnityEvent<T>
{
}