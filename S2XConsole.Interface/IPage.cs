using System;
namespace S2XConsole.Interface
{
	internal interface IPage
	{
		void Next();
		string PageData();
		void Enter();
		int Version();
		void Close();
	}
}
