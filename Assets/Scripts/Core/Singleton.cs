using System;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleExpress.Core
{
    /// <summary>
    /// 다른 클래스에서 정적 인스턴스 참조의 형태로 사용되는 객체들의 공통 클래스.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// 해당 클래스를 상속받는 자식 클래스의 정적 인스턴스.
        /// </summary>
        private static T instance = null;

        /// <summary>
        /// Thread-Safe를 위한 
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 프로그램의 종료에 대한 여부.
        /// 프로그램 종료 뒤에도 인스턴스를 참조하여 불필요한 객체가 생성되는 것을 막기 위함임.
        /// </summary>
        private static bool isApplicationQuit = false;

        /// <summary>
        /// 해당 클래스를 상속받는 자식 클래스의 정적 인스턴스 Getter.
        /// </summary>
        public static T Instance
        {
            get
            {
                // 프로그램이 종료되지 않았다면 인스턴스를 반환한다.
                if (isApplicationQuit == false)
                {
                    lock (locker)
                    {
                        instance = instance ?? FindObjectOfType(typeof(T)) as T;

                        // 인스턴스가 할당되지 않았다면, 바로 할당한다.
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
