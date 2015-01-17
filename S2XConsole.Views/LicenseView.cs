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
namespace S2XConsole.Views
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public class LicenseView : UserControl, IPage, IComponentConnector
	{
		private LicenseViewModel viewModel;
		internal Label NoLicensesLabel;
		private bool _contentLoaded;
		public LicenseView()
		{
			this.InitializeComponent();
			ThreadStart start = delegate
			{
				this.viewModel = new LicenseViewModel();
                //base.Dispatcher.Invoke(delegate
                //{
					base.DataContext = this.viewModel;
					if (this.viewModel.LicenseSettings.Count > 0)
					{
						this.NoLicensesLabel.Content = string.Empty;
						return;
					}
					this.NoLicensesLabel.Content = "No Exported licenses found";
                //}, new object[0]);
			};
			new Thread(start).Start();
		}
		public void Next()
		{
		}
		public void Enter()
		{
		}
		public string PageData()
		{
			return this.viewModel.PageData();
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
			Uri resourceLocator = new Uri("/S2XConsole;component/views/licenseview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
		[EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				this.NoLicensesLabel = (Label)target;
				return;
			}
			this._contentLoaded = true;
		}
	}
}
