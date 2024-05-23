using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    [SerializeField] private float activeTime = 0.1f;
    [SerializeField] private float alphaSet = 0.8f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float timeActivated;
    private float alpha;
    private float alphaMultiplier = 0.85f;

    private Color color;

    public void OnInit(Vector3 point, Quaternion rot, Sprite sprite)
    {
        alpha = alphaSet;

        spriteRenderer.sprite = sprite;
        transform.SetPositionAndRotation(point, rot);
        transform.localScale = new Vector3(0.5f, 0.5f, 1);

        timeActivated = Time.time;
    }


    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        spriteRenderer.color = color;
        
        if (Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
