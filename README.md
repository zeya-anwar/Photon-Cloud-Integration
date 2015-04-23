PlayFab / Photon (Turnbased) Integration for Custom Authentication and Webhooks
========
1. Overview:
----
This project uses the Photon Cloud Turnbased plugin and demo to illustrate how functions in the PlayFab cloud script environment can be called automatically via Photon Cloud webhooks. This is a powerful combination of convenience and flexibility that can give your title event-driven game logic running in a secured cloud environment with access the the entire PlayFab Server API set.

The demo (DemoFriends-Scene provides) provides a simple and straightforward approach to getting your events working. We added a PlayFabIntegration.cs class to the Scripts GameObject, a component which contains our examples. 

2. Prerequisites:
----
Users should be very familiar with the content presented in our [Using Photon Guide](https://playfab.com/using-photon-playfab).

There are several factors which must be in place before triggering your web hooks. For the purposes of getting up and running as fast as possible, we have included several test resources that will need to be replaced by your own.

* PlayFab Title: You must have a title on PlayFab before proceeding. This demo uses title "FD3A", which you can use for basic testing. To view and modify the script or catalog, you will need to use your own Title Id
* Cloud Script: Your title must have a valid Cloud Script file uploaded to the servers tab. Ours can be found under "unity_photon_demo/CloudScript"
* Photon: You must activate the Photon capabilities on your title. This can be done by visiting  "PlayFab game manager > Servers > Photon"

##### Credentials: 
* PlayFab Title ID: this is the id of the PlayFab game in use, and can be found at the top of PlayFabIntegration.cs. It defaults to the demo script id "FD3A"
* PlayFab Authentication: For this example we are using a testing account tied to a testing device. You will want to change this if you wish authenticate with your own users.
  * PlayFab User ID: Obtained after authentication, provided to the Photon SDK for CustomAuthentication. 
  * PlayFab Authentication Ticket: Obtained after authentication, provided to the Photon SDK for CustomAuthentication.
* Photon App ID: A GUID that uniquely defines your title. This can be found by visiting "PlayFab game manager > Servers > Photon" and should be saved into the Photon Setup Wizard

Once you have these factors in place, you are ready to use webhooks.

3. Source Code & Key Repository Components:
----
* Repo/Unity3dProject_WithPhotonTurnbasedExample -- contains Photon's example project, with modifications to illustrate how PlayFab custom authentication can be used in conjunction with Photon. 
* Repo/CloudScript -- Contains copies of both the custom javascript file used in this demo as well as the initial "hello world" script (added to all newly provisioned titles)

4. Installation & Configuration Instructions:
----
Open with Unity3d, Play in the editor or Build to PC / Mac / Web.


5. Usage Instructions:
----
1) Open the Unity Project and load the scene: "DemoScene";
2) Run in the editor
3) Enter a nickname and click the button to to begin
4) Starting here we are loading in our example code from PlayFabIntegration.cs
5) Follow the on screen prompts to experience the game flow to trigger events which fire webhooks
6) On the game view, you can hide the prompts by clicking the button (these will reappear after leaving the game).

6. Troubleshooting:
----
For a complete list of available APIs, check out the [online documentation](http://api.playfab.com/Documentation/).

#### Contact Us
We love to hear from our developer community! 
Do you have ideas on how we can make our products and services better? 

Our Developer Success Team can assist with answering any questions as well as process any feedback you have about PlayFab services.

[Forums, Support and Knowledge Base](https://support.playfab.com/support/home)


7. Copyright and Licensing Information:
----
  Apache License -- 
  Version 2.0, January 2004
  http://www.apache.org/licenses/

  Full details available within the LICENSE file.

8. Version History:
----
* (v1.00) -- Initial upload
