using Unity.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using ECS_01Spawn;



namespace ECS_03Destroy_MatchingEntities
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
        BallDestroySystem destroySystem;

        private void Start()
        {
            EM = World.Active.EntityManager;
            moveSystem = World.Active.GetOrCreateSystem<BallMoveSystem>();
            destroySystem = World.Active.GetOrCreateSystem<BallDestroySystem>();

            spawnBtn.onClick.AddListener(() => { spawnEntities(1000); });
            destroyBtn.onClick.AddListener(() => { destroySystem.isDeleting = true; });
        }
        private void FixedUpdate()
        {
            moveSystem.Update();
            if (destroySystem.isDeleting)
            {
                destroySystem.Update();
                BallRemained();
            }

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
        /// 计算剩下的小球数量
        /// </summary>
        void BallRemained()
        {
            EntityQuery EQ = EM.CreateEntityQuery(typeof(BallMoveSpeed));
            ballCount = EQ.CalculateLength();
            info.text = "Entities:" + ballCount.ToString();
            EQ.Dispose();
        }
    }



}