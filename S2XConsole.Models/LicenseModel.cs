using Microsoft.Win32;
using S2XConsole.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
namespace S2XConsole.Models
{
	public class LicenseModel
	{
		internal string[] GetExportedLicenseBundleNames()
		{
			SqlDataReader sqlDataReader = null;
			string[] result = null;
			try
			{
				List<string> list = new List<string>();
				string cmdText = "SELECT DISTINCT(Name) FROM ExportedLicenses";
				SqlConnection sqlConnection = new SqlConnection(Common.GetConnectionString());
				sqlConnection.Open();
				SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
				sqlDataReader = sqlCommand.ExecuteReader();
				sqlCommand.Parameters.Clear();
				while (sqlDataReader.Read())
				{
					string @string = sqlDataReader.GetString(0);
					if (!list.Contains(@string))
					{
						list.Add(@string);
					}
				}
				sqlDataReader.Close();
				result = list.ToArray();
			}
			catch (Exception)
			{
			}
			finally
			{
				if (sqlDataReader != null && !sqlDataReader.IsClosed)
				{
					sqlDataReader.Close();
				}
			}
			return result;
		}
		internal void GetExportedLicenseBundleInformation(string licenseName, out string featureName, out int count, out string note)
		{
			featureName = null;
			count = 0;
			note = null;
			SqlDataReader sqlDataReader = null;
			try
			{
				string cmdText = "SELECT ProductName, Count, Note FROM ExportedLicenses WHERE Name=@license";
				SqlConnection sqlConnection = new SqlConnection(Common.GetConnectionString());
				SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@license", licenseName);
				sqlConnection.Open();
				sqlDataReader = sqlCommand.ExecuteReader();
				sqlCommand.Parameters.Clear();
				if (sqlDataReader.HasRows && sqlDataReader.Read())
				{
					featureName = sqlDataReader.GetString(0);
					count = sqlDataReader.GetInt32(1);
					if (!sqlDataReader.IsDBNull(2))
					{
						note = sqlDataReader.GetString(2);
					}
				}
				sqlDataReader.Close();
			}
			catch (Exception)
			{
			}
			finally
			{
				if (sqlDataReader != null && !sqlDataReader.IsClosed)
				{
					sqlDataReader.Close();
				}
			}
		}
		internal string getFullPathForExportedLicenseBundle(string nameExported)
		{
			return Path.Combine(this.getSsLibPath(), string.Concat(new string[]
			{
				"Licenses\\",
				nameExported,
				"\\",
				nameExported,
				".xml"
			}));
		}
		internal string getSsLibPath()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(Settings.Default.SSLIBPath);
			if (registryKey != null)
			{
				return (string)registryKey.GetValue("RootPath");
			}
			return string.Empty;
		}
	}
}
