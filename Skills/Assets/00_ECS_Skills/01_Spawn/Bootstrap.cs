using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;


namespace ECS_01Spawn
{
    /// <summary>
    /// 启动程序类
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        public int maxCount = 10000;
        public float maxSpeed = 5;
        public float Range = 100;

        public GameObject prefab;

        public Button spawnBtn;
        public Text info;

        EntityManager entityManager;
        int ballCount = 0;
        BallMoveSystem moveSystem;

        private void Start()
        {
            entityManager = World.Active.EntityManager;
            moveSystem = World.Active.GetOrCreateSystem<BallMoveSystem>();

            spawnBtn.onClick.AddListener(() => { spawnEntities(1000); });
        }
        private void FixedUpdate()
        {
            moveSystem.Update();
        }
        

        /// <summary>
        /// Spawn a certain number of Entities；产生count个实体
        /// </summary>
        /// <param name="count">count to spawn</param>
        void spawnEntities(int count)
        {
            Entity entity;
            Vector2 circle;
            Entity enPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, World.Active);
            #region Record the count of spawned Entities 记录现有实体数量
            if (ballCount >= maxCount)
            {
                entityManager.DestroyEntity(enPrefab);
                return;
            }
            else if (ballCount + count > maxCount)
            {
                count = maxCount - ballCount;
                ballCount = maxCount;
            }
            else
                ballCount += count;

            info.text = "Entities:" + ballCount.ToString();
            #endregion

            for (int i = 0; i < count; i++)
            {
                entity = entityManager.Instantiate(enPrefab);

                circle = UnityEngine.Random.insideUnitCircle * Range;

                Translation pos = new Translation()
                {
                    Value = new float3(circle.x, 0, circle.y)
                };

                BallMoveSpeed speed = new BallMoveSpeed()
                {
                    Value = UnityEngine.Random.Range(1, maxSpeed)
                };
                entityManager.SetComponentData(entity, pos);
                entityManager.SetComponentData(entity, speed);
            }

            entityManager.DestroyEntity(enPrefab);

        }
    }
}