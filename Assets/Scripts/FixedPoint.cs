using System;
using UnityEngine;

public class FixedPoint {
    public float value;
    public float minValue;
    public float maxValue;
    public float precision = 0.125f;
    
    public FixedPoint(float _minValue, float _maxValue, float _precision) {
        minValue = _minValue;
        maxValue = _maxValue;
        precision = _precision;
    }
}