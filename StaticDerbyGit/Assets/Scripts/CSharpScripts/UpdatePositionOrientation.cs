using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePositionOrientation : MonoBehaviour
{
    [SerializeField] private Transform player;
    void Update()
    {
        transform.position = player.position;
    }
}
