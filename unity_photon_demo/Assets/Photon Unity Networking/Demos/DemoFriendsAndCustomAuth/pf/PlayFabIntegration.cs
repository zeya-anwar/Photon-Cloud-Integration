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
	private string team = string.Empty;
	
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
						GUILayout.Label("");
						GUILayout.Label("Photon Custom Authentication with Playfab Credentials:");
						GUILayout.Label("For the purposes of this demo, we will be using a testing device id for the login credentials. We provide these credentials to Photon, which then validates and permits entry into the MasterServer--MainLobby. From the main lobby a user can create and join rooms and games. Doing so will in turn fire the web hook associated with the player action.");
						GUILayout.Label("");
				GUILayout.Label("In the bottom left you fill find the Photon Client Connection Status. You will see this box update to reflect the many changes in the client state,  Click below to contine.");
						GUILayout.Label("");
						if(GUILayout.Button("Login to PlayFab!"))
						{
							LoginToPlayFab();
						}
					GUILayout.EndArea();	
				break;
				
				case GuiStates.createRoom:
					GUILayout.BeginArea(centralBox);	
						GUILayout.Label("");
						GUILayout.Label("PlayFab Authentication Successful!");
						GUILayout.Label("In the top left you can see the most up-to-date PlayFab PlayerStatistics for this user. You will see these stats change in real time as web hooks called to process game logic.");
						GUILayout.Label("");
						GUILayout.Label("Create & Join a Photon Room (Game):");
						GUILayout.Label("From the MainLobby, a player can join existing rooms or create a new one. When you create and join a room our cloud script is randomly assigning the player to a team (Red or Blu) and tracking the results in Player Stats. Watch the stats (top left) to see on which team you are assigned.");
						GUILayout.Label("");
						if(GUILayout.Button("Create a new Photon Room"))
						{
							this.activeState = GuiStates.loading;
							PhotonNetwork.CreateRoom(null);
						}
					GUILayout.EndArea();	
				break;
				
				case GuiStates.customEvents:
					GUILayout.BeginArea(centralBox);
						GUILayout.Label("");
						GUILayout.Label(string.Format("You are now in the game, welcome to the {0} team", this.team));
						GUILayout.Label("While the game is running your loops and mechanics, custom events can be raised to augment your logic flow. These events can have custom input parameters as well as access to the basic Photon room details. With a event system this flexible, nearly every game can benifit.");
						GUILayout.Label("");
						GUILayout.Label("Examples of Custom Events:");
						GUILayout.Label("");
						if(GUILayout.Button("Award ExperiencePoints, Ding!"))
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
							
						if(GUILayout.Button("Award MVP, you've earned it!"))
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
						if (GUILayout.Button("Leave Room & Start Over"))
						{
							OnLogOut();	
						}
						
					GUILayout.EndArea();	
				break;
				
				default:
					GUILayout.BeginArea(centralBox);
						GUILayout.Label("Awaiting Server...");
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
		if(result.UserStatistics.ContainsKey("BluTeamJoinedCount") && this.userStats.ContainsKey("BluTeamJoinedCount"))
		{
			if(result.UserStatistics["BluTeamJoinedCount"] > this.userStats["BluTeamJoinedCount"] )
			{
				this.team = "Blu";
			} 
		}
		else if(result.UserStatistics.ContainsKey("BluTeamJoinedCount") && !this.userStats.ContainsKey("BluTeamJoinedCount"))
		{
			this.team = "Blu";
		}
		
		if(result.UserStatistics.ContainsKey("RedTeamJoinedCount") && this.userStats.ContainsKey("RedTeamJoinedCount"))
		{
			if(result.UserStatistics["RedTeamJoinedCount"] > this.userStats["RedTeamJoinedCount"] )
			{
				this.team = "Red";
			} 
		}
		else if(result.UserStatistics.ContainsKey("RedTeamJoinedCount") && !this.userStats.ContainsKey("RedTeamJoinedCount"))
		{
			this.team = "Red";
		}
		
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
		this.team = string.Empty;
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.Disconnect();
	}
}

