using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using ExitGames.Client.Photon.LoadBalancing;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Internal;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayFabIntegration : MonoBehaviour {
	public int gui_depth = 0;
	public Texture2D pf_logo;
	public Rect logoDims;
	
	// using PlayFab testing account and title
	const string deviceId = "a931f26995380c5a";
	const string playfabTitleId = "FD3A";
	
	public enum GuiStates {off=0, login=1, createRoom=2, customEvents=3, loading=4 }
	public GuiStates activeState = GuiStates.off; 
	
	private bool isAuthenticated = false;
	private bool isJoined = false;
	private Dictionary<string, int> userStats = new Dictionary<string, int>();
	//private Dictionary<string, int> userStats = new Dictionary<string, int>();
	
	// Use this for initialization
	void Start()
	{
		PlayFabSettings.TitleId = playfabTitleId;
	}
	
	void OnGUI()
	{
		if(this.activeState == GuiStates.loading && PhotonNetwork.connectionStateDetailed == PeerState.Authenticated && this.isAuthenticated == false)
		{
			this.isAuthenticated = true;
			this.activeState = GuiStates.createRoom;
		}
		else if(this.activeState == GuiStates.loading && PhotonNetwork.connectionStateDetailed == PeerState.Joined && this.isJoined == false)
		{
			this.isJoined = true;
			this.activeState = GuiStates.customEvents;
			StartCoroutine(GetUserData());
		}
	
		if(this.activeState != GuiStates.off)
		{
			GUI.depth = this.gui_depth;
			
			GUI.Box(new Rect(0,0,Screen.width, Screen.height), "");
			if(pf_logo != null)
			{	
				GUI.DrawTexture(new Rect(Screen.width/2 - this.logoDims.width/2, 0, this.logoDims.width, this.logoDims.height), this.pf_logo);		
			}
			
			if(this.activeState == GuiStates.createRoom || this.activeState == GuiStates.customEvents || this.activeState == GuiStates.loading)
			{
				GUILayout.BeginArea(new Rect(5,5, Screen.width*.25f, Screen.height*.20f));
					string redTeamValue =  this.userStats.ContainsKey("RedTeamJoinedCount") ? this.userStats["RedTeamJoinedCount"].ToString() : "...";
					string bluTeamValue =  this.userStats.ContainsKey("BluTeamJoinedCount") ? this.userStats["BluTeamJoinedCount"].ToString() : "...";
					string mvpValue =  this.userStats.ContainsKey("MVPsWonCount") ? this.userStats["MVPsWonCount"].ToString() : "...";
					string expValue =  this.userStats.ContainsKey("ExperiencePoints") ? this.userStats["ExperiencePoints"].ToString() : "...";
					
					GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						GUILayout.Label("PlayFab Player Stats:");
						GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					
					GUILayout.Label(string.Format("RedTeamJoinedCount: {0}", redTeamValue));
					GUILayout.Label(string.Format("BluTeamJoinedCount: {0}", bluTeamValue));
					GUILayout.Label(string.Format("MVPsWonCount: {0}", mvpValue));
					GUILayout.Label(string.Format("ExperiencePoints: {0}", expValue));
				GUILayout.EndArea();
			}
			
			GUILayout.BeginArea(new Rect(5,Screen.height*.85f, Screen.width*.5f, Screen.height*.15f));
				GUILayout.BeginHorizontal();
					GUILayout.Label("Photon Connection Status:");
				GUILayout.EndHorizontal();
				
				GUILayout.Label(string.Format("IsConnected: {0}, IsReady: {1}", PhotonNetwork.connected.ToString(), PhotonNetwork.connectedAndReady.ToString()));
				GUILayout.Label(string.Format("ConnectionState: {0}", PhotonNetwork.connectionStateDetailed.ToString()));
				if(PhotonNetwork.room != null)
				{
					GUILayout.Label(string.Format("CurrentRoom: {0}", PhotonNetwork.room.name));
				}
			GUILayout.EndArea();

			Rect centralBox = new Rect(Screen.width/4, this.logoDims.height, Screen.width/2, Screen.height/2);
			
			switch(this.activeState)
			{
				case GuiStates.login:
					 GUILayout.BeginArea(centralBox);
						GUILayout.Label("Photon Custom Authentication with Playfab Credentials:");
						GUILayout.Label("For the purposes of this demo, we will be using a static device id for login. We provide these credentials to Photon, which then checks the credentials and permits entry into the MasterServer--MainLobby.");
						GUILayout.Label("");
						GUILayout.Label("REQUIRED INFO: Photon App Id -- \"Window > Photon Unity Networking > PUN Wizard\"");
						GUILayout.Label("REQUIRED INFO: PlayFab User Id -- Obtained after logging into PlayFab");
						GUILayout.Label("REQUIRED INFO: PlayFab Authentication Ticket -- Obtained after logging into PlayFab");
						GUILayout.Label("REQUIRED INFO: PlayFab Title Id -- Added in the inspector for PlayFabIntegration.cs");
						GUILayout.Label("");
						if(GUILayout.Button("Login to PlayFab!"))
						{
							LoginToPlayFab();
						}
					GUILayout.EndArea();	
				break;
				
				case GuiStates.createRoom:
					GUILayout.BeginArea(centralBox);	
						GUILayout.Label("Starting a Photon Room (Game):");
						GUILayout.Label("From the MainLobby, a player can join existing rooms or create a new one. For the purposes of this simple demo, you can create a new room by click.");
						GUILayout.Label("");
						if(GUILayout.Button("Create a new Photon Room"))
						{
							this.activeState = GuiStates.loading;
							PhotonNetwork.CreateRoom(null);
							//string roomName = Guid.NewGuid().ToString();
							//PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), new TypedLobby());
							//PhotonNetwork.JoinRoom(roomName);
						}
					GUILayout.EndArea();	
				break;
				
				case GuiStates.customEvents:
					GUILayout.BeginArea(centralBox);
						GUILayout.Label("Starting a Photon Room (Game):");
						GUILayout.Label("Starting a Photon Room (Game):");
						
						GUILayout.Label("CUSTOM EVENTS");
						if(GUILayout.Button("Award ExperiencePoints"))
						{
							// Trigger Custom Event
							
							int expAward = UnityEngine.Random.Range(5000,10000);
							Debug.Log("EXP: " + expAward);
							PhotonNetwork.networkingPeer.OpRaiseEvent(
								eventCode: 15,
							customEventContent: new Hashtable() { { "eventType", "exp" }, { "amt", expAward } },
							sendReliable: true,
							raiseEventOptions: new RaiseEventOptions()
							{
								ForwardToWebhook = true,
							});
							StartCoroutine(GetUserData(1.25f));
						}
							
						if(GUILayout.Button("Award MVP"))
						{
							// Trigger Custom Event
							PhotonNetwork.networkingPeer.OpRaiseEvent(
								eventCode: 15,
								customEventContent: new Hashtable() { { "eventType", "mvp" }},
								sendReliable: true,
								raiseEventOptions: new RaiseEventOptions()
								{
									ForwardToWebhook = true,
								});
								StartCoroutine(GetUserData(1.25f));
						}
						GUILayout.Label("");
						if (GUILayout.Button("Leave Room"))
						{
							OnLogOut();	
						}
						
					GUILayout.EndArea();	
				break;
				
				default:
					GUILayout.BeginArea(centralBox);
						GUILayout.Label("Loading...");
					GUILayout.EndArea();
				break;
			}	
		}
	}
	
	void LoginToPlayFab()
	{
		Debug.Log("Using demo device id");
		LoginWithAndroidDeviceIDRequest request = new LoginWithAndroidDeviceIDRequest();
		request.AndroidDeviceId = deviceId;
		request.TitleId = playfabTitleId;
		request.CreateAccount = true;
		PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginResult, OnPlayFabError);
		this.activeState = GuiStates.loading;
	}
	
	void OnLoginResult(LoginResult result)
	{
		Debug.Log(result.PlayFabId);
		StartCoroutine(GetUserData());

		PhotonNetwork.AuthValues = new AuthenticationValues();
		PhotonNetwork.AuthValues.SetAuthParameters(result.PlayFabId, result.SessionTicket);
		PhotonNetwork.ConnectUsingSettings("1.0");
	}
	
	IEnumerator GetUserData(float sec = 0)
	{
		yield return new WaitForSeconds(sec);
		GetUserStatisticsRequest request = new GetUserStatisticsRequest();
		PlayFabClientAPI.GetUserStatistics(request, OnGetUserDataSuccess, OnPlayFabError);
	}
	
	void OnGetUserDataSuccess(GetUserStatisticsResult result)
	{
		this.userStats = result.UserStatistics;
	}
	
	void OnPlayFabError(PlayFabError error)
	{
		Debug.Log(error.ErrorMessage);
	}

	public void SetStateAuthInput()
	{
		this.activeState = GuiStates.login;
	}
	
	void OnLogOut()
	{
		this.isAuthenticated = false;
		this.isJoined = false;
		this.activeState = GuiStates.login;
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.Disconnect();
	}
}

