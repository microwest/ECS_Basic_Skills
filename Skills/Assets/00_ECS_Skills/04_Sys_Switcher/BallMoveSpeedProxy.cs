using System;
using Unity.Entities;
using UnityEngine;

namespace Sys_Switcher
{
    [Serializable]
    public struct BallMoveSpeed : IComponentData
    {
        public float Value;
    }

    [RequiresEntityConversion]
    public class BallMoveSpeedProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float moveSpeed = 0;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var speed = new BallMoveSpeed { Value = moveSpeed };
            dstManager.AddComponentData(entity, speed);
        }
    }
}