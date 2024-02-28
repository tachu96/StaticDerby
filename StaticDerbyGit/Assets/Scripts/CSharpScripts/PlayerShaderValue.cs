using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShaderValue : MonoBehaviour
{
    [SerializeField]private float PlayerHeightOffset;

    private CapsuleCollider capsuleCollider;
    private float capsuleColliderRadius;
    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            capsuleColliderRadius = capsuleCollider.radius;
        }
        else
        {
            Debug.LogError("Capsule collider component not found on "+gameObject.name);
        }
    }

    void Update()
    {
        Shader.SetGlobalVector("_Player",transform.position+Vector3.up*capsuleColliderRadius);
    }
}
