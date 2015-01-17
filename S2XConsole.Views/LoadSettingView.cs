using Microsoft.Win32;
using S2XConsole.Interface;
using S2XConsole.Models;
using S2XConsole.Properties;
using S2XConsole.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
namespace S2XConsole.Views
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public class LoadSettingView : UserControl, IPage, IComponentConnector
	{
		private bool ignoreTextBoxFocusLost;
		private LoadSettingViewModel viewModel = new LoadSettingViewModel();
		internal Label labelUrl;
		internal Label labelFolder;
		internal ComboBox comboBoxUrl;
		internal ComboBox comboBoxFolder;
		internal CheckBox checkBoxDestination;
		internal Label labelSoftwareDest;
		internal TextBox textboxSoftwareDest;
		internal Label labelTextFile;
		internal TextBox textboxTextFile;
		internal Button buttonBrowseTextFile;
		internal Label labelTextFileDest;
		internal TextBox textboxTextFileDest;
		internal Label labelOta;
		internal ComboBox comboBoxOta;
		internal Label labelLoadText;
		internal ComboBox comboBoxLoadText;
		internal Label labelDestTextFromUrl;
		internal TextBox textboxDestTextFileFromUrl;
		private bool _contentLoaded;
		public LoadSettingView()
		{
			base.DataContext = this.viewModel;
			this.InitializeComponent();
			if (Common.IsInStandaloneMode)
			{
				base.Tag = "Software Download Location";
				this.labelFolder.Visibility = Visibility.Collapsed;
				this.comboBoxFolder.Visibility = Visibility.Collapsed;
				this.labelUrl.ToolTip = this.labelUrl.Content;
				this.comboBoxUrl.ToolTip = this.labelUrl.Content;
			}
		}
		public void Next()
		{
			this.viewModel.UpdateSource(this.comboBoxUrl.Text, this.comboBoxFolder.Text, this.comboBoxOta.Text, this.comboBoxLoadText.Text);
		}
		public void Enter()
		{
			this.updateTextFileOptionsVisibility();
			this.updateOtaVisibility();
			this.updateTextFileFromUrlVisibility();
		}
		private void updateDestinationOptionsVisibility()
		{
			if (!Common.UsingJson)
			{
				this.checkBoxDestination.Visibility = Visibility.Collapsed;
				this.labelSoftwareDest.Visibility = Visibility.Collapsed;
				this.textboxSoftwareDest.Visibility = Visibility.Collapsed;
			}
			else
			{
				this.checkBoxDestination.Visibility = Visibility.Visible;
				this.labelSoftwareDest.Visibility = Visibility.Visible;
				this.textboxSoftwareDest.Visibility = Visibility.Visible;
			}
			this.updateSoftwareDestEnabled();
		}
		private void updateTextFileOptionsVisibility()
		{
			if (!Common.UsingJson)
			{
				this.labelTextFile.Visibility = Visibility.Collapsed;
				this.textboxTextFile.Visibility = Visibility.Collapsed;
				this.buttonBrowseTextFile.Visibility = Visibility.Collapsed;
				this.labelTextFileDest.Visibility = Visibility.Collapsed;
				this.textboxTextFileDest.Visibility = Visibility.Collapsed;
				return;
			}
			this.labelTextFile.Visibility = Visibility.Visible;
			this.textboxTextFile.Visibility = Visibility.Visible;
			this.buttonBrowseTextFile.Visibility = Visibility.Visible;
			this.labelTextFileDest.Visibility = Visibility.Visible;
			this.textboxTextFileDest.Visibility = Visibility.Visible;
		}
		private void updateOtaVisibility()
		{
			if (!Common.UsingJson)
			{
				this.labelOta.Visibility = Visibility.Collapsed;
				this.comboBoxOta.Visibility = Visibility.Collapsed;
				return;
			}
			this.labelOta.Visibility = Visibility.Visible;
			this.comboBoxOta.Visibility = Visibility.Visible;
		}
		private void updateTextFileFromUrlVisibility()
		{
			if (!Common.UsingJson)
			{
				this.labelLoadText.Visibility = Visibility.Collapsed;
				this.comboBoxLoadText.Visibility = Visibility.Collapsed;
				this.labelDestTextFromUrl.Visibility = Visibility.Collapsed;
				this.textboxDestTextFileFromUrl.Visibility = Visibility.Collapsed;
				return;
			}
			this.labelLoadText.Visibility = Visibility.Visible;
			this.comboBoxLoadText.Visibility = Visibility.Visible;
			this.labelDestTextFromUrl.Visibility = Visibility.Visible;
			this.textboxDestTextFileFromUrl.Visibility = Visibility.Visible;
		}
		public string PageData()
		{
			return this.viewModel.PageData();
		}
		public int Version()
		{
			string b = this.PageData();
			if (string.Empty == b)
			{
				return 1;
			}
			if (Common.UsingJson)
			{
				return 4;
			}
			return 2;
		}
		public void Close()
		{
		}
		private void checkBoxDestination_Checked(object sender, RoutedEventArgs e)
		{
			this.updateSoftwareDestEnabled();
		}
		private void checkBoxDestination_Unchecked(object sender, RoutedEventArgs e)
		{
			this.updateSoftwareDestEnabled();
		}
		private void updateSoftwareDestEnabled()
		{
			this.labelSoftwareDest.IsEnabled = this.checkBoxDestination.IsChecked.Value;
			this.textboxSoftwareDest.IsEnabled = this.checkBoxDestination.IsChecked.Value;
		}
		private void buttonBrowseTextFile_Click(object sender, RoutedEventArgs e)
		{
			this.handleBrowse();
		}
		private void handleBrowse()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.DefaultExt = ".txt";
			openFileDialog.Filter = "Text files (*.txt;*.ini)|*.txt;*.ini|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == true && !string.IsNullOrEmpty(openFileDialog.FileName))
			{
				try
				{
					string input = File.ReadAllText(openFileDialog.FileName);
					bool flag = Common.TestStringWithinJson(input);
					if (flag)
					{
						FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
						this.viewModel.TextFile = fileInfo.FullName;
						this.textboxTextFile.Text = fileInfo.FullName;
					}
					else
					{
						MessageBox.Show("The file is unsuitable for inclusion in JSON bar codes.  It may contain characters such as brackets that cause JSON parsing failure.", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
					}
				}
				catch (Exception)
				{
					MessageBox.Show("The file could not be read.", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
				}
			}
		}
		private void textboxDestTextFileFromUrl_TextChanged(object sender, TextChangedEventArgs e)
		{
			string text = this.textboxDestTextFileFromUrl.Text;
			if (text.StartsWith("\\") || text.StartsWith("/"))
			{
				int caretIndex = this.textboxDestTextFileFromUrl.CaretIndex;
				string messageBoxText = "The path may not start with a slash character.";
				MessageBox.Show(messageBoxText, "Invalid Starting Character", MessageBoxButton.OK, MessageBoxImage.Hand);
				string text2 = text.Substring(1);
				this.textboxDestTextFileFromUrl.Text = text2;
				this.textboxDestTextFileFromUrl.CaretIndex = caretIndex;
				return;
			}
			if (text.Contains("\\"))
			{
				int caretIndex2 = this.textboxDestTextFileFromUrl.CaretIndex;
				string messageBoxText2 = "The path may not contain the backslash character.  Please use forward slashes instead.";
				MessageBox.Show(messageBoxText2, "Invalid Character", MessageBoxButton.OK, MessageBoxImage.Hand);
				string text3 = text.Replace("\\", "/");
				this.textboxDestTextFileFromUrl.Text = text3;
				this.textboxDestTextFileFromUrl.CaretIndex = caretIndex2;
			}
		}
		private void textboxDestTextFileFromUrl_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (e.NewFocus != null && !this.ignoreTextBoxFocusLost)
			{
				string text = e.NewFocus.ToString();
				if (!text.EndsWith("Close") && !(e.NewFocus is MenuItem))
				{
					this.ignoreTextBoxFocusLost = true;
					string text2 = this.textboxDestTextFileFromUrl.Text.Trim();
					if (text2 != string.Empty)
					{
						int num = text2.IndexOf("/");
						if (num <= 0 || num >= text2.Length - 1)
						{
							string messageBoxText = "The path supplied for " + this.labelDestTextFromUrl.Content + " must include a folder name and a file name.\r\nFor example, myFolder/myText.txt";
							MessageBox.Show(messageBoxText, "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Hand);
							e.Handled = true;
							this.textboxDestTextFileFromUrl.Focus();
						}
					}
					this.ignoreTextBoxFocusLost = false;
				}
			}
		}
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/S2XConsole;component/views/loadsettingview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
		[EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.labelUrl = (Label)target;
				return;
			case 2:
				this.labelFolder = (Label)target;
				return;
			case 3:
				this.comboBoxUrl = (ComboBox)target;
				return;
			case 4:
				this.comboBoxFolder = (ComboBox)target;
				return;
			case 5:
				this.checkBoxDestination = (CheckBox)target;
				this.checkBoxDestination.Checked += new RoutedEventHandler(this.checkBoxDestination_Checked);
				this.checkBoxDestination.Unchecked += new RoutedEventHandler(this.checkBoxDestination_Unchecked);
				return;
			case 6:
				this.labelSoftwareDest = (Label)target;
				return;
			case 7:
				this.textboxSoftwareDest = (TextBox)target;
				return;
			case 8:
				this.labelTextFile = (Label)target;
				return;
			case 9:
				this.textboxTextFile = (TextBox)target;
				return;
			case 10:
				this.buttonBrowseTextFile = (Button)target;
				this.buttonBrowseTextFile.Click += new RoutedEventHandler(this.buttonBrowseTextFile_Click);
				return;
			case 11:
				this.labelTextFileDest = (Label)target;
				return;
			case 12:
				this.textboxTextFileDest = (TextBox)target;
				return;
			case 13:
				this.labelOta = (Label)target;
				return;
			case 14:
				this.comboBoxOta = (ComboBox)target;
				return;
			case 15:
				this.labelLoadText = (Label)target;
				return;
			case 16:
				this.comboBoxLoadText = (ComboBox)target;
				return;
			case 17:
				this.labelDestTextFromUrl = (Label)target;
				return;
			case 18:
				this.textboxDestTextFileFromUrl = (TextBox)target;
				this.textboxDestTextFileFromUrl.TextChanged += new TextChangedEventHandler(this.textboxDestTextFileFromUrl_TextChanged);
				this.textboxDestTextFileFromUrl.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.textboxDestTextFileFromUrl_LostKeyboardFocus);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}
	}
}
