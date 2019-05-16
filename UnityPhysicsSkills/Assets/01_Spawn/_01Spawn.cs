using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Collider = Unity.Physics.Collider;

namespace Physics_01Spawn
{
    public class _01Spawn : MonoBehaviour
    {
        public Button btn;
        public GameObject prefab;
        public int spawnCount = 1000;
        public float3 range = 50;
        float3 center;
        EntityManager entityManager;
        Entity sourceEntity;

        void Start()
        {
            sourceEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, World.Active);
            entityManager = World.Active.EntityManager;
            center = float3.zero;
            center.y += range.y;

            btn.onClick.AddListener(() => { SpawnSphere(); });
        }

        void SpawnSphere()
        {
            BlobAssetReference<Collider> sourceCollider = entityManager.GetComponentData<PhysicsCollider>(sourceEntity).Value;
            Unity.Mathematics.Random random = new Unity.Mathematics.Random();
            random.InitState(10);
            for (int i = 0; i < spawnCount; i++)
            {
                var instance = entityManager.Instantiate(sourceEntity);
                entityManager.SetComponentData(instance, new Translation { Value = center + random.NextFloat3(-range, range) });
                
            }
        }

    }
}

