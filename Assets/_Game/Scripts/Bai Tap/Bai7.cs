using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToRandomPoints : MonoBehaviour
{
    [SerializeField] private Transform[] pointsArray;
    [SerializeField] private float speed = 4f;
    private int currentIndex = 0;
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, pointsArray[currentIndex].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, pointsArray[currentIndex].position) < 0.1f)
        {
            currentIndex = Random.Range(0, pointsArray.Length);
        }
    }
}
