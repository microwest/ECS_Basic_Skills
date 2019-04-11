using System;
using UnityEngine;
using Unity.Entities;

namespace ECS_01Spawn
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