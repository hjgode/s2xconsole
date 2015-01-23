using Newtonsoft.Json.Linq;
using S2XConsole.Models;
using S2XConsole.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
namespace S2XConsole.ViewModels
{
	internal class LoadSettingViewModel
	{
		private string[] separator = new string[]
		{
			"#"
		};
		public List<string> UrlAutoComplete
		{
			get;
			set;
		}
		public List<string> FolderAutoComplete
		{
			get;
			set;
		}
		public string SelectedURL
		{
			get;
			set;
		}
		public string Destination
		{
			get;
			set;
		}
		public bool DestinationSpecified
		{
			get;
			set;
		}
		public string SelectedFolder
		{
			get;
			set;
		}
		public string TextFile
		{
			get;
			set;
		}
		public string TextFileDestination
		{
			get;
			set;
		}
		public string SelectedOta
		{
			get;
			set;
		}
		public string SelectedTextFileFromUrl
		{
			get;
			set;
		}
		public string TextFileFromUrlDestination
		{
			get;
			set;
		}
		public LoadSettingViewModel()
		{
			this.UrlAutoComplete = new List<string>();
			this.FolderAutoComplete = new List<string>();
			string uRLCollection = Settings.Default.URLCollection;
			if (!string.IsNullOrEmpty(uRLCollection))
			{
				string[] array = uRLCollection.Split(this.separator, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (!string.IsNullOrEmpty(text.Trim()) && !this.UrlAutoComplete.Contains(text))
					{
						this.UrlAutoComplete.Add(text);
					}
				}
			}
			string folder = Settings.Default.Folder;
			if (!string.IsNullOrEmpty(folder))
			{
				string[] array2 = folder.Split(this.separator, StringSplitOptions.RemoveEmptyEntries);
				for (int j = 0; j < array2.Length; j++)
				{
					string text2 = array2[j];
					if (!string.IsNullOrEmpty(text2.Trim()) && !this.FolderAutoComplete.Contains(text2))
					{
						this.FolderAutoComplete.Add(text2);
					}
				}
			}
		}
		public void UpdateSource(string newURLValue, string newFolderValue, string newOtaValue, string newTextFileFromUrl)
		{
			this.SelectedURL = newURLValue;
			Common.DownloadUrl = newURLValue;
			this.SelectedFolder = newFolderValue;
			this.SelectedOta = newOtaValue;
			this.SelectedTextFileFromUrl = newTextFileFromUrl;
			if (!string.IsNullOrEmpty(newURLValue) && string.Empty != newURLValue.Trim() && !this.UrlAutoComplete.Contains(newURLValue))
			{
				Settings expr_50 = Settings.Default;
				expr_50.URLCollection = expr_50.URLCollection + this.separator[0] + newURLValue;
			}
			if (!string.IsNullOrEmpty(newFolderValue) && string.Empty != newFolderValue.Trim() && !this.FolderAutoComplete.Contains(newFolderValue))
			{
				Settings.Default.Folder = Settings.Default.Folder + this.separator[0] + newFolderValue;
			}
			if (!string.IsNullOrEmpty(newOtaValue) && string.Empty != newOtaValue.Trim() && !this.UrlAutoComplete.Contains(newOtaValue))
			{
				Settings expr_E0 = Settings.Default;
				expr_E0.URLCollection = expr_E0.URLCollection + this.separator[0] + newOtaValue;
			}
			if (!string.IsNullOrEmpty(newTextFileFromUrl) && string.Empty != newTextFileFromUrl.Trim() && !this.UrlAutoComplete.Contains(newTextFileFromUrl))
			{
				Settings expr_129 = Settings.Default;
				expr_129.URLCollection = expr_129.URLCollection + this.separator[0] + newTextFileFromUrl;
			}
		}

        /// <summary>
        /// loads the selected settings of the download options dialog and creates JSON or XML data
        /// </summary>
        /// <returns></returns>
		public string PageData()
		{
            Logger.logger.add2log("LoadSettingViewModel::PageData()");
			string text = string.Empty;
			if (!Common.UsingJson)
			{
				if (!string.IsNullOrEmpty(this.SelectedFolder) && string.Empty != this.SelectedFolder.Trim())
				{
					this.SelectedFolder = this.SelectedFolder.Trim();
					if (!this.SelectedFolder.StartsWith("\\"))
					{
						string selectedFolder = this.SelectedFolder;
						this.SelectedFolder = "\\" + selectedFolder;
					}
					XElement xElement = new XElement("Subsystem", new object[]
					{
						new XAttribute("Name", "SS_Client"),
						new XElement("Group", new object[]
						{
							new XAttribute("Name", "Identity"),
							new XElement("Field", new object[]
							{
								new XAttribute("Name", "ConsoleFolder"),
								new XText(this.SelectedFolder)
							})
						})
					});
					text += xElement.ToString(SaveOptions.DisableFormatting);
                    Logger.logger.add2log("LoadSettingsViewModel::PageData() SelectedFolder->text=" + text);
				}
				if (!string.IsNullOrEmpty(this.SelectedURL) && string.Empty != this.SelectedURL.Trim())
				{
					XElement xElement2 = new XElement("Subsystem", new object[]
					{
						new XAttribute("Name", "SS_Client"),
						new XElement("Group", new object[]
						{
							new XAttribute("Name", "FileSystem"),
							new XElement("Group", new object[]
							{
								new XAttribute("Name", "Download"),
								new XElement("Field", new object[]
								{
									new XAttribute("Name", "Url"),
									new XText(this.SelectedURL)
								})
							})
						})
					});
					text += xElement2.ToString(SaveOptions.DisableFormatting);
                    Logger.logger.add2log("LoadSettingsViewModel::PageData() SelectedURL->text=" + text);
				}
			}
			else
			{
				string json = "{'v':1.0}";
				JObject jObject = JObject.Parse(json);
				if (!string.IsNullOrEmpty(this.SelectedURL) && string.Empty != this.SelectedURL.Trim())
				{
					if (this.DestinationSpecified && this.Destination != null && this.Destination != string.Empty)
					{
						JProperty jProperty = new JProperty("s", new JValue(this.SelectedURL));
						JProperty jProperty2 = new JProperty("d", new JValue(this.Destination));
						JObject item = new JObject(new object[]
						{
							jProperty,
							jProperty2
						});
						JProperty content = new JProperty("c", new JArray
						{
							item
						});
						jObject.Add(content);
                        Logger.logger.add2log("LoadSettingsViewModel::PageData() Destination->jObject=" + content.ToString());

					}
					else
					{
						JArray content2 = new JArray(this.SelectedURL);
						JProperty content3 = new JProperty("r", content2);
						jObject.Add(content3);
                        Logger.logger.add2log("LoadSettingsViewModel::PageData() Destination->SelectedURL=" + content3.ToString());
					}
				}
				if (this.TextFile != null && this.TextFile.Trim() != string.Empty)
				{
					FileInfo fileInfo = new FileInfo(this.TextFile);
					string name;
					if (this.TextFileDestination == null || this.TextFileDestination == string.Empty)
					{
						name = fileInfo.Name;
					}
					else
					{
						if (this.TextFileDestination.EndsWith("/"))
						{
							name = this.TextFileDestination + fileInfo.Name;
						}
						else
						{
							name = this.TextFileDestination + "/" + fileInfo.Name;
						}
					}
					string value = File.ReadAllText(this.TextFile);
                    Logger.logger.add2log("LoadSettingsViewModel::PageData() ReadAllText('"+this.TextFile+"')=>" + value);
                    JValue content4 = new JValue(value);
					JProperty content5 = new JProperty(name, content4);
					JObject content6 = new JObject(content5);
					JProperty content7 = new JProperty("w", content6);
					jObject.Add(content7);
                    Logger.logger.add2log("LoadSettingsViewModel::PageData() content7=" + content7.ToString());
				}

                //process SelectedOta field
				if (this.SelectedOta != null && this.SelectedOta.Trim() != string.Empty)
				{
					JProperty content8 = new JProperty("o", new JValue(this.SelectedOta));
					jObject.Add(content8);
                    Logger.logger.add2log("LoadSettingsViewModel::PageData() SelectedOta=" + content8.ToString());
				}

                //process SelectedTextFileFromUrl field
				if (this.SelectedTextFileFromUrl != null && this.SelectedTextFileFromUrl.Trim() != string.Empty)
				{
					JArray jArray = new JArray();
					JProperty content9 = new JProperty("c", jArray);
					if (this.TextFileFromUrlDestination != null && this.TextFileFromUrlDestination.Trim() != string.Empty)
					{
						JObject jObject2 = new JObject();
						JProperty content10 = new JProperty("s", new JValue(this.SelectedTextFileFromUrl));
						JProperty content11 = new JProperty("d", new JValue(this.TextFileFromUrlDestination));
						jObject2.Add(content10);
						jObject2.Add(content11);
						jArray.Add(jObject2);
                        Logger.logger.add2log("LoadSettingsViewModel::PageData() SelectedTextFileFromUrl::jArray.add " + jObject2.ToString());
					}
					else
					{
						jArray.Add(this.SelectedTextFileFromUrl);
                        Logger.logger.add2log("LoadSettingsViewModel::PageData() SelectedTextFileFromUrl=" + SelectedTextFileFromUrl);
					}
					jObject.Add(content9);
                    Logger.logger.add2log("LoadSettingsViewModel::PageData() SelectedTextFileFromUrl-content9=" + content9);
				}
				text = jObject.ToString();
			}
            Logger.logger.add2log("LoadSettingViewModel::PageData() return="+text);
			return text;
		}
	}
}
