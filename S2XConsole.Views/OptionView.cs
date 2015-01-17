using S2XConsole.Interface;
using S2XConsole.Models;
using S2XConsole.ViewModels;
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
	public class OptionView : UserControl, IPage, IComponentConnector
	{
		private MainViewModel viewModel;
		private bool isCommunicationSettingsPresent;
		private bool isLoadSettingsPresent;
		internal CheckBox checkboxSettings;
		internal CheckBox checkboxDownloadLoc;
		internal CheckBox checkboxLicenses;
		internal CheckBox checkboxFormat;
		private bool _contentLoaded;
		public OptionView(MainViewModel mainViewModel)
		{
			this.InitializeComponent();
			this.viewModel = mainViewModel;
			if (Common.IsInStandaloneMode)
			{
				this.checkboxDownloadLoc.Content = "Software Download Location";
				this.checkboxLicenses.Visibility = Visibility.Collapsed;
				this.checkboxDownloadLoc.ToolTip = this.checkboxDownloadLoc.Content;
				this.checkboxSettings.ToolTip = this.checkboxSettings.Content;
			}
			this.updateCheckboxAndroidEnabled();
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
		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			try
			{
				CheckBox checkBox = sender as CheckBox;
				UserControl control = null;
				int index = 1;
				string a;
				if ((a = checkBox.Tag.ToString()) != null)
				{
					if (!(a == "CommunicationSettings"))
					{
						if (!(a == "LoadSettings"))
						{
							if (a == "Licenses")
							{
								control = new LicenseView();
								if (this.isCommunicationSettingsPresent && this.isLoadSettingsPresent)
								{
									index = 3;
								}
								else
								{
									if (this.isCommunicationSettingsPresent || this.isLoadSettingsPresent)
									{
										index = 2;
									}
									else
									{
										index = 1;
									}
								}
							}
						}
						else
						{
							control = new LoadSettingView();
							this.isLoadSettingsPresent = true;
							if (this.isCommunicationSettingsPresent)
							{
								index = 2;
							}
							else
							{
								index = 1;
							}
							this.updateCheckboxAndroidEnabled();
						}
					}
					else
					{
						if (Common.IsInStandaloneMode)
						{
							control = new StandaloneBrowse();
						}
						else
						{
							control = new CommunicationSettingsView();
						}
						this.isCommunicationSettingsPresent = true;
						index = 1;
						this.updateCheckboxAndroidEnabled();
					}
				}
				if (this.viewModel.TotalPages == 1)
				{
					this.viewModel.AddPage(new BarcodeOptionView(this.viewModel));
				}
				this.viewModel.AddPage(control, index);
			}
			catch (Exception)
			{
			}
		}
		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			try
			{
				CheckBox checkBox = sender as CheckBox;
				int index = 1;
				string a;
				if ((a = checkBox.Tag.ToString()) != null)
				{
					if (!(a == "CommunicationSettings"))
					{
						if (!(a == "LoadSettings"))
						{
							if (a == "Licenses")
							{
								if (this.isCommunicationSettingsPresent && this.isLoadSettingsPresent)
								{
									index = 3;
								}
								else
								{
									if (this.isCommunicationSettingsPresent || this.isLoadSettingsPresent)
									{
										index = 2;
									}
									else
									{
										index = 1;
									}
								}
							}
						}
						else
						{
							this.isLoadSettingsPresent = false;
							if (this.isCommunicationSettingsPresent)
							{
								index = 2;
							}
							else
							{
								index = 1;
							}
							this.updateCheckboxAndroidEnabled();
						}
					}
					else
					{
						this.isCommunicationSettingsPresent = false;
						Common.SettingsSourceName = null;
						index = 1;
						this.updateCheckboxAndroidEnabled();
					}
				}
				this.viewModel.RemovePage(index);
				if (this.viewModel.TotalPages == 2)
				{
					this.viewModel.RemovePage(this.viewModel.PageList.Last.Value);
				}
			}
			catch (Exception)
			{
			}
		}
		private void checkboxFormat_Checked(object sender, RoutedEventArgs e)
		{
			Common.UsingJson = true;
		}
		private void checkboxFormat_Unchecked(object sender, RoutedEventArgs e)
		{
			Common.UsingJson = false;
		}
		private void updateCheckboxAndroidEnabled()
		{
			if (Common.IsInStandaloneMode && !this.checkboxSettings.IsChecked.Value && this.checkboxDownloadLoc.IsChecked.Value)
			{
				this.checkboxFormat.Visibility = Visibility.Visible;
				return;
			}
			this.checkboxFormat.Visibility = Visibility.Hidden;
		}
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/S2XConsole;component/views/optionview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
		[EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.checkboxSettings = (CheckBox)target;
				this.checkboxSettings.Checked += new RoutedEventHandler(this.CheckBox_Checked);
				this.checkboxSettings.Unchecked += new RoutedEventHandler(this.CheckBox_Unchecked);
				return;
			case 2:
				this.checkboxDownloadLoc = (CheckBox)target;
				this.checkboxDownloadLoc.Checked += new RoutedEventHandler(this.CheckBox_Checked);
				this.checkboxDownloadLoc.Unchecked += new RoutedEventHandler(this.CheckBox_Unchecked);
				return;
			case 3:
				this.checkboxLicenses = (CheckBox)target;
				this.checkboxLicenses.Checked += new RoutedEventHandler(this.CheckBox_Checked);
				this.checkboxLicenses.Unchecked += new RoutedEventHandler(this.CheckBox_Unchecked);
				return;
			case 4:
				this.checkboxFormat = (CheckBox)target;
				this.checkboxFormat.Checked += new RoutedEventHandler(this.checkboxFormat_Checked);
				this.checkboxFormat.Unchecked += new RoutedEventHandler(this.checkboxFormat_Unchecked);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}
	}
}
