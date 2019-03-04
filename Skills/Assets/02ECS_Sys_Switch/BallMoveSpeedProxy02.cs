using System;
using Unity.Entities;


[Serializable]
public struct BallMoveSpeed02:IComponentData
{
    public float Value;
}

[UnityEngine.DisallowMultipleComponent]
public class BallMoveSpeedProxy02 :ComponentDataProxy<BallMoveSpeed02> { }
