using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class LuaModule : BussinessModule
{
	public LuaModule(string name):base(name)
	{

	}
	public override void Create(object arg = null)
	{
		base.Create(arg);
	}
	public override void Release()
	{
		base.Release();
	}
}
