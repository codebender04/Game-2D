using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * Vector3.up * Time.deltaTime;
        } 
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += speed * Vector3.down * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position += speed * Vector3.left * Time.deltaTime;
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * Vector3.right * Time.deltaTime;
        }
    }
}
