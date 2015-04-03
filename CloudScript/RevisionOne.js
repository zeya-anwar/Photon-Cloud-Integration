///////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Welcome to your first Cloud Script revision. 
// The examples here provide a quick introduction to using Cloud Script and some
// ideas about how you might use it in your game.
//
// Feel free to use this as a starting point for your game server logic, or to replace it altogether. 
// If you have any questions or need advice on where to begin, 
// check out the resources at https://playfab.com/cloud-script or email our 
// Customer Success team at devrel@playfab.com.
//
// - The PlayFab Team
//
///////////////////////////////////////////////////////////////////////////////////////////////////////


// This is a Cloud Script handler function. It runs in the PlayFab cloud and 
// has full access to the PlayFab GameServer API 
// (https://api.playfab.com/Documentation/Server). You can invoke the function 
// from your game client by calling the RunCloudScript API 
// (https://api.playfab.com/Documentation/Client/method/RunCloudScript) and 
// specifying "helloWorld" as the ActionId.
handlers.helloWorld = function (args) {

    // currentPlayerId is initialized to the PlayFab ID of the player logged-in on the game client. 
    // Cloud Script handles authenticating the player automatically.
    var message = "Hello " + currentPlayerId + "!";

    // You can use the log object to output debugging and informational statements 
    // that you can view in the Logs tab in the Servers section of the Game Manager. 
    // Log has three functions corresponding to logging level: debug, info, and error.
    log.info(message);

    // Whatever value you return from a CloudScript handler function is passed back 
    // to the game client. It is set in the Results property of the object returned by the 
    // RunCloudScript API (https://api.playfab.com/Documentation/Client/method/RunCloudScript).
    return { messageValue: message };
}


handlers.completedLevel = function (args) {

    // args is the object passed in to RunCloudScript from the client. 
    // It contains whatever properties you want to pass into your Cloud Script function. 
    // In this case it contains information about the level a player has completed.
    var level = args.levelName;
    var monstersKilled = args.monstersKilled;

    // The server object has functions for each PlayFab server API 
    // (https://api.playfab.com/Documentation/Server). It is automatically 
    // authenticated as your title and handles all communication with 
    // the PlayFab API, so you don’t have to write the code to make web requests. 
    var updateUserDataResult = server.UpdateUserInternalData({
	    PlayFabId: currentPlayerId,
	    Data: {
	        lastLevelCompleted: level
	    }
	});

    log.debug("Set lastLevelCompleted for player " + currentPlayerId + " to " + level);
    var statName = level + "_monster_kills";

    server.UpdateUserStatistics({
        PlayFabId: currentPlayerId,
        UserStatistics: {
            statName: monstersKilled
        }
    });

    log.debug("Updated " + statName + " stat for player " + currentPlayerId + " to " + monstersKilled);
}


// In addition to the Cloud Script handlers, you can define your own functions and call them from your handlers. 
// This makes it possible to share code between multiple handlers and to improve code organization.
handlers.UpdatePlayerMove = function (args) {
    var validMove = processPlayerMove(args);
    return { validMove: validMove };
}

function processPlayerMove(playerMove) {
    var now = Date.now();

    var playerData = server.GetUserInternalData({
        PlayFabId: currentPlayerId,
        Keys: ["last_move_timestamp"]
    });

    var lastMoveTimestamp = playerData.Data["last_move_timestamp"];

    if (lastMoveTimestamp) {
        var lastMoveTime = Date.parse(lastMoveTimestamp);
        var timeSinceLastMoveInSeconds = (now - lastMoveTime) / 1000;

        if (timeSinceLastMoveInSeconds < 60) {
            log.error("Invalid move - time since last move: " + timeSinceLastMoveInSeconds + "s.")
            return false;
        }
    }

    var playerStats = server.GetUserStatistics({
        PlayFabId: currentPlayerId
    }).UserStatistics;

    if (playerStats.movesMade)
        playerStats.movesMade += 1;
    else
        playerStats.movesMade = 1;

    server.UpdateUserStatistics({
        PlayFabId: currentPlayerId,
        UserStatistics: playerStats
    });

    server.UpdateUserInternalData({
        PlayFabId: currentPlayerId,
        Data: {
            last_move_timestamp: new Date(now).toUTCString()
        }
    });

    return true;
}


//
// Photon Webhooks Integration
//
// The following functions are examples of Photon Cloud Webhook handlers. 
// When you enable Photon integration in the Game Manager, your Photon applications 
// are automatically configured to authenticate players using their PlayFab accounts 
// and to fire events that trigger your CloudScript Webhook handlers, if defined. 
// This makes it easier than ever to incorporate server logic into your game.
//
//For more information, see https://playfab.com/using-photon-playfab
//

// Triggered automatically when a Photon room is first created
handlers.RoomCreated = function (args) {
    log.debug("Room Created - Game: " + args.GameId + " MaxPlayers: " + args.CreateOptions.MaxPlayers);
}

// Triggered automatically when a player joins a Photon room
handlers.RoomJoined = function (args) {
    log.debug("Room Joined - Game: " + args.GameId + " PlayFabId: " + args.UserId);
}

// Triggered automatically when a player leaves a Photon room
handlers.RoomLeft = function (args) {
    log.debug("Room Left - Game: " + args.GameId + " PlayFabId: " + args.UserId);
}

// Triggered automatically when a Photon room closes
handlers.RoomClosed = function (args) {
    log.debug("Room Closed - Game: " + args.GameId);
}

// Triggered by calling OpRaiseEvent on the Photon client. The args.Data is 
// set to the value of the customEventContent HashTable parameter, so you can use
// it to pass in arbitrary data.
handlers.RoomEventRaised = function (args) {
    var eventData = args.Data;
    log.debug("Event Raised - Game: " + args.GameId + " Event Type: " + eventData.eventType);

    switch (eventData.eventType) {
        case "playerMove":
            processPlayerMove(eventData);
            break;

        default:
            break;
    }
}
