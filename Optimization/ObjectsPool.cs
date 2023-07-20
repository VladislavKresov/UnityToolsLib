using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace UniversalTools {
    public class ObjectsPool : Singleton<ObjectsPool> {
        private class Pool {
            private List<GameObject> _inactiveObjects = new List<GameObject>();
            private GameObject _objectPrefab;
            private Transform _objectsParent;
            public Pool(GameObject prefab) {
                _objectPrefab = prefab;
                _objectsParent = new GameObject(prefab.name).transform;
                _objectsParent.SetParent(Instance.transform);
            }

            public GameObject SpawnObject(Vector3 position, Quaternion rotation) {
                GameObject spawnObject;
                if(_inactiveObjects.Count > 0) {
                    spawnObject = _inactiveObjects.Last();
                    _inactiveObjects.Remove(spawnObject);
                    spawnObject.SetActive(true);
                }
                else {
                    spawnObject = Instantiate(_objectPrefab, position, rotation, _objectsParent);
                    spawnObject.name = _objectPrefab.name;
                }
                return spawnObject;
            }

            public void DespawnObject(GameObject obj) {
                obj.transform.SetParent(_objectsParent);
                obj.SetActive(false);
                _inactiveObjects.Add(obj);
            }
        }

        private static Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation) {
            if (!_pools.ContainsKey(prefab.name)) {
                _pools.Add(prefab.name, new Pool(prefab));
            }
            return _pools[prefab.name].SpawnObject(position, rotation);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) {
            var spawnedObject = Spawn(prefab, position, rotation);
            spawnedObject.transform.SetParent(parent, true);
            return spawnedObject;
        }

        public static GameObject Spawn(GameObject prefab, Transform parent = null) {
            var spawnedObject = Spawn(prefab, Vector3.zero, Quaternion.identity);
            spawnedObject.transform.SetParent(parent, false);
            return spawnedObject;
        }

        public static void Despawn(GameObject obj) {
            if(_pools.ContainsKey(obj.name)) {
                _pools[obj.name].DespawnObject(obj);
            }
            else {
                Destroy(obj);
            }
        }
    }
}
