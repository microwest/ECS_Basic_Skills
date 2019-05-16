using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Random = UnityEngine.Random;


namespace Physics_06_RayCast_LR
{
    public class SpawnSystem : ComponentSystem
    {
        public int maxCount;
        public float Range;
        public GameObject playerPrefab;
        public bool isSpawn = false;

        public int CountToSpawn = 0;
        int carCount = 0;

        protected override void OnCreateManager()
        {
            Enabled = false;
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            if (Spawn.Instance == null)
            {//make sure this system running in demo 04
                this.Enabled = false;
                return;
            }
            maxCount = Spawn.Instance.maxCount;
            Range = Spawn.Instance.Range;
            playerPrefab = Spawn.Instance.playerPrefab;
        }

        protected override void OnUpdate()
        {
            if (CountToSpawn > 0)
            {
                SpawnEntities(CountToSpawn);
                CountToSpawn = 0;
                Spawn.Instance.CarNum = carCount;
            }
        }
        void SpawnEntities(int count)
        {
            isSpawn = true;
            Entity entity;
            Vector2 circle;


            Entity EntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, World.Active);
            #region Record the count of spawned Entities 记录现有实体数量
            if (carCount >= maxCount)
            {
                PostUpdateCommands.DestroyEntity(EntityPrefab);
                return;
            }
            else if (carCount + count > maxCount)
            {
                count = maxCount - carCount;
                carCount = maxCount;
            }
            else
                carCount += count;
            #endregion
            for (int i = 0; i < count; i++)
            {
                entity = PostUpdateCommands.Instantiate(EntityPrefab);

                circle = Random.insideUnitCircle * Range;

                Translation pos = new Translation()
                {
                    Value = new float3(circle.x, 1, circle.y)
                };
                Move move = new Move()
                {
                    acceleratioin = 5,
                    maxSpeed = 5,
                    targetPos = pos.Value,
                    isHit = 0,
                };

                PostUpdateCommands.SetComponent(entity, pos);
                PostUpdateCommands.SetComponent(entity, move);
            }

            PostUpdateCommands.DestroyEntity(EntityPrefab);
        }
    }
}

