using CommonAboutDialog;
using S2XConsole.Interface;
using S2XConsole.Models;
using S2XConsole.Properties;
using S2XConsole.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace S2XConsole.Views
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public class MainView : Window, IComponentConnector
	{
		private MainViewModel viewModel;
		private FormAbout frmAbout;
		internal MenuItem menuItemHelp;
		private bool _contentLoaded;
		public MainView(string[] args)
		{
			this.viewModel = new MainViewModel(args);
			if (!this.viewModel.IsBrowseMode)
			{
				this.viewModel.AddPage(new OptionView(this.viewModel));
			}
			else
			{
				this.viewModel.AddPage(new BarcodeOptionView(this.viewModel));
			}
			this.viewModel.CurrentPage = this.viewModel.PageList.First.Value;
			base.DataContext = this.viewModel;
			this.InitializeComponent();
			if (Common.IsInStandaloneMode)
			{
				this.menuItemHelp.Header = "User Guide";
			}
		}
		private void Window_Closed(object sender, EventArgs e)
		{
			using (LinkedList<UserControl>.Enumerator enumerator = this.viewModel.PageList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IPage page = (IPage)enumerator.Current;
					page.Close();
				}
			}
			Settings.Default.Save();
		}
		private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
			e.Handled = true;
		}
		private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (Common.IsInStandaloneMode)
			{
				this.viewModel.ShowHelpHtml();
				return;
			}
			this.viewModel.ShowHelpChm();
		}
		private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
		{
			this.getAboutBox().ShowDialog();
		}
		private FormAbout getAboutBox()
		{
			if (this.frmAbout == null || this.frmAbout.IsDisposed)
			{
				this.frmAbout = new FormAbout();
				this.frmAbout.Product = "ScanNGo";
				this.frmAbout.HideVersionsButton = true;
				this.frmAbout.HideUserLevel = true;
			}
			return this.frmAbout;
		}
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/S2XConsole;component/views/mainview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
		[DebuggerNonUserCode]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}
		[EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((MainView)target).Closed += new EventHandler(this.Window_Closed);
				return;
			case 2:
				((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(this.CanExecute);
				((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(this.HelpExecuted);
				return;
			case 3:
				this.menuItemHelp = (MenuItem)target;
				return;
			case 4:
				((MenuItem)target).Click += new RoutedEventHandler(this.MenuItemAbout_Click);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}
	}
}
