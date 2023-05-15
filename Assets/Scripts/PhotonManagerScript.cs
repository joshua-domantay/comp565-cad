using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonManagerScript : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server, you may now create or join a room");
    }

    public void Connect2PUN()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Photon...");
        do {
            // this is here to essentially loop until connected for debugging log messaging
            // there is a difference between the PhotonNetwork beinng connected & being ready to create & join rooms
            // I wanted to verify I connected to see if I was having issues, after connecting to the photon network, reaching the master server to host or join rooms.
        } while(!PhotonNetwork.IsConnected);
        Debug.Log("Connected to Photon Cloud");
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("Creating room...");
            string roomName = Random.Range(100000000, 999999999).ToString();
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4 });
            Debug.Log($"Room #{roomName} created");
        }
        else
        {
            Debug.Log("Not connected to Master Server yet, waiting for callback");
        }
    }

    public void CreateRoom(TMP_Text roomIDText)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("Creating room...");
            string roomID = roomIDText.text;
            PhotonNetwork.CreateRoom(roomID, new RoomOptions { MaxPlayers = 4 });
            Debug.Log($"Room #{roomID} created");
        }
        else
        {
            Debug.Log("Not connected to Master Server yet, waiting for callback");
        }
    }

    public void JoinRoomByName(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinRoomByName(TMP_Text roomIDText)
    {
        PhotonNetwork.JoinRoom(roomIDText.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, playerPrefab.transform.rotation);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created");
        PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, playerPrefab.transform.rotation);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join random room");
    }

    public override void OnConnected()
    {
        Debug.Log("Connected to Photon");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected from Photon. Reason: {cause}");
    }

    public void CloseRoomAndDisconnect()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Leaving room...");
        }
        else
        {
            Debug.Log("Not in a room or not connected to Photon.");
        }

        PhotonNetwork.Disconnect();
        Debug.Log("Disconnecting from Photon...");
    }
}