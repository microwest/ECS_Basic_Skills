using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using ECS_01Spawn;



namespace ECS_02Destroy_AllEntities
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

        EntityManager EM;
        int ballCount = 0;
        BallMoveSystem moveSystem;

        private void Start()
        {
            EM = World.Active.EntityManager;
            moveSystem = World.Active.GetOrCreateSystem<BallMoveSystem>();

            spawnBtn.onClick.AddListener(() => { spawnEntities(1000); });
            destroyBtn.onClick.AddListener(() => { destroyEntity(1000); });
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
            Entity enPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active);
            #region Record the count of spawned Entities 记录现有实体数量
            if (ballCount >= maxCount)
            {
                EM.DestroyEntity(enPrefab);
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
                entity = EM.Instantiate(enPrefab);

                circle = UnityEngine.Random.insideUnitCircle * Range;

                Translation pos = new Translation()
                {
                    Value = new float3(circle.x, 0, circle.y)
                };

                BallMoveSpeed speed = new BallMoveSpeed()
                {
                    Value = UnityEngine.Random.Range(1, maxSpeed)
                };
                EM.SetComponentData(entity, pos);
                EM.SetComponentData(entity, speed);
            }

            EM.DestroyEntity(enPrefab);

        }


                
        /// <summary>
        /// Destroy a certain number of Entities;删除count个实体
        /// </summary>
        /// <param name="count"></param>
        void destroyEntity(int count)
        {
            if (ballCount < 1)
                return;

            //***************************************Destroy Entities Without Matching
            #region Destroy Entities Without Matching.
            NativeArray<Entity> entities = EM.GetAllEntities();
            foreach (Entity en in entities)
            {
                if (ballCount > 0 && count > 0)
                {
                    EM.DestroyEntity(en);
                    ballCount--;
                    count--;
                    info.text = "Entities:" + ballCount.ToString();
                }
            }
            #endregion
        }
    }
}