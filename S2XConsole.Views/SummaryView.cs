using S2XConsole.Interface;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace S2XConsole.Views
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public class SummaryView : UserControl, IPage, IComponentConnector
	{
		private bool _contentLoaded;
		public SummaryView()
		{
			this.InitializeComponent();
		}
		public void Next()
		{
		}
		public void Enter()
		{
		}
		public string PageData()
		{
			return string.Empty;
		}
		public int Version()
		{
			return 1;
		}
		public void Close()
		{
		}
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/S2XConsole;component/views/summaryview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
		[EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}
	}
}
