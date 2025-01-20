using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject m_FlowerPot;
    [SerializeField] private float m_spawnValueX = 10;
    [SerializeField] private float m_spawnValueY = 10;
    [SerializeField] private float m_TimeMin = 1;
    [SerializeField] private float m_TimeMax = 3;


    private void Start() {
        Invoke("SpawnFlowerPot", Random.Range(m_TimeMin, m_TimeMax));
    }


    public void SpawnFlowerPot()
    {
        float spawnPointX = Random.Range(-m_spawnValueX, m_spawnValueX);
        float spawnPointY = m_spawnValueY;
        
        Vector2 spawnPosition = new Vector2(spawnPointX, spawnPointY);
        Instantiate(m_FlowerPot, spawnPosition, Quaternion.identity);

        Invoke("SpawnFlowerPot", Random.Range(m_TimeMin, m_TimeMax));
    }
}
