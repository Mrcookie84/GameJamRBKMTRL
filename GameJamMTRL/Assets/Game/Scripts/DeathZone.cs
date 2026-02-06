using System;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Dead");
        levelManager.RestartLevel();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
