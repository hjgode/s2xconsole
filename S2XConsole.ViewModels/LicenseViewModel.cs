using S2XConsole.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
namespace S2XConsole.ViewModels
{
	public class LicenseViewModel : INotifyPropertyChanged
	{
		private List<SettingsSource> _licenseSettings = new List<SettingsSource>();
		private LicenseModel model = new LicenseModel();
		public event PropertyChangedEventHandler PropertyChanged;
		public List<SettingsSource> LicenseSettings
		{
			get
			{
				return this._licenseSettings;
			}
			set
			{
				this._licenseSettings = value;
				this.NotifyPropertyChanged("LicenseSettings");
			}
		}
		public LicenseViewModel()
		{
			try
			{
				string[] exportedLicenseBundleNames = this.model.GetExportedLicenseBundleNames();
				if (exportedLicenseBundleNames != null)
				{
					string[] array = exportedLicenseBundleNames;
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						new SettingsSource();
						string description;
						int num;
						string text2;
						this.model.GetExportedLicenseBundleInformation(text, out description, out num, out text2);
						this.LicenseSettings.Add(new SettingsSource
						{
							Name = text,
							Path = this.model.getFullPathForExportedLicenseBundle(text),
							Description = description
						});
					}
				}
			}
			catch (Exception)
			{
			}
		}
		public string PageData()
		{
			IEnumerable<SettingsSource> enumerable = 
				from item in this.LicenseSettings
				where item.IsSelected
				select item;
			string text = string.Empty;
			if (enumerable != null)
			{
				foreach (SettingsSource current in enumerable)
				{
					if (!string.IsNullOrEmpty(current.Path) && File.Exists(current.Path))
					{
						string xml = File.ReadAllText(current.Path);
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.LoadXml(xml);
						if (xmlDocument.DocumentElement.Name == "DevInfo")
						{
							text += xmlDocument.DocumentElement.InnerXml;
						}
					}
				}
			}
			return text;
		}
		private void NotifyPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
