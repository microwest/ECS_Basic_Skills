using System;
using Unity.Entities;


[Serializable]
public struct BallMoveSpeed:IComponentData
{
    public float Value;
}

[UnityEngine.DisallowMultipleComponent]
public class BallMoveSpeedProxy :ComponentDataProxy<BallMoveSpeed> { }
