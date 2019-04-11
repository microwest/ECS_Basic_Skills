using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Jobs;
using System;
using ECS_01Spawn;

namespace ECS_03Destroy_MatchingEntities
{
    [UpdateAfter(typeof(BallMoveSystem))]
    public class BallDestroySystem : ComponentSystem
    {
        public static BallDestroySystem Instance;
        public int deleteCount = 0;

        protected override void OnStartRunning()
        {
            if (Instance == null)
                Instance = this;
        }

        protected override void OnUpdate()
        {
            if (deleteCount < 1)
            {
                Enabled = false;
                return;
            }
            Entities.ForEach((Entity entity, ref Translation pos, ref BallMoveSpeed speed) =>
            {
                if (deleteCount > 0)
                {
                    PostUpdateCommands.DestroyEntity(entity);
                    deleteCount--;                    
                }
            });
        }
    }
}