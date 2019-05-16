using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;



namespace Physics_06_RayCast_LR
{
    public class Spawn : MonoBehaviour
    {
        public static Spawn Instance;

        public int maxCount = 10000;
        public int spawnCount = 100;
        [HideInInspector]
        public int CountToSpawn = 0;
        public float Range = 100;
        public GameObject playerPrefab;

        public Button spawnBtn;
        public Text info;

        void Start()
        {
            Instance = this;
            World.Active.GetExistingSystem<SpawnSystem>().Enabled = true;
            spawnBtn.onClick.AddListener(() => { World.Active.GetExistingSystem<SpawnSystem>().CountToSpawn = spawnCount; });

            spawnBtn.GetComponentInChildren<Text>().text = "Spawn " + spawnCount;
        }

        public int CarNum
        {
            set
            {
                info.text = "Total:" + value.ToString();
            }
        }
    }
}

