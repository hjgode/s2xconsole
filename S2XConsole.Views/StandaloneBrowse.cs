using Microsoft.Win32;
using S2XConsole.Interface;
using S2XConsole.Models;
using S2XConsole.Properties;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace S2XConsole.Views
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public class StandaloneBrowse : UserControl, IPage, IComponentConnector
	{
		private string fileData;
		internal TextBox textBoxConfig;
		internal Button buttonBrowse;
		internal Label label1;
		private bool _contentLoaded;
		public StandaloneBrowse()
		{
			this.InitializeComponent();
		}
		private void buttonBrowse_Click(object sender, RoutedEventArgs e)
		{
			string browseData = this.GetBrowseData();
			if (browseData != null)
			{
				this.fileData = browseData;
			}
		}
		private string GetBrowseData()
		{
            System.Diagnostics.Debug.WriteLine("StandAloneBrowse::GetBrowseData()");
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.DefaultExt = ".json";
			openFileDialog.Filter = "Config files (*.xml;*.json;*.config;*.config_result)|*.xml;*.json;*.config;*.config_result|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == true)
			{
				if (!string.IsNullOrEmpty(openFileDialog.FileName))
				{
					try
					{
						string text = File.ReadAllText(openFileDialog.FileName);
						bool flag = Common.IsJsonString(text);
						bool flag2 = Common.IsXmlString(text);
						string result;
						if (flag || flag2)
						{
							if (flag)
							{
								if (!Common.IsJsonActionValid(text))
								{
									MessageBox.Show("This JSON file is not valid for bar code generation since the action value is not \"set\"", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
									result = null;
									return result;
								}
								Common.UsingJson = true;
								text = Common.TransformJsonText(text);
							}
							else
							{
								Common.UsingJson = false;
								text = Common.PrepareXmlContent(text);
							}
							this.textBoxConfig.Text = openFileDialog.FileName;
							FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
							Common.SettingsSourceName = fileInfo.Name;
							result = text;
							return result;
						}
						MessageBox.Show("The file is not a valid JSON or XML file.", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
						result = null;
						return result;
					}
					catch (Exception)
					{
						MessageBox.Show("The file could not be read.", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
						string result = null;
						return result;
					}
				}
				MessageBox.Show("No file selected.", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return null;
			}
			return null;
		}
		public void Next()
		{
		}
		public string PageData()
		{
			return this.fileData;
		}
		public void Enter()
		{
		}
		public int Version()
		{
			if (Common.UsingJson)
			{
				return 4;
			}
			return 3;
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
			Uri resourceLocator = new Uri("/S2XConsole;component/views/standalonebrowse.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
		[EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.textBoxConfig = (TextBox)target;
				return;
			case 2:
				this.buttonBrowse = (Button)target;
				this.buttonBrowse.Click += new RoutedEventHandler(this.buttonBrowse_Click);
				return;
			case 3:
				this.label1 = (Label)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}
	}
}
