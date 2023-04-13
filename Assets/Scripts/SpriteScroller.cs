using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroller : MonoBehaviour
{


    [SerializeField] float scrollSpeed;

    Vector2 offset;
    Material material;

    private void Awake() {
        material = GetComponent<SpriteRenderer>().material;
    }


    // Update is called once per frame
    void Update()
    {
        offset = new Vector2(0f, scrollSpeed * Time.deltaTime);
        material.mainTextureOffset = material.mainTextureOffset + offset;
    }
}
