using Microsoft.Win32;
using S2X;
using S2XConsole.Interface;
using S2XConsole.Models;
using S2XConsole.Properties;
using S2XConsole.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Printing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
namespace S2XConsole.Views
{
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public class BarcodeOptionView : UserControl, IPage, IComponentConnector
	{
		private BarcodeOptionViewModel viewModel;
		private MainViewModel mainViewModel;
		internal RadioButton radioPdf417;
		internal RadioButton radioCode128;
		internal InstructionBox url;
		internal GroupBox groupBoxOptions;
		internal CheckBox checkboxNoStartBarcode;
		internal CheckBox checkboxNoReboot;
		private bool _contentLoaded;
		public BarcodeOptionView(MainViewModel mainViewModel)
		{
			this.mainViewModel = mainViewModel;
			this.viewModel = new BarcodeOptionViewModel(mainViewModel);
			base.DataContext = this.viewModel;
			this.InitializeComponent();
			if (Common.IsInStandaloneMode)
			{
				this.checkboxNoStartBarcode.ToolTip = this.checkboxNoStartBarcode.Content;
				this.checkboxNoReboot.ToolTip = this.checkboxNoReboot.Content;
			}
		}
		public void Next()
		{
		}
		public void Enter()
		{
			if (Common.UsingJson)
			{
				this.groupBoxOptions.Visibility = Visibility.Hidden;
			}
			else
			{
				this.groupBoxOptions.Visibility = Visibility.Visible;
			}
			if (Common.IsInStandaloneMode)
			{
				this.radioCode128.IsEnabled = false;
				if (!this.radioPdf417.IsChecked.Value)
				{
					this.radioPdf417.IsChecked = new bool?(true);
				}
			}
			else
			{
				this.radioCode128.IsEnabled = true;
			}
			this.viewModel.InputData = this.mainViewModel.GetBarCodeData();
			ThreadStart start = delegate
			{
				this.viewModel.Update();
			};
			new Thread(start).Start();
		}
		public string PageData()
		{
			return string.Empty;
		}
		public int Version()
		{
			return this.viewModel.Version();
		}
		public void Close()
		{
			this.viewModel.SaveInstruction();
		}
		private void SavePDF_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				MemoryStream memoryStream = this.viewModel.SaveAsPDF();
				if (memoryStream != null)
				{
					SaveFileDialog saveFileDialog = new SaveFileDialog();
					saveFileDialog.FileName = Settings.Default.AppDisplayName;
					saveFileDialog.DefaultExt = ".pdf";
					saveFileDialog.Filter = "PDF |*.pdf";
					if (saveFileDialog.ShowDialog() == true)
					{
						FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
						using (fileStream)
						{
							byte[] buffer = memoryStream.GetBuffer();
							fileStream.Write(buffer, 0, (int)memoryStream.Length);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
		private void pdf417_Checked(object sender, RoutedEventArgs e)
		{
			if (this.viewModel.BarcodeType != Symbol.PDF417)
			{
				this.viewModel.BarcodeType = Symbol.PDF417;
			}
		}
		private void code128_Checked(object sender, RoutedEventArgs e)
		{
			if (this.viewModel.BarcodeType != Symbol.Code128)
			{
				if (this.viewModel.EstimatedPDF417Quantity > 4)
				{
					string messageBoxText = string.Concat(new object[]
					{
						"Bar Code content is too large for ",
						Symbol.Code128,
						".  ",
						Symbol.PDF417.ToString(),
						" will be selected instead."
					});
					string caption = "Content Too Large for " + Symbol.Code128;
					MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					this.radioPdf417.IsChecked = new bool?(true);
					return;
				}
				this.viewModel.BarcodeType = Symbol.Code128;
			}
		}
		private void Print_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				this.viewModel.Print();
			}
			catch (PrintSystemException)
			{
				MessageBox.Show("Error: Printer Unavailable", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
		private void PrintPreview_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.PrintPreview();
		}
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/S2XConsole;component/views/barcodeoptionview.xaml", UriKind.Relative);
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
				this.radioPdf417 = (RadioButton)target;
				this.radioPdf417.Checked += new RoutedEventHandler(this.pdf417_Checked);
				return;
			case 2:
				this.radioCode128 = (RadioButton)target;
				this.radioCode128.Checked += new RoutedEventHandler(this.code128_Checked);
				return;
			case 3:
				this.url = (InstructionBox)target;
				return;
			case 4:
				this.groupBoxOptions = (GroupBox)target;
				return;
			case 5:
				this.checkboxNoStartBarcode = (CheckBox)target;
				return;
			case 6:
				this.checkboxNoReboot = (CheckBox)target;
				return;
			case 7:
				((Button)target).Click += new RoutedEventHandler(this.PrintPreview_Click);
				return;
			case 8:
				((Button)target).Click += new RoutedEventHandler(this.SavePDF_Click);
				return;
			case 9:
				((Button)target).Click += new RoutedEventHandler(this.Print_Click);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}
	}
}
