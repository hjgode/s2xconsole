using S2XConsole.Interface;
using S2XConsole.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
namespace S2XConsole.Views
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public class CommunicationSettingsView : UserControl, IPage, IComponentConnector
	{
		private static object lockObject = new object();
		private CommunicationViewModel viewModel;
		internal ListView grid;
		internal Label NoBackupLabel;
		private bool _contentLoaded;
		public CommunicationSettingsView()
		{
			this.InitializeComponent();
			ThreadStart start = delegate
			{
				lock (CommunicationSettingsView.lockObject)
				{
					this.viewModel = new CommunicationViewModel();
                    //base.Dispatcher.Invoke(delegate
                    //{
						base.DataContext = this.viewModel;
						if (this.viewModel.CommunicationSettings.Count > 0)
						{
							this.NoBackupLabel.Content = string.Empty;
							return;
						}
						this.NoBackupLabel.Content = "No backup settings found";
                    //}, DispatcherPriority.Send, new object[0]);
				}
			};
			Thread thread = new Thread(start);
			thread.Start();
		}
		public void Next()
		{
		}
		public void Enter()
		{
		}
		public string PageData()
		{
			string result;
			lock (CommunicationSettingsView.lockObject)
			{
				result = this.viewModel.PageData();
			}
			return result;
		}
		public int Version()
		{
			int result;
			lock (CommunicationSettingsView.lockObject)
			{
				result = this.viewModel.Version();
			}
			return result;
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
			Uri resourceLocator = new Uri("/S2XConsole;component/views/communicationsettingsview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
		[EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.grid = (ListView)target;
				return;
			case 2:
				this.NoBackupLabel = (Label)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}
	}
}
