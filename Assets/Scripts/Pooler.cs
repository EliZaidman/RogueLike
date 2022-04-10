using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;    
    }

    #region Singleton
    public static Pooler Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("An instance of Pooler already exists");
            return;
        }
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDic;

    void Start()
    {
        poolDic = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDic.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDic.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag \"" + tag + "\" doesn't exists.");
            return null;
        }

        GameObject objToSpawn = poolDic[tag].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        //if (objToSpawn.GetComponent<Rigidbody>() != null)
        //{
        //    objToSpawn.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //    objToSpawn.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}

        //IPooledObject pooledObj = objToSpawn.GetComponent<IPooledObject>();
        //if (pooledObj != null)
        //{
        //    pooledObj.OnObjectSpawn();
        //}

        poolDic[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }
        
}
