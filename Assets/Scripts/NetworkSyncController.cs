using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkSyncController : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkedPosition;
    private Quaternion networkedRotation;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            // Set up the local player
        }
        else
        {
            // Set up the remote player
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, networkedPosition, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkedRotation, Time.deltaTime * 5);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
