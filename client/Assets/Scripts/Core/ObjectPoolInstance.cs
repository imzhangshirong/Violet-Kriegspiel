using UnityEngine;
using System.Collections;

public class ObjectPoolInstance : MonoBehaviour ,IObjectPoolInstance
{
    public virtual void OnPoolInstantiated()
    {
        //throw new System.NotImplementedException();
    }

    public virtual void OnPoolRelease()
    {
        //throw new System.NotImplementedException();
    }
}
