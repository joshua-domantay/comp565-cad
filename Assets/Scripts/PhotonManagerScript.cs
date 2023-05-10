using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PhotonManagerScript : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
    }

    public void CreateRandomRoom()
    {
        string roomName = Random.Range(100000000, 999999999).ToString();
        Debug.Log($"Room #{roomName} created");
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4 });
    }

    public void JoinRoomByName(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
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
        Debug.Log("Failed to join random room, creating a new room");
        CreateRandomRoom();
    }
}