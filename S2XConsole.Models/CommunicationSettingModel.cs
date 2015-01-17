using S2XConsole.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Xsl;
namespace S2XConsole.Models
{
	public class CommunicationSettingModel
	{
		private bool _isWWANExist;
		public bool IsWWANExist
		{
			get
			{
				return this._isWWANExist;
			}
		}
		public void GetSoftwareInformation(string softwareName, out string description, out string pathIcon, out bool isBackup, out string displayString, out bool isProvision, out int state, out string brokenReason, out string type, out string family, out string model)
		{
			description = null;
			pathIcon = null;
			isBackup = false;
			displayString = null;
			isProvision = false;
			state = 0;
			brokenReason = null;
			type = null;
			family = null;
			model = null;
			SqlDataReader sqlDataReader = null;
			try
			{
				List<SqlParameter> list = new List<SqlParameter>();
				list.Add(new SqlParameter("@softwareName", softwareName));
				string cmdText = "SELECT Description, IconPath, UidType, UidFamily, UidModel, Name FROM DragItems WHERE Name=@softwareName";
				SqlConnection sqlConnection = new SqlConnection(Common.GetConnectionString());
				sqlConnection.Open();
				SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
				sqlCommand.Parameters.AddRange(list.ToArray());
				sqlDataReader = sqlCommand.ExecuteReader();
				if (sqlDataReader.Read())
				{
					description = sqlDataReader.GetString(0);
					pathIcon = sqlDataReader.GetString(1);
					type = sqlDataReader.GetString(2);
					family = sqlDataReader.GetString(3);
					model = sqlDataReader.GetString(4);
					if (!sqlDataReader.IsDBNull(5))
					{
						displayString = sqlDataReader.GetString(5);
					}
				}
				sqlDataReader.Close();
				sqlCommand.Parameters.Clear();
				if (type != null && type.ToLower() == "package" && family != null && family.ToLower() == "backup")
				{
					isBackup = true;
				}
				if (type != null && type.ToLower() == "package" && family != null && family.ToLower() == "provision" && model != null && model.ToLower() == "bundle")
				{
					isProvision = true;
				}
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
		public List<BackupSetting> GetBackupSettings()
		{
			string description = null;
			string path = null;
			List<BackupSetting> list = new List<BackupSetting>();
			SqlDataReader sqlDataReader = null;
			try
			{
				string cmdText = "SELECT Name, Description, Path FROM DragItems WHERE UidType='Package' AND UidFamily='Backup'";
				SqlConnection sqlConnection = new SqlConnection(Common.GetConnectionString());
				sqlConnection.Open();
				SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
				sqlDataReader = sqlCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					string @string = sqlDataReader.GetString(0);
					if (!sqlDataReader.IsDBNull(1))
					{
						description = sqlDataReader.GetString(1);
					}
					if (!sqlDataReader.IsDBNull(2))
					{
						path = sqlDataReader.GetString(2);
					}
					BackupSetting item = new BackupSetting(@string, path, description);
					list.Add(item);
				}
				sqlDataReader.Close();
				sqlCommand.Parameters.Clear();
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
			return list;
		}
		internal string GetSettingsData(string path)
		{
			string text = path + "\\" + Settings.Default.SettingsFile;
			if (!File.Exists(text))
			{
				MessageBox.Show("The settings file " + text + " could not be found", Settings.Default.AppDisplayName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return string.Empty;
			}
			string uriString = "pack://application:,,/Models/" + Settings.Default.TransformFile;
			Uri uriResource = new Uri(uriString);
			Stream stream = Application.GetResourceStream(uriResource).Stream;
			StreamReader streamReader = new StreamReader(stream);
			StringReader input = new StringReader(streamReader.ReadToEnd());
			XmlTextReader stylesheet = new XmlTextReader(input);
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(text);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Unable to load " + text + " Exception - " + ex.Message, "Exception");
				string empty = string.Empty;
				return empty;
			}
			XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
			try
			{
				XsltSettings settings = new XsltSettings(true, false);
				xslCompiledTransform.Load(stylesheet, settings, new XmlUrlResolver());
			}
			catch (XmlException ex2)
			{
				MessageBox.Show(ex2.Message, "Exception Loading XSLT");
				string empty = string.Empty;
				return empty;
			}
			catch (XsltException ex3)
			{
				MessageBox.Show(ex3.Message, "Exception Loading XSLT");
				string empty = string.Empty;
				return empty;
			}
			catch (Exception ex4)
			{
				MessageBox.Show(ex4.Message, "Exception Loading XSLT");
				string empty = string.Empty;
				return empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			XmlWriterSettings settings2 = new XmlWriterSettings();
			XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings2);
			try
			{
				xslCompiledTransform.Transform(xmlDocument, xmlWriter);
			}
			catch (XmlException ex5)
			{
				MessageBox.Show("Exception attempting to transform data: " + ex5.Message, "Transform Exception");
				xmlWriter.Close();
				string empty = string.Empty;
				return empty;
			}
			catch (XsltException ex6)
			{
				string text2 = string.Empty;
				if (ex6.Message.Contains("_cache"))
				{
					text2 = "It appears that the selected backup may not have been created on or properly installed to this computer.\r\nPlease create or install the backup again before launching this tool.";
				}
				else
				{
					text2 = "XSLT Exception attempting to transform data: " + ex6.Message;
				}
				if (ex6.InnerException != null)
				{
					text2 = text2 + "\r\n\r\nError message: " + ex6.InnerException.Message;
				}
				if (ex6.LineNumber != 0)
				{
					text2 = text2 + " Line: " + ex6.LineNumber;
				}
				if (ex6.LinePosition != 0)
				{
					text2 = text2 + " Position: " + ex6.LinePosition;
				}
				MessageBox.Show(text2, "Transform Exception");
				xmlWriter.Close();
				string empty = string.Empty;
				return empty;
			}
			catch (Exception ex7)
			{
				MessageBox.Show("Exception attempting to transform data: " + ex7.Message, "Transform Exception");
				xmlWriter.Close();
				string empty = string.Empty;
				return empty;
			}
			xmlWriter.Close();
			XmlDocument xmlDocument2 = new XmlDocument();
			xmlDocument2.LoadXml(stringBuilder.ToString());
			if (xmlDocument2.SelectSingleNode(Settings.Default.WWANPath) != null)
			{
				this._isWWANExist = true;
			}
			else
			{
				this._isWWANExist = false;
			}
			if (xmlDocument2.DocumentElement.Name == "DevInfo")
			{
				return xmlDocument2.DocumentElement.InnerXml;
			}
			return xmlDocument2.InnerXml;
		}
	}
}
