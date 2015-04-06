README for PlayFab / Photon Integration for Custom Authentication and Webhooks
=====================================================================================================================================

Overview:
-------------------------------------------------------------------------------------------------------------------------------------
This project uses the Photon Cloud Turn Based plugin and demo to illustrate how functions in the PlayFab cloud script environment can be called automatically from the Photon Turn Based web hooks service. This is a powerful combination of convenience and flexibility that can give your title event-driven game logic running in a secured cloud environment with access the the entire PlayFab Client and Server API set.

The demo (DemoFriends-Scene provides) provides a simple and straightforward approach to getting your events working. We added a PlayFabIntegration.cs class to the Scripts GameObject, a component which contains our examples. 


Prerequisites: / Credentials:
-------------------------------------------------------------------------------------------------------------------------------------
There are several factors which must be in place before triggering your web hooks. For the purposes of getting up and running as fast as possible, we have included several test resources that will need to be replaced by your own.

Prerequisites:
+ PlayFab Title: You must have a title on PlayFab before proceeding. This demo uses title "FD3A"
+ Cloud Script: Your title must have a valid Cloud Script file uploaded to the servers tab. Ours can be found under "unity_photon_demo/CloudScript"
+ Photon: You must activate the Photon capabilities on your title. This can be done by visiting  "PlayFab game manager > Servers > Photon"

Credentials: 
+ PlayFab Title Id: this is the id of your PlayFab game. We are using "FD3A", and this can be found at the top of PlayFabIntegration.cs
+ PlayFab Authentication: For this example we are using a testing account tied to a testing device. You will want to change this if you wish authenticate with your own users.
    ++ PlayFab User Id: Obtained after authentication, provided to the Photon SDK for CustomAuthentication. 
    ++ PlayFab Authentication Ticket: Obtained after authentication, provided to the Photon SDK for CustomAuthentication.
+ Photon App Id: A GUID that uniquely defines your title. This can be found by visiting "PlayFab game manager > Servers > Photon" and should be saved into the Photon Setup Wizard

Once you have these factors in place, you are ready to web hook. 

Starting the Demo:
-------------------------------------------------------------------------------------------------------------------------------------
1) Open the Unity Project and load the scene: "DemoScene";
2) Run in the editor
3) Enter a nickname and click the button to to begin
4) Starting here we are loading in our example code from PlayFabIntegration.cs
5) Follow the on screen prompts to experience the game flow to trigger events which fire webhooks
6) On the game view, you can hide the prompts by clicking the button (these will reappear after leaving the game.



Testing Info:
=====================================================================================================================================
Title: FD3A
Photon App Id: 5ee5d644-fcd7-40cc-8253-a2651e180254
Android Device ID: a931f26995380c5a

Left To Do:
-------------------------------------------------------------------------------------------------------------------------------------
+ Address Bugs / questions I posted in Asana
+ Need to document this somewhere:
    Cloud Script Parameter "Args" Object:
    {
        "ActorNr": 2,
        "AppVersion": "",
        "AppId": "b3360ec5-e44c-4fbb-866b-04ec48ae1780",
        "Data": {
            "headshots": 2,
            "message": "bf52ecd4-3e1f-4b41-9828-f750cbde104c",
            "kills": 5
        },
        "GameId": "57ebe739-0c53-4996-b6f5-344e7c5c35b3",
        "Region": "EU",
        "State": {
            "ActorCounter": 2,
            "ActorList": [
                {
                    "ActorNr": 2,
                    "UserId": "3ECDB804292429BB",
                    "Username": "",
                    "Binary": "RGIAAAEBRAAAAAFi/3MAAA==",
                    "DEBUG_BINARY": {
                        "1": {
                            "255": ""
                        }
                    }
                }
            ],
            "CheckUserOnJoin": true,
            "DeleteCacheOnLeave": true,
            "EmptyRoomTTL": 1000,
            "IsOpen": true,
            "IsVisible": true,
            "LobbyId": null,
            "LobbyType": 0,
            "LobbyProperties": [
                "fooRoomLobbyProp",
                "barRoomLobbyProp"
            ],
            "MaxPlayers": 4,
            "PlayerTTL": 0,
            "SuppressRoomEvents": false,
            "Slice": 0,
            "Binary": {
                "18": "RAAAAARi+nkAAnMAEGZvb1Jvb21Mb2JieVByb3AAEGJhclJvb21Mb2JieVByb3Bi/2IEYv5vAWL9bwE="
            },
            "DEBUG_PROPERTIES_18": {
                "250": [
                    "fooRoomLobbyProp",
                    "barRoomLobbyProp"
                ],
                "253": true,
                "254": true,
                "255": 4
            }
        },
        "Type": "Event",
        "UserId": "3ECDB804292429BB",
        "Username": ""
    }



