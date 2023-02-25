using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleManiacs.Core
{
    /// <summary>
    /// �ٸ� Ŭ�������� ���� �ν��Ͻ� ������ ���·� ���Ǵ� ��ü���� ���� Ŭ����.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// �ش� Ŭ������ ��ӹ޴� �ڽ� Ŭ������ ���� �ν��Ͻ�.
        /// </summary>
        private static T instance = null;

        /// <summary>
        /// Thread-Safe�� ���� 
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// ���α׷��� ���ῡ ���� ����.
        /// ���α׷� ���� �ڿ��� �ν��Ͻ��� �����Ͽ� ���ʿ��� ��ü�� �����Ǵ� ���� ���� ������.
        /// </summary>
        private static bool isApplicationQuit = false;

        /// <summary>
        /// �ش� Ŭ������ ��ӹ޴� �ڽ� Ŭ������ ���� �ν��Ͻ� Getter.
        /// </summary>
        public static T Instance
        {
            get
            {
                // ���α׷��� ������� �ʾҴٸ� �ν��Ͻ��� ��ȯ�Ѵ�.
                if (isApplicationQuit == false)
                {
                    lock (locker)
                    {
                        instance = instance ?? FindObjectOfType(typeof(T)) as T;

                        // �ν��Ͻ��� �Ҵ���� �ʾҴٸ�, �ٷ� �Ҵ��Ѵ�.
                        if (instance == null)
                        {
                            var obj = GameObject.Find(typeof(T).ToString());

                            if (obj == null)
                                obj = new GameObject(obj.name);

                            instance = obj.AddComponent(typeof(T)) as T;
                        }
                    }

                    return instance;
                }

                return null;
            }
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
        }

        protected virtual void OnApplicationQuit()
        {
            isApplicationQuit = true;
        }

        protected virtual void OnDestroy()
        {
            isApplicationQuit = true;
        }
    }
}