using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bai9 : MonoBehaviour
{
    [SerializeField] private Transform[] pointsArray;
    [SerializeField] private float movementTime = 1f;
    private int currentIndex = 0;
    private float timer = 0f;
    private void Update()
    {
        timer =+ Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, pointsArray[currentIndex].position, timer / movementTime);
           
        if (Vector2.Distance(transform.position, pointsArray[currentIndex].position) < 0.1f)
        {
            timer = 0f;
            if (currentIndex >= pointsArray.Length - 1)
            {
                currentIndex = 0;
            }
            else currentIndex++;
        }
    }

}
