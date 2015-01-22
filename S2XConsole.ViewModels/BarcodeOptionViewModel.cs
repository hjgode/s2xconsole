using S2X;
using S2XConsole.Models;
using S2XConsole.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
namespace S2XConsole.ViewModels
{
	internal class BarcodeOptionViewModel : INotifyPropertyChanged
	{
		public const string ERROR_INPUT_TOO_LARGE = "Error: input may be too large.";
		private Symbol _barcodeType;
		private string _password = string.Empty;
		private string _instruction = string.Empty;
		private string _inputData = string.Empty;
		private int _version;
		private bool _isNoStartBarcode;
		private bool _isNoReboot;
		private string _strEstimatedPDF417;
		private string _strEstimatedCode128;
		private MainViewModel mainViewModel;
		private string[] separator = new string[]
		{
			"#"
		};
		private Hashtable barcodeResult = new Hashtable();
		public event PropertyChangedEventHandler PropertyChanged;
		public List<string> InstructionAutoComplete
		{
			get;
			set;
		}
		public int VersionNumber
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}
		public string InputData
		{
			get
			{
				return this._inputData;
			}
			set
			{
				this._inputData = value;
			}
		}
		public string Instruction
		{
			get
			{
				return this._instruction;
			}
			set
			{
				this._instruction = value;
			}
		}
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
			}
		}
		public Symbol BarcodeType
		{
			get
			{
				return this._barcodeType;
			}
			set
			{
				this._barcodeType = value;
			}
		}
		public bool IsNoReboot
		{
			get
			{
				return this._isNoReboot;
			}
			set
			{
				this._isNoReboot = value;
			}
		}
		public bool IsNoStartBarcode
		{
			get
			{
				return this._isNoStartBarcode;
			}
			set
			{
				this._isNoStartBarcode = value;
			}
		}
		public int EstimatedPDF417Quantity
		{
			get;
			set;
		}
		public int EstimatedCode128Quantity
		{
			get;
			set;
		}
		public string EstimatedPDF417Summary
		{
			get
			{
				return this._strEstimatedPDF417;
			}
			set
			{
				this._strEstimatedPDF417 = value;
				this.NotifyPropertyChanged("EstimatedPDF417Summary");
			}
		}
		public string EstimatedCode128Summary
		{
			get
			{
				return this._strEstimatedCode128;
			}
			set
			{
				this._strEstimatedCode128 = value;
				this.NotifyPropertyChanged("EstimatedCode128Summary");
			}
		}
		public BarcodeOptionViewModel(MainViewModel mainViewModel)
		{
			this.mainViewModel = mainViewModel;
			this.InstructionAutoComplete = new List<string>();
			string instructionCollection = Settings.Default.InstructionCollection;
			if (!string.IsNullOrEmpty(instructionCollection))
			{
				string[] array = instructionCollection.Split(this.separator, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (!string.IsNullOrEmpty(text.Trim()) && !this.InstructionAutoComplete.Contains(text))
					{
						this.InstructionAutoComplete.Add(text);
					}
				}
			}
		}
		public MemoryStream SaveAsPDF()
		{
			S2X.S2X s2X = this.Update();
			try
			{
				MemoryStream result = s2X.ToPDF();
				if (this.barcodeResult.Contains(s2X))
				{
					this.barcodeResult.Remove(s2X);
				}
				return result;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error generating PDF. " + ex.ToString());
			}
			return null;
		}
		public void Print()
		{
			try
			{
				PrintingModel printingModel = new PrintingModel();
                S2X.S2X s2X = this.Update();
				printingModel.Print(s2X.GetPages());
				if (this.barcodeResult.Contains(s2X))
				{
					this.barcodeResult.Remove(s2X);
				}
			}
			catch (Exception ex)
			{
				string messageBoxText = "An error occurred while attempting to print.  Error message: " + ex.Message;
				MessageBox.Show(messageBoxText, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
		public void PrintPreview()
		{
			try
			{
				PrintingModel printingModel = new PrintingModel();
                S2X.S2X s2X = this.Update();
				printingModel.PrintPreview(s2X.GetPages());
				if (this.barcodeResult.Contains(s2X))
				{
					this.barcodeResult.Remove(s2X);
				}
			}
			catch (OutOfMemoryException ex)
			{
				GC.Collect();
				string text = "The application ran out of memory.  You may need to restart the application to use the printing commands.";
				string name = ex.GetType().Name;
				if (this.BarcodeType == Symbol.Code128)
				{
					text = text + "\r\nThe content of your bar codes may be too large for Code128.  Please try " + Symbol.PDF417.ToString() + " instead.";
				}
				MessageBox.Show(text, name, MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			catch (Exception ex2)
			{
				string messageBoxText = "An error occurred while attempting to generate the print preview.  Error message: " + ex2.Message;
				MessageBox.Show(messageBoxText, ex2.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
        public S2X.S2X Update()
		{
            System.Diagnostics.Debug.WriteLine("BarcodeOptionViewModel::Update()");
			try
			{
				string name = Enum.GetName(typeof(Symbol), this.BarcodeType);
				string text = this.PageData();
				string text2 = this.InputData;
				if (!string.IsNullOrEmpty(text) && !Common.UsingJson)
				{
					XDocument xDocument = XDocument.Parse(text2);
					XDocument xDocument2 = XDocument.Parse(text);
					xDocument.Root.Add(xDocument2.Root);
					text2 = xDocument.ToString();
				}
				this.VersionNumber = this.mainViewModel.Version();
				string[] names = Enum.GetNames(typeof(Symbol));
                names = new string[] { Symbol.PDF417.ToString() };  //only interested in PDF417
				for (int i = 0; i < names.Length; i++)
				{
					string text3 = names[i];
					Symbol symbol = (Symbol)Enum.Parse(typeof(Symbol), text3);
                    S2X.S2X s2X = null;
					try
					{
						if (this.barcodeResult.ContainsKey(text3))
						{
                            s2X = (this.barcodeResult[text3] as S2X.S2X);
						}
						else
						{
                            s2X = new S2X.S2X();
						}
						s2X.IsNoReboot = this.IsNoReboot;
						s2X.IsNoStartBarcode = this.IsNoStartBarcode;
						if (Common.UsingJson)
						{
							s2X.IsNoStartBarcode = true;
						}
						s2X.SetSourceName(Common.GetFullFooterAddition());
                        // "{\"s\":{\"WiFi\":{\"base\":0,\"v\":0,\"mode\":\"full\",\"nets\":[{\"ssid\":\"\\\"MyNetworkName1\\\"\",\"pri\":3,\"hid\":0,\"auth\":\"OPEN,SHARED\",\"km\":\"NONE\",\"wepk\":[\"*\",\"\",\"\",\"\"]},{\"ssid\":\"\\\"MyNetworkName2\\\"\",\"pri\":1,\"hid\":0,\"auth\":\"\",\"km\":\"WPA_PSK\",\"pskey\":\"*\"}]}},\"v\":\"1.0\"}"
						s2X.PrintPages(this.Instruction, this.Password, text2, symbol, this.VersionNumber);
                        /*
                        // JSON download
                        data={"v":1.0,"r":["ftp://199.64.70.66/textfiles"],"w":{"textfiledestination/textfile.txt.txt":"textfile sample"},"o":"ftp://199.64.70.66/updates","c":[{"s":"loadtextfileurl.txt","d":"textfile/loadtextfileurldest"}]}
                        */
                        if (symbol == Symbol.PDF417)
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format(
                                "data={0}\r\n instructions={1}\r\n pass={2}\r\n version={3}\r\nCommon.UsingJson={4}\r\nSetSourceName={5}\r\nIsNoReboot={6}\r\nIsNoStartBarcode={7}",
                                text2,
                                this.Instruction,
                                this.Password,
                                this.VersionNumber,
                                Common.UsingJson,
                                Common.GetFullFooterAddition(),
                                s2X.IsNoReboot,
                                s2X.IsNoStartBarcode));
                        }

						this.barcodeResult[text3] = s2X;
						string text4 = string.Format("Estimated bar codes {0}", s2X.EstimatedBarcodes);
						if (symbol == Symbol.PDF417)
						{
							this.EstimatedPDF417Quantity = s2X.EstimatedBarcodes;
							this.EstimatedPDF417Summary = text4;
						}
						else
						{
							if (symbol == Symbol.Code128)
							{
								this.EstimatedCode128Quantity = s2X.EstimatedBarcodes;
								this.EstimatedCode128Summary = text4;
							}
						}
					}
					catch (ArgumentException)
					{
						if (s2X != null && this.barcodeResult.Contains(s2X))
						{
							this.barcodeResult.Remove(s2X);
						}
						string text5 = "Error: input may be too large.";
						if (symbol == Symbol.PDF417)
						{
							this.EstimatedPDF417Summary = text5;
						}
						else
						{
							if (symbol == Symbol.Code128)
							{
								this.EstimatedCode128Summary = text5;
							}
						}
						GC.Collect();
					}
				}
                S2X.S2X result;
				if ((name == Symbol.Code128.ToString() && this.EstimatedCode128Summary == "Error: input may be too large.") || (name == Symbol.PDF417.ToString() && this.EstimatedPDF417Summary == "Error: input may be too large."))
				{
					string text6 = "Could not generate bar codes for " + name + ".  The input data may be too large.";
					Common.WriteEntryToLog(text6, EventLogEntryType.Error);
					MessageBox.Show(text6, Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
                    result = new S2X.S2X();
					return result;
				}
				string tempFileName = Path.GetTempFileName();
				File.WriteAllText(tempFileName, text2);
                result = (this.barcodeResult[name] as S2X.S2X);
				return result;
			}
			catch (XmlException ex)
			{
				Common.WriteEntryToLog(ex.ToString(), EventLogEntryType.Error);
				MessageBox.Show("Invalid data", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			catch (ExternalException ex2)
			{
				throw ex2;
			}
			catch (Exception ex3)
			{
				Common.WriteEntryToLog(ex3.ToString(), EventLogEntryType.Error);
				MessageBox.Show(ex3.Message, Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
			}
            return new S2X.S2X();
		}
		public int Version()
		{
			if (this.IsNoReboot || this.IsNoStartBarcode)
			{
				return 3;
			}
			return 1;
		}
		private void NotifyPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		internal void SaveInstruction()
		{
			if (!string.IsNullOrEmpty(this.Instruction) && string.Empty != this.Instruction.Trim() && this.InstructionAutoComplete.FirstOrDefault((string ins) => ins == this.Instruction) == null)
			{
				Settings expr_42 = Settings.Default;
				expr_42.InstructionCollection = expr_42.InstructionCollection + this.separator[0] + this.Instruction;
			}
		}
		private string PageData()
		{
			if (!this.IsNoReboot || Common.UsingJson)
			{
				return string.Empty;
			}
			return "<Subsystem Name=\"SS_Client\"><Group Name=\"Download\"><Field Name=\"ProcessNow\">True</Field></Group></Subsystem>";
		}
	}
}
