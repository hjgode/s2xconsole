using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using S2XConsole.Commands;
using S2XConsole.Interface;
using S2XConsole.Models;
using S2XConsole.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
namespace S2XConsole.ViewModels
{
	public class MainViewModel : ViewModelBase, INotifyPropertyChanged
	{
		private const string REGISTRY_KEY_SERVER = "Software\\Intermec\\SmartSystem\\Server";
		private const string REGISTRY_VALUE_SERVER_PATH = "RootPath";
		private DelegateCommand exitCommand;
		private DelegateCommand nextCommand;
		private DelegateCommand previousCommand;
		private LinkedListNode<System.Windows.Controls.UserControl> _currentNode;
		private int _currentPageIndex;
		private int _totalPages;
		private int _commandLineVersion;
		private bool _isBrowseMode;
		private bool _isNotLastPage;
		private bool _isNotFirstPage;
		private System.Windows.Controls.UserControl _currentPage;
		private LinkedList<System.Windows.Controls.UserControl> _pageList;
		private List<object> viewModels = new List<object>();
		public new event PropertyChangedEventHandler PropertyChanged;
		public ICommand ExitCommand
		{
			get
			{
				if (this.exitCommand == null)
				{
					this.exitCommand = new DelegateCommand(new Action(this.Exit));
				}
				return this.exitCommand;
			}
		}
		public ICommand NextCommand
		{
			get
			{
				if (this.nextCommand == null)
				{
					this.nextCommand = new DelegateCommand(new Action(this.Next));
				}
				return this.nextCommand;
			}
		}
		public ICommand PreviousCommand
		{
			get
			{
				if (this.previousCommand == null)
				{
					this.previousCommand = new DelegateCommand(new Action(this.Previous));
				}
				return this.previousCommand;
			}
		}
		public System.Windows.Controls.UserControl CurrentPage
		{
			get
			{
				return this._currentPage;
			}
			set
			{
				this._currentPage = value;
				for (int i = 0; i < this.PageList.Count; i++)
				{
					if (this.PageList.ElementAt(i) == value)
					{
						this.CurrentPageIndex = i + 1;
					}
				}
				if (this.CurrentPage is IPage)
				{
					IPage page = this.CurrentPage as IPage;
					page.Enter();
				}
				this.NotifyPropertyChanged("CurrentPage");
			}
		}
		public LinkedList<System.Windows.Controls.UserControl> PageList
		{
			get
			{
				return this._pageList;
			}
			set
			{
				this._pageList = value;
				this.NotifyPropertyChanged("PageList");
			}
		}
		public LinkedListNode<System.Windows.Controls.UserControl> CurrentNode
		{
			get
			{
				if (this._currentNode == null && this.PageList != null)
				{
					this._currentNode = this.PageList.First;
				}
				return this._currentNode;
			}
			set
			{
				this._currentNode = value;
			}
		}
		public int CurrentPageIndex
		{
			get
			{
				return this._currentPageIndex;
			}
			set
			{
				this._currentPageIndex = value;
				this.IsNotFirstPage = (value != 1);
				this.IsNotLastPage = (value != this.TotalPages);
				this.NotifyPropertyChanged("CurrentPageIndex");
			}
		}
		public int TotalPages
		{
			get
			{
				return this._totalPages;
			}
			set
			{
				this._totalPages = value;
				this.IsNotFirstPage = (this.CurrentPageIndex != 1);
				this.IsNotLastPage = (this.CurrentPageIndex != value);
				this.NotifyPropertyChanged("TotalPages");
			}
		}
		public bool IsBrowseMode
		{
			get
			{
				return this._isBrowseMode;
			}
			set
			{
				this._isBrowseMode = value;
			}
		}
		public int CommandLineVersion
		{
			get
			{
				return this._commandLineVersion;
			}
			set
			{
				this._commandLineVersion = value;
			}
		}
		public bool IsNotFirstPage
		{
			get
			{
				return this._isNotFirstPage;
			}
			set
			{
				this._isNotFirstPage = value;
				this.NotifyPropertyChanged("IsNotFirstPage");
			}
		}
		public bool IsNotLastPage
		{
			get
			{
				return this._isNotLastPage;
			}
			set
			{
				this._isNotLastPage = value;
				this.NotifyPropertyChanged("IsNotLastPage");
			}
		}
		public MainViewModel(string[] args)
		{
			this.PageList = new LinkedList<System.Windows.Controls.UserControl>();
			string text = string.Empty;
			for (int i = 0; i < args.Length; i++)
			{
				string str = args[i];
				text = text + str + "|";
			}
			if (args != null)
			{
				try
				{
					if (args.FirstOrDefault((string value) => value.ToLower() == "/browse") != null)
					{
						this.IsBrowseMode = true;
					}
					else
					{
						if (args.FirstOrDefault((string value) => value.ToLower() == "/standalone") != null)
						{
							Common.IsInStandaloneMode = true;
						}
					}
					for (int j = 0; j < args.Length; j++)
					{
						if (string.Compare(args[j].ToLower(), "/browse") == 0)
						{
							this.IsBrowseMode = true;
						}
						if (string.Compare(args[j].ToLower(), "/version") == 0 && j + 1 < args.Length)
						{
							int commandLineVersion = 0;
							if (int.TryParse(args[++j], out commandLineVersion))
							{
								this.CommandLineVersion = commandLineVersion;
							}
						}
					}
				}
				catch (Exception ex)
				{
					System.Windows.MessageBox.Show(string.Concat(new string[]
					{
						"Command Line: \"",
						text,
						"\"  Error: ",
						ex.Message,
						" inner: ",
						ex.InnerException.Message
					}), "Command Line Exception");
				}
			}
			this.PreloadData();
		}
		public void AddPage(System.Windows.Controls.UserControl control)
		{
			this.PageList.AddLast(control);
			this.TotalPages = this.PageList.Count;
		}
		public void AddPage(System.Windows.Controls.UserControl control, System.Windows.Controls.UserControl referenceControl)
		{
			LinkedListNode<System.Windows.Controls.UserControl> node = this.PageList.Find(referenceControl);
			this.PageList.AddBefore(node, control);
			this.TotalPages = this.PageList.Count;
		}
		public void AddPage(System.Windows.Controls.UserControl control, int index)
		{
			LinkedListNode<System.Windows.Controls.UserControl> linkedListNode = this.PageList.First;
			for (int i = 0; i < index; i++)
			{
				linkedListNode = linkedListNode.Next;
			}
			if (linkedListNode != null)
			{
				this.AddPage(control, linkedListNode.Value);
			}
		}
		public void RemovePage(System.Windows.Controls.UserControl control)
		{
			this.PageList.Remove(control);
			this.TotalPages = this.PageList.Count;
		}
		public void RemovePage(int index)
		{
			LinkedListNode<System.Windows.Controls.UserControl> linkedListNode = this.PageList.First;
			for (int i = 0; i < index; i++)
			{
				linkedListNode = linkedListNode.Next;
			}
			if (linkedListNode != null)
			{
				this.RemovePage(linkedListNode.Value);
			}
		}
		public string GetBarCodeData()
		{
			if (this.PageList == null)
			{
				return string.Empty;
			}
			string text = string.Empty;
			if (this.IsBrowseMode)
			{
				text = this.GetBrowseData();
                System.Diagnostics.Debug.WriteLine("GetBrowseData(): " + text);

			}
			else
			{
				if (!Common.UsingJson)
				{
					using (LinkedList<System.Windows.Controls.UserControl>.Enumerator enumerator = this.PageList.GetEnumerator())
					{
                        int i = 0;
						while (enumerator.MoveNext())
						{
							System.Windows.Controls.UserControl current = enumerator.Current;
							if (current is IPage)
							{
								IPage page = current as IPage;
								text += page.PageData();
                                System.Diagnostics.Debug.WriteLine("page " + i.ToString() + ": " + text);
							}
						}
						goto IL_12B;
					}
				}
				List<string> list = new List<string>();
				foreach (System.Windows.Controls.UserControl current2 in this.PageList)
				{
					if (current2 is IPage)
					{
						IPage page2 = current2 as IPage;
						string text2 = page2.PageData();
						if (text2 != null && text2 != string.Empty)
						{
							try
							{
								JObject.Parse(text2);
								list.Add(text2);
							}
							catch (Exception)
							{
							}
						}
					}
				}
				if (list.Count == 0)
				{
					System.Windows.MessageBox.Show("No data supplied.", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return string.Empty;
				}
				text = MainViewModel.processJsonStrings(list);
				text = Common.TrimJsonWhitespace(text);
			}
			IL_12B:
			if (!Common.UsingJson)
			{
				text = Common.WrapXmlInDevInfo(text);
			}
			return text;
            // "{\"s\":{\"WiFi\":{\"base\":0,\"v\":0,\"mode\":\"full\",\"nets\":[{\"ssid\":\"\\\"MyNetworkName1\\\"\",\"pri\":3,\"hid\":0,\"auth\":\"OPEN,SHARED\",\"km\":\"NONE\",\"wepk\":[\"*\",\"\",\"\",\"\"]},{\"ssid\":\"\\\"MyNetworkName2\\\"\",\"pri\":1,\"hid\":0,\"auth\":\"\",\"km\":\"WPA_PSK\",\"pskey\":\"*\"}]}},\"v\":\"1.0\"}"
		}
		private static string processJsonStrings(List<string> listJson)
		{
			JObject jObject = JObject.Parse(listJson[0]);
			JObject jObject2 = jObject["subsystems"] as JObject;
			for (int i = 1; i < listJson.Count; i++)
			{
				JObject jObject3 = JObject.Parse(listJson[i]);
				foreach (JToken current in jObject3.Children())
				{
					if (current.Type == JTokenType.Property)
					{
						JProperty jProperty = current as JProperty;
						if (jProperty.Name != "subsystems")
						{
							if (jObject[jProperty.Name] == null)
							{
								jObject.Add(jProperty.Name, jProperty.Value);
								continue;
							}
							if (!(jProperty.Name == "c") && !(jProperty.Name == "r") && !(jProperty.Name == "w"))
							{
								continue;
							}
							JToken jToken = jObject[jProperty.Name];
							if (!(jToken is JArray))
							{
								jObject.Remove(jProperty.Name);
								jObject.Add(jProperty);
								continue;
							}
							JArray jArray = jToken as JArray;
							JArray jArray2 = jObject3[jProperty.Name] as JArray;
							if (jArray2.HasValues)
							{
								jArray.Add(jArray2.First);
								continue;
							}
							continue;
						}
						else
						{
							JObject jObject4 = jObject3["subsystems"] as JObject;
							if (jObject4 == null)
							{
								continue;
							}
							using (IEnumerator<KeyValuePair<string, JToken>> enumerator2 = jObject4.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									KeyValuePair<string, JToken> current2 = enumerator2.Current;
									if (jObject2[current2.Key] == null)
									{
										jObject2.Add(current2.Key, current2.Value);
									}
								}
								continue;
							}
						}
					}
					jObject.Add(current);
				}
			}
			return jObject.ToString();
		}
		private string GetBrowseData()
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			openFileDialog.DefaultExt = ".xml";
			openFileDialog.Filter = "Settings (.xml)|*.xml";
			if (!(openFileDialog.ShowDialog() == true))
			{
				System.Windows.MessageBox.Show("No data provided", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
				System.Windows.Application.Current.Shutdown();
				return null;
			}
			if (!string.IsNullOrEmpty(openFileDialog.FileName))
			{
				FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
				Common.SettingsSourceName = fileInfo.Name;
				return File.ReadAllText(openFileDialog.FileName);
			}
			System.Windows.MessageBox.Show("No data provided", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
			System.Windows.Application.Current.Shutdown();
			return null;
		}
		public int Version()
		{
			if (this.CommandLineVersion > 0)
			{
				return this.CommandLineVersion;
			}
			int num = 1;
			if (this.PageList != null)
			{
				foreach (System.Windows.Controls.UserControl current in this.PageList)
				{
					if (current is IPage)
					{
						IPage page = current as IPage;
						int num2 = page.Version();
						if (num < num2)
						{
							num = num2;
						}
					}
				}
			}
			return num;
		}
		private void Exit()
		{
			System.Windows.Application.Current.Shutdown();
		}
		private void Next()
		{
			if (this.CurrentNode != null && this.CurrentNode.Next != null)
			{
				if (this.CurrentPage is IPage)
				{
					IPage page = this.CurrentPage as IPage;
					page.Next();
				}
				this.CurrentNode = this.CurrentNode.Next;
				this.CurrentPage = this.CurrentNode.Value;
				this.CurrentPage.Focus();
			}
		}
		private void Previous()
		{
			if (this.CurrentNode != null && this.CurrentNode.Previous != null)
			{
				this.CurrentNode = this.CurrentNode.Previous;
				this.CurrentPage = this.CurrentNode.Value;
			}
		}
		private void PreloadData()
		{
			ThreadStart start = delegate
			{
				this.viewModels.Add(new CommunicationViewModel());
				this.viewModels.Add(new LicenseViewModel());
			};
			new Thread(start).Start();
		}
		private void NotifyPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public void ShowHelpHtml()
		{
			try
			{
				string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				string text = Path.Combine(directoryName, "Help\\default.htm");
				if (File.Exists(text))
				{
					Process.Start(text);
				}
				else
				{
					System.Windows.MessageBox.Show("Help not found at: " + text, "Help Files Not Found", MessageBoxButton.OK, MessageBoxImage.Hand);
				}
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Error launching help files: " + ex.Message, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
		public void ShowHelpChm()
		{
			try
			{
				string ssServerPath = MainViewModel.getSsServerPath();
				if (ssServerPath != null)
				{
					using (Form form = new Form())
					{
						form.Name = Settings.Default.AppDisplayName + " Help";
						Help.ShowHelp(form, ssServerPath + "\\ScanToConnect.chm");
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteEntryToLog(ex.ToString(), EventLogEntryType.Error);
			}
		}
		private static string getSsServerPath()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Intermec\\SmartSystem\\Server");
			return (string)registryKey.GetValue("RootPath");
		}
	}
}
