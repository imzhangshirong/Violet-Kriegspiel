using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IPackage
{
    void Init(object data);
    void Release();
}
