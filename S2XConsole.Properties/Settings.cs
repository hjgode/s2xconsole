using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace S2XConsole.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), CompilerGenerated]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}
		[ApplicationScopedSetting, DefaultSettingValue("ScanNGo Bar Code Creator"), DebuggerNonUserCode]
		public string AppDisplayName
		{
			get
			{
				return (string)this["AppDisplayName"];
			}
		}
		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string Folder
		{
			get
			{
				return (string)this["Folder"];
			}
			set
			{
				this["Folder"] = value;
			}
		}
		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string URLCollection
		{
			get
			{
				return (string)this["URLCollection"];
			}
			set
			{
				this["URLCollection"] = value;
			}
		}
		[DefaultSettingValue("SOFTWARE\\\\Intermec\\\\SmartSystem"), UserScopedSetting, DebuggerNonUserCode]
		public string SQLInstanceKey
		{
			get
			{
				return (string)this["SQLInstanceKey"];
			}
			set
			{
				this["SQLInstanceKey"] = value;
			}
		}
		[DefaultSettingValue("SQLInstance"), UserScopedSetting, DebuggerNonUserCode]
		public string SQLInstance
		{
			get
			{
				return (string)this["SQLInstance"];
			}
			set
			{
				this["SQLInstance"] = value;
			}
		}
		[DefaultSettingValue("Server={0}; Database={1}; Integrated Security=SSPI; MultipleActiveResultSets=True;"), UserScopedSetting, DebuggerNonUserCode]
		public string ConnectionString
		{
			get
			{
				return (string)this["ConnectionString"];
			}
			set
			{
				this["ConnectionString"] = value;
			}
		}
		[DefaultSettingValue("SOFTWARE\\\\Intermec\\\\SmartSystem\\\\Server\\\\Software Store"), UserScopedSetting, DebuggerNonUserCode]
		public string SSLIBPath
		{
			get
			{
				return (string)this["SSLIBPath"];
			}
			set
			{
				this["SSLIBPath"] = value;
			}
		}
		[DefaultSettingValue("metadata=res://*/Models.DataModel.csdl|res://*/Models.DataModel.ssdl|res://*/Models.DataModel.msl;provider=System.Data.SqlClient;provider connection string=\"Server={0}; Database={1}; Integrated Security=SSPI; MultipleActiveResultSets=True;\""), UserScopedSetting, DebuggerNonUserCode]
		public string EntityConnectionString
		{
			get
			{
				return (string)this["EntityConnectionString"];
			}
			set
			{
				this["EntityConnectionString"] = value;
			}
		}
		[DefaultSettingValue("settings.xml"), UserScopedSetting, DebuggerNonUserCode]
		public string SettingsFile
		{
			get
			{
				return (string)this["SettingsFile"];
			}
			set
			{
				this["SettingsFile"] = value;
			}
		}
		[DefaultSettingValue("TranslateSettingsForSTC.xsl"), UserScopedSetting, DebuggerNonUserCode]
		public string TransformFile
		{
			get
			{
				return (string)this["TransformFile"];
			}
			set
			{
				this["TransformFile"] = value;
			}
		}
		[DefaultSettingValue("/*/Subsystem[@Name='WWAN Radio']"), UserScopedSetting, DebuggerNonUserCode]
		public string WWANPath
		{
			get
			{
				return (string)this["WWANPath"];
			}
			set
			{
				this["WWANPath"] = value;
			}
		}
		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string InstructionCollection
		{
			get
			{
				return (string)this["InstructionCollection"];
			}
			set
			{
				this["InstructionCollection"] = value;
			}
		}
		[DefaultSettingValue("SOFTWARE\\\\Intermec\\\\ScanNGo"), UserScopedSetting, DebuggerNonUserCode]
		public string S2XKey
		{
			get
			{
				return (string)this["S2XKey"];
			}
			set
			{
				this["S2XKey"] = value;
			}
		}
		[DefaultSettingValue("StandaloneMode"), UserScopedSetting, DebuggerNonUserCode]
		public string StandaloneValue
		{
			get
			{
				return (string)this["StandaloneValue"];
			}
			set
			{
				this["StandaloneValue"] = value;
			}
		}
		private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
		{
		}
		private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
		{
		}
	}
}
