using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUV : MonoBehaviour
{
    public float parralax = 3f;


    // Update is called once per frame
    void Update()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material mat = mr.material;
        mat.GetTextureOffset("_MainTex");
        Vector2 offset = mat.mainTextureOffset;


        offset.x = transform.position.x / transform.localScale.x / parralax;
        offset.y = transform.position.y / transform.localScale.y / parralax;

        mat.mainTextureOffset = offset;
    }
}
