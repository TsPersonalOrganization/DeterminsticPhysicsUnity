using UnityEngine;
using System.Collections.Generic;
using TrueSync;

/**
* @brief Manages boxes instantiation.
**/
public class GameSyncManager : TrueSyncBehaviour {
    
    /**
    * @brief Initial setup when game is started.
    **/
    public override void OnSyncedStart() {
        
    }

    /**
    * @brief Logs a text when game is paused.
    **/
    public override void OnGamePaused() {
        Debug.Log("Game Paused");
    }

    /**
    * @brief Logs a text when game is unpaused.
    **/
    public override void OnGameUnPaused() {
        Debug.Log("Game UnPaused");
    }

    /**
    * @brief Logs a text when game is ended.
    **/
    public override void OnGameEnded() {
        Debug.Log("Game Ended");
    }

    /**
    * @brief When a player get disconnected all objects belonging to him are destroyed.
    **/
    public override void OnPlayerDisconnection(int playerId) {
        TrueSyncManager.RemovePlayer(playerId);
    }

}