using Unity.Collections;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

/// <summary>
/// 启动程序类
/// </summary>
public class Bootstrap02 : MonoBehaviour
{
    public int maxCount = 10000;
    public float maxSpeed = 5;
    public float Range = 100;

    public GameObject entityPrefab;

    public Button toggleBtn;
    public Text info;

    EntityManager entityManager;
    int ballCount = 0;
    bool sysEnable = true;

    void Start()
    {
        entityManager = World.Active.GetOrCreateManager<EntityManager>();

        spawnEntities(5000);


        toggleBtn.onClick.AddListener(() => { toggleSystem(); });
    }


    /// <summary>
    /// Spawn a certain number of Entities；产生count个实体
    /// </summary>
    /// <param name="count">count to spawn</param>
    void spawnEntities(int count)
    {
        Entity entity;
        Vector2 circle;

        #region Record the count of spawned Entities 记录现有实体数量
        if (ballCount >= maxCount)
        {
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
            entity = entityManager.Instantiate(entityPrefab);

            circle = UnityEngine.Random.insideUnitCircle * Range;

            Position pos = new Position()
            {
                Value = new float3(circle.x, 0, circle.y)
            };

            BallMoveSpeed02 speed = new BallMoveSpeed02()
            {
                Value = UnityEngine.Random.Range(1, maxSpeed)
            };
            entityManager.SetComponentData(entity, pos);
            entityManager.SetComponentData(entity, speed);
        }
    }

    /// <summary>
    /// toggle running state of the system; 切换系统的开关
    /// </summary>
    void toggleSystem( )
    {
        sysEnable = !sysEnable;
        BallMoveSystem02.Instance.Enabled = sysEnable;

        if (sysEnable)
            info.text = "Running";
        else
            info.text = "Stop";
    }
}
