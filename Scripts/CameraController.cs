using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject targetPlayer;

    // Update is called once per frame
    void Update()
    {
        //teleport and rotate same as player
        transform.position = targetPlayer.transform.position;
        transform.rotation = targetPlayer.transform.rotation;
    }
}
