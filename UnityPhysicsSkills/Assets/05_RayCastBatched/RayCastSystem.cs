using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;


namespace Physics_05RayCastBatched
{
    public class RayCastSystem : ComponentSystem
    {
        protected override void OnCreateManager()
        {
            this.Enabled = false;
        }

        protected override void OnUpdate()
        {

        }
    }
}

