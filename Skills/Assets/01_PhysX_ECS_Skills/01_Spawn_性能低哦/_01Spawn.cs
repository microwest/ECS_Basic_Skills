
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace PhysiX_01Spawn
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

        /// <summary>
        /// Spawn a certain number of Entities；产生count个实体
        /// </summary>
        void SpawnSphere()
        {
            Entity entity;
            Vector2 circle;
            Unity.Mathematics.Random random = new Unity.Mathematics.Random();
            random.InitState(10);

            for (int i = 0; i < spawnCount; i++)
            {
                entity = entityManager.Instantiate(sourceEntity);

                entityManager.SetComponentData(entity, new Translation { Value = center + random.NextFloat3(-range, range) });
            }
        }
    }
}