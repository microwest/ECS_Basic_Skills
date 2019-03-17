using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using _01Spawn;



namespace _02Spawn_Destroy
{
    /// <summary>
    /// 启动程序类
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        public int maxCount = 10000;
        public float maxSpeed = 5;
        public float Range = 100;

        public GameObject Prefab;

        public Button spawnBtn;
        public Button destroyBtn;
        public Text info;

        EntityManager entityManager;
        int ballCount = 0;


        private void Start()
        {

            entityManager = World.Active.GetOrCreateManager<EntityManager>();
            m_EntityCommandBufferSystem = World.Active.GetOrCreateManager<EndSimulationEntityCommandBufferSystem>();

            spawnBtn.onClick.AddListener(() => { spawnEntities(10000); });
            destroyBtn.onClick.AddListener(() => { destroyEntity(10000); });
        }


        /// <summary>
        /// Spawn a certain number of Entities；产生count个实体
        /// </summary>
        /// <param name="count">count to spawn</param>
        void spawnEntities(int count)
        {
            Entity entity;
            Vector2 circle;
            Entity enPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active);
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




        EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;
        EntityCommandBuffer CommandBuffer;
        /// <summary>
        /// Destroy a certain number of Entities;删除count个实体
        /// </summary>
        /// <param name="count"></param>
        void destroyEntity(int count)
        {
            if (ballCount < 1)
                return;
            CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer();
            NativeArray<Entity> entities = entityManager.GetAllEntities();

            foreach (Entity en in entities)
            {
                if (ballCount > 0 && count > 0)
                {
                    CommandBuffer.DestroyEntity(en);
                    ballCount--;
                    count--;
                    info.text = "Entities:" + ballCount.ToString();
                }
            }
        }
    }
}