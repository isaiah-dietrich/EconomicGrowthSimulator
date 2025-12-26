using UnityEngine;
using System;

public class SimulationClock : MonoBehaviour
{
    public static event Action OnTick;

    [SerializeField] private float secondsPerTick = 1.0f;

    private float timer;

    private int tickCount = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= secondsPerTick)
        {
            timer = 0;
            tickCount++;

            //Broadcast the tick to anyone listening
            OnTick?.Invoke();

            Debug.Log($"--- Global Tick: {tickCount} ---");
        }
    }


}
