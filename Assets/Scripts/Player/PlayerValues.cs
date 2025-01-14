using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
[CreateAssetMenu(fileName = "SO_PlayerValues", menuName = "ScriptableObjects/SO_PlayerValues", order = 2)]
public class PlayerValues : ScriptableObject
{
    public string des;
    public float maxValue;
    public float currentValue;
    public bool isNull;

    public void StartResetValues(){
        currentValue = maxValue;
        isNull = false;
    }

    public void RemoveValue(float value) {
        if(currentValue>0){currentValue -= value;}
        if(currentValue <=0){ isNull = true;}
    }

    public void AddValue(float value){
        currentValue += value;
        if (currentValue > 0){isNull = true;}
        if (currentValue >= maxValue){ currentValue = maxValue;}
    }
}
