using S2XConsole.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
namespace S2XConsole.ViewModels
{
	public class CommunicationViewModel : INotifyPropertyChanged
	{
		public List<SettingsSource> _communicationSettings = new List<SettingsSource>();
		private CommunicationSettingModel model = new CommunicationSettingModel();
		public event PropertyChangedEventHandler PropertyChanged;
		public List<SettingsSource> CommunicationSettings
		{
			get
			{
				return this._communicationSettings;
			}
			set
			{
				this._communicationSettings = value;
				this.NotifyPropertyChanged("CommunicationSettings");
			}
		}
		public CommunicationViewModel()
		{
			try
			{
				List<BackupSetting> backupSettings = this.model.GetBackupSettings();
				foreach (BackupSetting current in backupSettings)
				{
					this.CommunicationSettings.Add(new SettingsSource
					{
						Name = current.Name,
						Path = current.Path,
						Description = current.Description
					});
				}
				this.CommunicationSettings = this.CommunicationSettings;
			}
			catch (Exception)
			{
			}
		}
		public string PageData()
		{
			SettingsSource settingsSource = this.CommunicationSettings.FirstOrDefault((SettingsSource s) => s.IsSelected);
			if (settingsSource != null && !string.IsNullOrEmpty(settingsSource.Path) && Directory.Exists(settingsSource.Path))
			{
				Common.SettingsSourceName = settingsSource.Name;
				return this.model.GetSettingsData(settingsSource.Path);
			}
			return string.Empty;
		}
		private void NotifyPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public int Version()
		{
			if (this.model.IsWWANExist)
			{
				return 3;
			}
			return 1;
		}
	}
}
