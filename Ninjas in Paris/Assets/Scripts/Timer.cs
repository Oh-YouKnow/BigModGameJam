using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeLimit;
    private float _currentTime = 0f;
    public List<EnemySpawner> spawners;
    private EnemySpawner _activeSpawner;
    private int _activeSpawnerIndex = 0;
    private float _waveTime;
    public int waveTransition = 60;
    // Start is called before the first frame update
    void Start()
    {
        _activeSpawner = spawners[_activeSpawnerIndex];
        Transform spawner = _activeSpawner.transform;
        Instantiate(spawner);
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;
        _waveTime += Time.deltaTime;
        timerText.text = (timeLimit - _currentTime).ToString("00");
        if (_waveTime >= waveTransition)
        {
            // When the wave timer is over the wave transition, check the next wave list
            // index. If there is a valid one, make the active spawner into that.
            // If there is not one, set the wave spawner to the first in the list.
            _waveTime = 0f;
            if (_activeSpawnerIndex < spawners.Count - 1)
            {
                _activeSpawnerIndex++;
            }
            else
            {
                _activeSpawnerIndex = 0;
            }
            ChangeSpawner();
        }
    }

    private void ChangeSpawner()
    { 
        GameObject oldSpawner = GameObject.FindGameObjectWithTag("Spawner");
        Destroy(oldSpawner);
        _activeSpawner = spawners[_activeSpawnerIndex];
        Transform newSpawner = _activeSpawner.transform;
        Instantiate(newSpawner);
    }
}
