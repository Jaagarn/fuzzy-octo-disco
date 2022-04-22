using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    private GameObject player;        
    private Vector3 offset;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}