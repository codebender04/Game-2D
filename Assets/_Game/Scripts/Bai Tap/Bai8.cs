using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bai8 : MonoBehaviour
{
    [SerializeField] private Transform[] pointsArray;
    [SerializeField] private float speed = 4f;
    private int currentIndex = 0;
    private void Update()
    {
       
    }
    private void Start()
    {
        StartCoroutine(MoveAndWait());
    }
    private IEnumerator MoveAndWait()
    {
        while (Vector2.Distance(transform.position, pointsArray[currentIndex].position) < 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointsArray[currentIndex].position, speed * Time.deltaTime);
            yield return null;
        }

        if (currentIndex >= pointsArray.Length - 1)
        {
            Debug.Log("Return");
            currentIndex = 0;
            yield return new WaitForSeconds(1f);
        }
        else 
        { 
            currentIndex++;
            yield return new WaitForSeconds(1f); 
        }
    }
}
