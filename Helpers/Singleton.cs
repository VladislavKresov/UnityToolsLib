using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalTools {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static readonly object threadLock = new object();
        private static T _instance;
        public static T Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }
                lock (threadLock) {
                    var instances = FindObjectsOfType<T>();
                    if (instances.Length > 0) {
                        _instance = instances[0];
                        for (int i = 1; i < instances.Length; i++) {
                            Destroy(instances[i]);
                        }
                    }
                    else {
                        _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    }
                    DontDestroyOnLoad(_instance);
                    return _instance;
                }
            }
        }
    }
}
