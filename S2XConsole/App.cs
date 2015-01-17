using S2XConsole.Models;
using S2XConsole.Views;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
namespace S2XConsole
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public class App : Application
	{
		private void OnStartup(object sender, StartupEventArgs e)
		{
			try
			{
				MainView mainView = new MainView(e.Args);
				mainView.Show();
			}
			catch (Exception ex)
			{
				Common.WriteEntryToLog(ex.ToString(), EventLogEntryType.Error);
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			base.Startup += new StartupEventHandler(this.OnStartup);
		}
		[DebuggerNonUserCode, STAThread]
		public static void Main()
		{
			App app = new App();
			app.InitializeComponent();
			app.Run();
		}
	}
}
