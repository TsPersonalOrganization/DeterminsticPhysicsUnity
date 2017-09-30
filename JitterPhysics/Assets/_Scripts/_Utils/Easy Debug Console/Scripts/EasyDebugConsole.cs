#pragma warning disable 0618

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EasyDebugConsole : MonoBehaviour
{
	private static EasyDebugConsole Instance = null; // Our instance to allow this script to be called without a direct connection.


	[Header("Settings")]
	public bool	visible = true;		// Enable toggle
	public bool collapse = true;	// collapsed toggle
	public int countPerPage = 100;
	public int currentPage = 1;
	public int pageCount = 1;

	[Header("Toggle Key")]
	public KeyCode keyCode;			// Custom visibility toggle

	[Header("UI Elements")]
	public Text ConsoleText;			// UI text Element
	public GameObject PanelObject;	// UI panel Element
	public Text CollapseText;
	

	
	//Lists
	private List<Log> logs = new List<Log> ();			//List with Debug Messages

	struct Log{
		public string message;
		public string stackTrace;
		public LogType type;
	}

	public static EasyDebugConsole instance {
		get {
			if (Instance == null) {
				Instance = FindObjectOfType (typeof(EasyDebugConsole)) as EasyDebugConsole;
			}
			return Instance;
		}
	}

	private void OnEnable (){
		Application.logMessageReceived += AddMesage;
	}

	private void OnDisable (){
		Application.logMessageReceived -= AddMesage;
	}
	
	private void Start (){
		Initialisation ();
	}
	
	private void Initialisation (){
		visible = true;
		PanelObject.SetActive (true);
	}

	private void Update (){
		//Toggle Framerate Counter On/Off
		if (Input.GetKeyUp (keyCode)) {
			if (!visible) {

				Initialisation ();
			} else {
				Invisible ();
			}
		}

	}

	//+++++++++ INTERFACE FUNCTIONS ++++++++++++++++++++++++++++++++
	public static bool isVisible {                                      
		get {
			return EasyDebugConsole.instance.visible;
		}
		
		set {
			EasyDebugConsole.instance.visible = value;
			if (value == true) {
				EasyDebugConsole.instance.Initialisation ();
			} else if (value == false) {
				EasyDebugConsole.instance.Invisible ();
			}
		}
	}

	public static void Clear (){
		EasyDebugConsole.instance.ClearMessages ();
	}

	public static void Collapse (){
		EasyDebugConsole.instance.CollapseToggle ();
	}
	//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


	//---------- void AddMesage(string message, string color) ------
	//Adds a mesage to the list
	//--------------------------------------------------------------
	private void AddMesage (string message, string stackTrace, LogType type){

		bool latest = currentPage == pageCount;

		if(latest)
		{
			RefreshCurrentPageLogs();
		}

		logs.Add (new Log () {
			message = message,
			stackTrace = stackTrace,
			type = type,
		});

		RefreshPageCount();

		CollapseText.text = GetPageString ();

		if(latest && currentPage < pageCount)
		{
			currentPage = pageCount;
			RefreshCurrentPageLogs();

		}


		//Display ();
	}

	//----------- void ClearMessages() ------------------------------
	// Clears the messages from the screen and the lists
	//---------------------------------------------------------------
	public void ClearMessages (){
		logs.Clear ();
		ConsoleText.text = "";
	}

	//-------- void CollapseToggle() ---------------------------------
	// Toggles the Collapse function
	//----------------------------------------------------------------
	public void CollapseToggle (){
		if (collapse) {
			collapse = false;
			ClearMessages();
		} else {
			collapse = true;
			ClearMessages();
		}
	}

	//-------- void OnDisable() -------------------------------------
	// Clears and hides the Console
	//---------------------------------------------------------------
	public void Invisible (){
		visible = false;
		PanelObject.SetActive (false);
		//logs = new List<Log> ();
		//ConsoleText.text = "";
	} 

	//---------- void Display() --------------------------------------
	// Displays the list and handles coloring
	//----------------------------------------------------------------
	private void Display (){
		//if visible
		if (visible) {

			var log = logs [logs.Count - 1];

			if (collapse && logs.Count > 1) {
				var messageSameAsPrevious = log.message == logs [logs.Count - 2].message;
		
				if (messageSameAsPrevious) {
					return;
				}
			}

//			if (ConsoleText.text.Length > 10000) {
//				ClearMessages ();
//			}

			//Set the color tags
			string _colorTagStart = logTypeColors [log.type];
			string _colorTagEnd = "</color>";

			ConsoleText.text += _colorTagStart + "" + (string)logs [logs.Count - 1].message + "" + _colorTagEnd + "\n";

			Resize ();

		} else {
			ConsoleText.text = "";
		}

		CollapseText.text = GetPageString ();
	}
	
	static readonly Dictionary<LogType, string> logTypeColors = new Dictionary<LogType, string> (){
		{ LogType.Assert, "<color=#cccccc>" },
		{ LogType.Error, "<color=#ff0000ff>" },
		{ LogType.Exception, "<color=#ff0000ff>" },
		{ LogType.Log, "<color=#ffffffff>" },
		{ LogType.Warning, "<color=#ffff00ff>" },
	};


	private void Resize (){
		RectTransform rectObject = ConsoleText.GetComponent<RectTransform> ();
		rectObject.sizeDelta = new Vector2 (rectObject.sizeDelta.x, -540 +(15f * viewLogs.Count ));
	}

	#region ex

	private List<Log> viewLogs
	{
		get{
			currentPage = currentPage<1?1:currentPage;

			int start = (currentPage-1)*countPerPage;

			int end = (logs.Count-1 - start)>countPerPage?(start+countPerPage-1):logs.Count-1;	

			List<Log> temp = new List<Log>();

			if(end >= start)
			{
				for(int i =start;i<end;i++)
				{
					temp.Add (logs[i]);
				}
			}

			return temp;

		}
	}

	private void RefreshCurrentPageLogs()
	{
		List<Log> temp = viewLogs;
		ConsoleText.text = "";

		for( int i =0;i<temp.Count; i++){

			string _colorTagStart = logTypeColors [temp[i].type];

			string _colorTagEnd = "</color>";
		
			ConsoleText.text += _colorTagStart + "" + (string)temp[i].message + "" + _colorTagEnd + "\n";

		}
		Resize();

	}

	public void PrevPage()
	{
		if(currentPage >1)
		{	
			currentPage--;

			RefreshCurrentPageLogs();
		}
	}

	public void NextPage()
	{
		if(currentPage<pageCount)
		{
			currentPage++;

			RefreshCurrentPageLogs();
		}
	}

	public void SetCountPerPage(int count)
	{
		countPerPage = count;

		RefreshPageCount();
		
		CollapseText.text = GetPageString ();
		
		currentPage = pageCount;

		RefreshCurrentPageLogs();
			
	}


	private void RefreshPageCount()
	{
		countPerPage = countPerPage<1?1:countPerPage;
		
		pageCount = logs.Count/countPerPage+(logs.Count % countPerPage>0?1:0);
	}

	private string GetPageString()
	{
		return string.Format (" CurrentPage{0}, PageCount{1}",currentPage, pageCount);
	}

	#endregion
}