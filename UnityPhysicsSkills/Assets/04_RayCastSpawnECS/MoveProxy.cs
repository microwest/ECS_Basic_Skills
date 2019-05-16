using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace Physics_04RayCastSpawnECS
{
    [Serializable]
    public struct Move : IComponentData
    {
        public float acceleratioin;
        public float maxSpeed;
        public float speed;
        public float3 targetPos;
        public int isHit;
    }

    [RequiresEntityConversion]
    public class MoveProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var rayCastMove = new Move { targetPos = transform.position, isHit = 0 };
            dstManager.AddComponentData(entity, rayCastMove);
        }
    }
}

