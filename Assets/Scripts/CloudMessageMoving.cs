using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMessageMoving : MonoBehaviour
{
    private Vector2 spawnPoint = new Vector2(-14f,3f);
    private Vector2 target = new Vector2(14f,3f);
    [Range(0.0f, 1f)] 
    [SerializeField] private float cloudTime = 0;
    [Range(0.1f, 10f)] 
    [SerializeField] private float speedMove;
    // частота волны
    [SerializeField] private float freq = 30f; 
    // размер волны
    [SerializeField] private float waveScale = 1f;

    public float CloudTime
    {
        get
        {
            return cloudTime;
        }
    }
    private void Update()
    {
        cloudTime += (Time.deltaTime / 10) * speedMove;
        if (spawnPoint == null || target == null) return;
        //Реализации перемещения объекта по "волне".
        Vector2 res = WaveLerp(spawnPoint, target, cloudTime, waveScale, freq);
        transform.position = res;


    }
    // Метод для создания волны, подобной графику y = sin(x) - т.е синусойда
    private Vector2 WaveLerp(Vector2 a, Vector2 b, float time, float waveScale = 1f, float freq = 1f)
    {
        Vector2 result = Vector2.Lerp(a, b, time);
 
        Vector2 dir = (b - a).normalized;
        Vector2 leftNormal = result + new Vector2(-dir.y, dir.x) * waveScale;
 
        result = Vector2.LerpUnclamped(result, leftNormal, Mathf.Sin(time * freq));
 
        return result;
    }

    public void StartMessage()
    {
        cloudTime = 0;
    }
    
}
