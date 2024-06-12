using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    [Range(0.1f,0.5f)]
    public float WaitBetweenWobbles = 1.0f;

    [Range(1f,50f)]
    public float Intensity = 10.0f;

    Quaternion _targetAngle;
    void Start()
    {
        InvokeRepeating("ChangeTarget",  0, WaitBetweenWobbles); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetAngle, Time.deltaTime);
    }

    void ChangeTarget()
    {
        var intensity = Random.Range(1, Intensity);
        var curve = Mathf.Sin(Random.Range(0, Mathf.PI * 2));
        _targetAngle = Quaternion.Euler(Vector3.forward * curve * intensity);
    }
}
