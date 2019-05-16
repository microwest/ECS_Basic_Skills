using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace Physics_03RayCastECS
{
    [Serializable]
    public struct Move : IComponentData
    {
        public float3 targetPos;
        public int isHit;
    }

    [RequiresEntityConversion]
    public class MoveProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float range;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var rayCastMove = new Move { targetPos = GetRandomTarget(), isHit = 0 };
            dstManager.AddComponentData(entity, rayCastMove);
        }
        public float3 GetRandomTarget()
        {
            Random rand = new Random(100);
            float3 tmp = new float3(rand.NextFloat(-10, 10), 1, rand.NextFloat(-10, 10));
            return tmp;
        }
    }
}

