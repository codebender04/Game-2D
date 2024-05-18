using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bai8 : MonoBehaviour
{
    [SerializeField] private Transform[] pointsArray;
    [SerializeField] private float speed = 4f;
    private int currentIndex = 0;
    private float timer;
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, pointsArray[currentIndex].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, pointsArray[currentIndex].position) < 0.1f)
        {
            if (currentIndex >= pointsArray.Length - 1)
            {
                currentIndex = 0;
            }
            else currentIndex++;
        }
    }

}
