start with arg '/standalone'

sample files see sample dir

dec using ilSpy22

function:

start with /standalone -> Common.IsInStandaloneMode=>TRUE
new OptionView
Common.IsInStandaloneMode=>TRUE

//you must check 'Communication Settings' and that will result in
	control = new StandaloneBrowse();
	and
	OptionView::this.isCommunicationSettingsPresent = true;
	and 
	this.viewModel.AddPage(new BarcodeOptionView(this.viewModel));
	then you MUST specify a file (JSON or XML)
	next we are in 
	StandAloneBrowse::GetBrowseData()

OptionView.cs->[X] Communication Settings => new StandaloneBrowse();
	OptionView.cs->this.isCommunicationSettingsPresent = true;
	OptionView.cs->this.viewModel.AddPage(new BarcodeOptionView(this.viewModel));

	Common.UsingJson = false; //valid for CommSett, only True for Android Downloads

NEXT->
	StandaloneBrowse->GetBrowseData()
		bool flag = Common.IsJsonString(text);
		bool flag2 = Common.IsXmlString(text);
		if(flag) //isJsonString
			Common.UsingJson = true;
			text = Common.TransformJsonText(text);	//convert read file text to Transformed json text
			Common.SettingsSourceName = fileInfo.Name;
			return text
		else
			Common.UsingJson = false;
			text = Common.PrepareXmlContent(text);	//removes <xml> line

	StandaloneBrowse->this.fileData = browseData; //this is the transformed text (JSON or XML)
	StandaloneBrowse->PageData returns fileData

NEXT-> with XML
	MainViewModel->GetBarCodeData() returns pageData  //transformed JSON or XML
	if (!Common.UsingJson)
	{
		text = Common.WrapXmlInDevInfo(text); //executed for XML, wraps with <DevInfo>
	}

	S2X.S2X Update()->Update() called
		string text = this.PageData(); //empty
		string text2 = this.InputData; //transformed JSON or XML (PageData)
		->shows estimated number of barcodes

	entered password and notes, no Start barcode and no Reboot selected
	S2X.S2X Update()->Update() called
		string text = this.PageData(); // "<Subsystem Name=\"SS_Client\"><Group Name=\"Download\"><Field Name=\"ProcessNow\">True</Field></Group></Subsystem>"
		string text2 = this.InputData; // transformed wrapped XML
		if (!string.IsNullOrEmpty(text) && !Common.UsingJson)
			adds text to text2
		s2X.IsNoReboot = this.IsNoReboot;
		s2X.IsNoStartBarcode = this.IsNoStartBarcode;
		(generates Images with barcodes)
		saves text2: File.WriteAllText(tempFileName, text2);

NEXT-> with JSON
	GetBrowseData()
		bool flag = Common.IsJsonString(text); //TRUE  "{\r\n  \"action\": \"set\",\r\n  \"subsystems\": {\r\n    \"WiFi\": {\r\n      \"base\": 0,    \r\n      \"v\": 0,      \r\n      \"mode\": \"full\",\r\n      \"nets\": [\r\n        {\r\n          \"ssid\": \"\\\"MyNetworkName1\\\"\",\r\n          \"pri\": 3,\r\n          \"hid\": 0,\r\n          \"auth\": \"OPEN,SHARED\",\r\n          \"km\": \"NONE\",\r\n          \"wepk\": [\r\n             \"*\",\r\n             \"\",\r\n             \"\",\r\n             \"\"\r\n          ]\r\n        },\r\n        {\r\n          \"ssid\": \"\\\"MyNetworkName2\\\"\",\r\n          \"pri\": 1,\r\n          \"hid\": 0,\r\n          \"auth\": \"\",\r\n          \"km\": \"WPA_PSK\",\r\n          \"pskey\": \"*\"\r\n        }\r\n      ]\r\n    }\r\n  },\r\n  \"version\": \"1.0\"\r\n}\r\n"
		bool flag2 = Common.IsXmlString(text); //false
		string result;
		if (flag || flag2)
		{
			if (flag) //true for json
			{
				if (!Common.IsJsonActionValid(text))
				{
					...
				}
				Common.UsingJson = true;
				text = Common.TransformJsonText(text); // transforms to : "{\r\n  \"s\": {\r\n    \"WiFi\": {\r\n      \"base\": 0,\r\n      \"v\": 0,\r\n      \"mode\": \"full\",\r\n      \"nets\": [\r\n        {\r\n          \"ssid\": \"\\\"MyNetworkName1\\\"\",\r\n          \"pri\": 3,\r\n          \"hid\": 0,\r\n          \"auth\": \"OPEN,SHARED\",\r\n          \"km\": \"NONE\",\r\n          \"wepk\": [\r\n            \"*\",\r\n            \"\",\r\n            \"\",\r\n            \"\"\r\n          ]\r\n        },\r\n        {\r\n          \"ssid\": \"\\\"MyNetworkName2\\\"\",\r\n          \"pri\": 1,\r\n          \"hid\": 0,\r\n          \"auth\": \"\",\r\n          \"km\": \"WPA_PSK\",\r\n          \"pskey\": \"*\"\r\n        }\r\n      ]\r\n    }\r\n  },\r\n  \"v\": \"1.0\"\r\n}"
			}
		
	getBarcodeData()
		...
		text = MainViewModel.processJsonStrings(list); // only one entry: "{\r\n  \"s\": {\r\n    \"WiFi\": {\r\n      \"base\": 0,\r\n      \"v\": 0,\r\n      \"mode\": \"full\",\r\n      \"nets\": [\r\n        {\r\n          \"ssid\": \"\\\"MyNetworkName1\\\"\",\r\n          \"pri\": 3,\r\n          \"hid\": 0,\r\n          \"auth\": \"OPEN,SHARED\",\r\n          \"km\": \"NONE\",\r\n          \"wepk\": [\r\n            \"*\",\r\n            \"\",\r\n            \"\",\r\n            \"\"\r\n          ]\r\n        },\r\n        {\r\n          \"ssid\": \"\\\"MyNetworkName2\\\"\",\r\n          \"pri\": 1,\r\n          \"hid\": 0,\r\n          \"auth\": \"\",\r\n          \"km\": \"WPA_PSK\",\r\n          \"pskey\": \"*\"\r\n        }\r\n      ]\r\n    }\r\n  },\r\n  \"v\": \"1.0\"\r\n}"
		text = Common.TrimJsonWhitespace(text);
			=>text = "{\"s\":{\"WiFi\":{\"base\":0,\"v\":0,\"mode\":\"full\",\"nets\":[{\"ssid\":\"\\\"MyNetworkName1\\\"\",\"pri\":3,\"hid\":0,\"auth\":\"OPEN,SHARED\",\"km\":\"NONE\",\"wepk\":[\"*\",\"\",\"\",\"\"]},{\"ssid\":\"\\\"MyNetworkName2\\\"\",\"pri\":1,\"hid\":0,\"auth\":\"\",\"km\":\"WPA_PSK\",\"pskey\":\"*\"}]}},\"v\":\"1.0\"}"

	S2X.S2X Update()->Update() called
		string text = this.PageData(); //empty on first call
		string text2 = this.InputData; // "{\"s\":{\"WiFi\":{\"base\":0,\"v\":0,\"mode\":\"full\",\"nets\":[{\"ssid\":\"\\\"MyNetworkName1\\\"\",\"pri\":3,\"hid\":0,\"auth\":\"OPEN,SHARED\",\"km\":\"NONE\",\"wepk\":[\"*\",\"\",\"\",\"\"]},{\"ssid\":\"\\\"MyNetworkName2\\\"\",\"pri\":1,\"hid\":0,\"auth\":\"\",\"km\":\"WPA_PSK\",\"pskey\":\"*\"}]}},\"v\":\"1.0\"}"

		noReboot and noStartBarcode is always set TRUE for JSON

		File.WriteAllText(tempFileName, text2); //text2= "{\"s\":{\"WiFi\":{\"base\":0,\"v\":0,\"mode\":\"full\",\"nets\":[{\"ssid\":\"\\\"MyNetworkName1\\\"\",\"pri\":3,\"hid\":0,\"auth\":\"OPEN,SHARED\",\"km\":\"NONE\",\"wepk\":[\"*\",\"\",\"\",\"\"]},{\"ssid\":\"\\\"MyNetworkName2\\\"\",\"pri\":1,\"hid\":0,\"auth\":\"\",\"km\":\"WPA_PSK\",\"pskey\":\"*\"}]}},\"v\":\"1.0\"}"
