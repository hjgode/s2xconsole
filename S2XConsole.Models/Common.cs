using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S2XConsole.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
namespace S2XConsole.Models
{
	public class Common
	{
		public const int PDF_COMPARISON_LIMIT = 4;
		private static bool inStandaloneMode = false;
		private static EventLog eventLog;
		public static bool IsInStandaloneMode
		{
			get
			{
				return Common.inStandaloneMode;
			}
			set
			{
				Common.inStandaloneMode = value;
			}
		}
		public static bool UsingJson
		{
			get;
			set;
		}
		public static string SettingsSourceName
		{
			get;
			set;
		}
		public static string DownloadUrl
		{
			get;
			set;
		}
		public static string GetConnectionString()
		{
			string result = string.Empty;
			try
			{
				string sQLInstanceKey = Settings.Default.SQLInstanceKey;
				RegistryKey localMachine = Registry.LocalMachine;
				RegistryKey registryKey = localMachine.OpenSubKey(sQLInstanceKey);
				string arg = (string)registryKey.GetValue(Settings.Default.SQLInstance);
				string arg2 = (string)registryKey.GetValue("DBName");
				registryKey.Close();
				result = string.Format(Settings.Default.ConnectionString, arg, arg2);
			}
			catch
			{
			}
			return result;
		}
		public static string GetFullFooterAddition()
		{
			string result = string.Empty;
			string settingsSourceName = Common.SettingsSourceName;
			string text = Common.DownloadUrl;
			if (text != null)
			{
				try
				{
					Uri uri = new Uri(text);
					string text2 = uri.AbsolutePath;
					if (text2.EndsWith("/"))
					{
						text2 = text2.TrimEnd(new char[]
						{
							'/'
						});
					}
					text = uri.AbsolutePath.Substring(uri.AbsolutePath.LastIndexOf("/") + 1);
				}
				catch (Exception)
				{
				}
			}
			if (string.IsNullOrEmpty(settingsSourceName) && !string.IsNullOrEmpty(text))
			{
				result = text;
			}
			else
			{
				if (!string.IsNullOrEmpty(settingsSourceName) && string.IsNullOrEmpty(text))
				{
					result = settingsSourceName;
				}
			}
			if (!string.IsNullOrEmpty(settingsSourceName) && !string.IsNullOrEmpty(text))
			{
				result = settingsSourceName + " | " + text;
			}
			return result;
		}
		public static string GetEntityConnectionString()
		{
			string result = string.Empty;
			try
			{
				string sQLInstanceKey = Settings.Default.SQLInstanceKey;
				RegistryKey localMachine = Registry.LocalMachine;
				RegistryKey registryKey = localMachine.OpenSubKey(sQLInstanceKey);
				string arg = (string)registryKey.GetValue(Settings.Default.SQLInstance);
				string arg2 = (string)registryKey.GetValue("DBName");
				registryKey.Close();
				result = string.Format(Settings.Default.EntityConnectionString, arg, arg2);
			}
			catch
			{
			}
			return result;
		}
		public static bool IsJsonString(string input)
		{
			bool result;
			try
			{
				JObject.Parse(input);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		public static bool IsJsonActionValid(string input)
		{
			try
			{
				JObject jObject = JObject.Parse(input);
				if (jObject["action"] != null)
				{
					JToken value = jObject["action"];
					string a = value.Value<string>();
					if (a != "set")
					{
						bool result = false;
						return result;
					}
				}
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			return true;
		}
		internal static string TransformJsonText(string input)
		{
			string result = input;
			try
			{
				JObject jObject = new JObject();
				JObject jObject2 = JObject.Parse(input);
				JToken jToken = jObject2.SelectToken("subsystems");
				if (jToken != null)
				{
					JProperty content = new JProperty("s", jToken);
					jObject.Add(content);
				}
				JToken jToken2 = jObject2.SelectToken("version");
				if (jToken2 != null)
				{
					JProperty content2 = new JProperty("v", jToken2);
					jObject.Add(content2);
				}
				else
				{
					JProperty content3 = new JProperty("v", new JValue(1f));
					jObject.Add(content3);
				}
				JToken jToken3 = jObject2.SelectToken("scanngo");
				if (jToken3 != null && jToken3.Type == JTokenType.Object)
				{
					JObject jObject3 = jToken3 as JObject;
					foreach (JToken current in jObject3.Children())
					{
						if (current.Type == JTokenType.Property)
						{
							JProperty jProperty = current as JProperty;
							if (jObject[jProperty.Name] != null)
							{
								continue;
							}
						}
						jObject.Add(current);
					}
				}
				result = jObject.ToString();
			}
			catch (Exception)
			{
			}
			return result;
		}
		internal static bool TestStringWithinJson(string input)
		{
			bool result;
			try
			{
				JValue content = new JValue(input);
				JObject jObject = new JObject();
				JProperty content2 = new JProperty("test", content);
				jObject.Add(content2);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		private static string oldTransformJsonText(string input)
		{
			string result = input;
			try
			{
				JObject jObject = JObject.Parse(input);
				string[] collection = new string[]
				{
					"result",
					"resultOf",
					"results",
					"uniqueID"
				};
				List<string> list = new List<string>(collection);
				foreach (string current in list)
				{
					if (jObject[current] != null)
					{
						jObject.Remove(current);
					}
				}
				Common.renameJsonToken(jObject, "version", "v");
				Common.renameJsonToken(jObject, "subsystems", "s");
				result = jObject.ToString();
			}
			catch (Exception)
			{
			}
			return result;
		}
		private static void renameJsonToken(JObject jo, string tokenName, string newTokenName)
		{
			JToken jToken = jo.SelectToken(tokenName);
			if (jToken != null)
			{
				JProperty content = new JProperty(newTokenName, jToken);
				jo.Remove(tokenName);
				jo.Add(content);
			}
		}
		public static string TrimJsonWhitespace(string input)
		{
			string result = input;
			try
			{
				JObject jObject = JObject.Parse(input);
				result = jObject.ToString(Newtonsoft.Json.Formatting.None, new JsonConverter[0]);
			}
			catch (Exception)
			{
			}
			return result;
		}
		public static bool IsXmlString(string input)
		{
			bool result;
			try
			{
				XDocument.Parse(input);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		public static bool IsXmlFile(string filePath)
		{
			bool result;
			try
			{
				XDocument.Load(filePath);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		public static string PrepareXmlContent(string xml)
		{
			string result = xml;
			try
			{
				XDocument node = XDocument.Parse(xml);
				XElement xElement = node.XPathSelectElement("/DevInfo");
				if (xElement != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (XElement current in xElement.Elements())
					{
						stringBuilder.Append(current.ToString());
					}
					result = stringBuilder.ToString();
				}
			}
			catch (Exception)
			{
			}
			return result;
		}
		public static string WrapXmlInDevInfo(string input)
		{
			string result = input;
			try
			{
				XDocument xDocument = XDocument.Parse(input);
				XElement root = xDocument.Root;
				if (root.Name != "DevInfo")
				{
					XElement xElement = new XElement("DevInfo");
					root.Remove();
					xDocument.Add(xElement);
					xElement.Add(root);
					result = xDocument.ToString();
				}
			}
			catch (XmlException)
			{
				result = "<DevInfo>" + input + "</DevInfo>";
			}
			catch (Exception)
			{
			}
			return result;
		}
		public static void WriteEntryToLog(string message, EventLogEntryType type)
		{
			try
			{
				if (Common.eventLog == null)
				{
					string source = "SmartSystems ScanNGo";
					string logName = "SmartSystems (Intermec)";
					if (!EventLog.SourceExists(source, Environment.MachineName))
					{
						EventSourceCreationData sourceData = new EventSourceCreationData(source, logName);
						EventLog.CreateEventSource(sourceData);
					}
					Common.eventLog = new EventLog(logName, Environment.MachineName, source);
					Common.eventLog.MaximumKilobytes = 1024L;
					Common.eventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
				}
				Common.eventLog.WriteEntry(message, type);
			}
			catch (ArgumentException ex)
			{
				if (ex.Message.StartsWith("Log entry string is too long"))
				{
					Common.WriteEntryToLog(message.Substring(0, 32766), type);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
