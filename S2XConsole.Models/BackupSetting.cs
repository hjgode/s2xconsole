using System;
namespace S2XConsole.Models
{
	public class BackupSetting
	{
		public readonly string Name;
		public readonly string Path;
		public readonly string Description;
		public BackupSetting(string name, string path, string description)
		{
			this.Name = name;
			this.Path = path;
			this.Description = description;
		}
	}
}
