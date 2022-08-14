using UnityEngine;
using System.Threading;

namespace Onur.Template
{
    // This object wont be destroyed accross the scenes. Lifetime is same as App's
    public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;
        private static bool _isDestroyed;

        public static T instance {
            get {
                if (_isDestroyed) {
                    return null;
                }
                if (_instance == null) {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (FindObjectsOfType(typeof(T)).Length > 1) {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                " - there should never be more then 1 singletion!" +
                                " Reopening the scene might fix it. Thread: " + Thread.CurrentThread.Name);
                        return _instance;
                    }
                }
                return _instance;
            }
        }



        virtual protected void Awake() {
            
            string name = typeof(T).Name;
            if (_instance == null) {
                _instance = GetComponent<T>();
                DontDestroyOnLoad(gameObject);
                Debug.Log($"[{name}::Awake] SingletonDontDestroy object initialized.");
            } else {
                if (gameObject.GetComponents<Component>().Length > 1) {
                    Debug.Log($"[{name}::Awake] '{name}' already created! GameObject has other components, so just destroying newly created component.");
                    DestroyImmediate(gameObject);
                } else {
                    Debug.Log($"[{name}::Awake] '{gameObject.name}' already created! Destroying newly created one");
                    DestroyImmediate(gameObject);
                }
            }
        }

        virtual protected void OnDestroy() {}

		private void OnApplicationQuit() {
            _isDestroyed = true;
		}
	}
}
