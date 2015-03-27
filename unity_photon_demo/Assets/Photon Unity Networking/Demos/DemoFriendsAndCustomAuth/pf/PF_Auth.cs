using UnityEngine;
using System;
using System.Collections;
//using ExitGames.Client.Photon.LoadBalancing;


public class PF_Auth : MonoBehaviour {
//	const string pfid = "BB7382A5A8239F8C";
//	const string auth = "BB7382A5A8239F8C-0-0-FD3A-8D2362A1C71D8E9-3E984DE6928BC35D.6582DE10F5A169FE";
//	string room = "";
//	const string photonAppId = " 5ee5d644-fcd7-40cc-8253-a2651e180254";
//	
//	const string PhotonRegion = "us";
//	//	const string TestUserName = "photontestuser";
//	//	const string TestUserPassword = "password";
//	//	const UInt64 TestUserId = 4525475528586832315;
//	
//	public bool connectInProcess = false;
//	public bool lobbyJoined = false;
//	public bool roomCreated = false;
//	public bool roomJoined = false;
//	private LoadBalancingClient photonClient;
//	private ClientState clientStateOnLastFrame;
//	private bool isClientConnectedAndReady = false;
//	private LoadBalancingClient.ServerConnection serverStateOnLastFrame;
//	
//	// Use this for initialization
//	void Awake () 
//	{
//		Application.runInBackground = true;
//		InitializePhotonClient(photonAppId, auth, pfid);
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//		CheckPhotonClientState();
//		photonClient.Service();
//	}
//	
//	void OnGUI()
//	{
//		GUILayout.Label(string.Format("Client State: {0}", clientStateOnLastFrame.ToString()));
//		GUILayout.Label(string.Format("IsClientConnectedAndReady: {0}", isClientConnectedAndReady.ToString()));
//		GUILayout.Label(string.Format("Server State: {0}", serverStateOnLastFrame.ToString()));
//		GUILayout.Label(string.Format("Room: {0}", this.photonClient.CurrentRoom));
//		GUILayout.Label(string.Format("Rooms: {0}", this.photonClient.RoomsCount));
//	}
//	
//	void InitializePhotonClient( string photonApplicationId, string pf_ticket, string pf_userId)
//	{
//		this.photonClient = new LoadBalancingClient()
//		{
//			AppId = photonApplicationId,
//			CustomAuthenticationValues = new AuthenticationValues()
//			{
//				AuthType = CustomAuthenticationType.Custom,
//				Secret = null,
//				AuthParameters = null,
//			}
//		};
//		
//		photonClient.CustomAuthenticationValues.SetAuthParameters(pf_userId, pf_ticket);
//		//bool connectInProcess = photonClient.ConnectToRegionMaster(PhotonRegion);
//		connectInProcess = photonClient.ConnectToRegionMaster(PhotonRegion);
//	}
//	
//	void CheckPhotonClientState()
//	{
//		if (photonClient.State == ClientState.JoinedLobby && this.lobbyJoined == false)
//		{
//			var photonPeer = photonClient.loadBalancingPeer;
//			this.lobbyJoined = true;
//			Debug.Log("Lobby Joined.");
//			CreatePhotonRoom();
//		}
//		else if((photonClient.State == ClientState.Disconnected && photonClient.DisconnectedCause == DisconnectCause.CustomAuthenticationFailed))
//		{
//			Debug.Log(photonClient.State.ToString());
//		}
//		else if(this.roomCreated == true && this.roomJoined == false && this.clientStateOnLastFrame == ClientState.Joined && this.serverStateOnLastFrame == LoadBalancingClient.ServerConnection.GameServer)
//		{
//			this.roomJoined = true;
//			Debug.Log("Calling Join Room");
//			
//			photonClient.OpJoinRoom(this.room);
//		}
//		else
//		{
//			//				Debug.Log("Client State:" + photonClient.State.ToString());
//			//				Debug.Log("Client Is ConnectedAndReady:" + photonClient.IsConnectedAndReady.ToString());
//			//				Debug.Log("Server Status:" + photonClient.Server.ToString());
//		}
//		
//		this.clientStateOnLastFrame = photonClient.State;
//		this.isClientConnectedAndReady = photonClient.IsConnectedAndReady;
//		this.serverStateOnLastFrame = photonClient.Server;
//	}
//	
//	
//	
//	void CreatePhotonRoom()
//	{
//		this.room = Guid.NewGuid().ToString();
//		photonClient.CustomAuthenticationValues.Secret = auth;
//		
//		roomCreated = photonClient.OpCreateRoom(
//			roomName: this.room,
//			roomOptions: new RoomOptions()
//			{
//			CheckUserOnJoin = true,
//			MaxPlayers = 4,
//			CustomRoomPropertiesForLobby = new string[] { "fooRoomLobbyProp", "barRoomLobbyProp" },
//			EmptyRoomTtl = 1000,
//			IsOpen = true,
//			IsVisible = true,
//		},
//		lobby: null
//		);
//	}
}


