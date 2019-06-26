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
    [DisableAutoCreation]
    public class BallDestroySystem : ComponentSystem
    {
        public bool isDeleting = false;

        /// <summary>
        /// 条件性删除：删除Y坐标>4的小球
        /// </summary>
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref Translation pos, ref BallMoveSpeed speed) =>
            {
                if (isDeleting && pos.Value.y > 4)
                {
                    PostUpdateCommands.DestroyEntity(entity);
                }
            });
            isDeleting = false;
        }
    }
}