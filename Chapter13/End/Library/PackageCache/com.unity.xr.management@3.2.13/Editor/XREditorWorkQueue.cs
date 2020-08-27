using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;


namespace UnityEditor.XR.Management
{
    internal class EditorWorkQueue<T>
    {
        [Serializable]
        struct Queue
        {
            [SerializeField]
            public List<T> workItems;
        }


        public string QueueName { get; set; }

        private static Lazy<EditorWorkQueue<T>> s_Instance = new Lazy<EditorWorkQueue<T>>();
        public static EditorWorkQueue<T> Instance => s_Instance.Value;

        public bool HasWorkItems => EditorPrefs.HasKey(QueueName);

        public Action<T> ProcessItemCallback { get; set; }

        public void StartQueue()
        {
            EditorApplication.update += ProcessWorkQueue;
        }

        public void QueueWorkItem(T workItem)
        {
            Queue queue = new Queue();
            queue.workItems = new List<T>();

            if (EditorPrefs.HasKey(QueueName))
            {
                string fromJson = EditorPrefs.GetString(QueueName);
                JsonUtility.FromJsonOverwrite(fromJson, queue);
            }

            if (queue.workItems == null)
            {
                queue.workItems = new List<T>();
            }

            queue.workItems.Add(workItem);
            string json = JsonUtility.ToJson(queue);
            EditorPrefs.SetString(QueueName, json);
            StartQueue();
        }

        private static void ProcessWorkQueue()
        {
            EditorApplication.update -= ProcessWorkQueue;
            if (!Instance.HasWorkItems)
                return;

            T workItem = GetNextWorkItem();

            if (Instance.ProcessItemCallback != null)
                Instance.ProcessItemCallback(workItem);

            if (Instance.HasWorkItems)
                EditorApplication.update += ProcessWorkQueue;

        }

        private static T GetNextWorkItem()
        {
            T ret = default(T);

            if (!Instance.HasWorkItems)
            {
                return ret;
            }

            string fromJson = EditorPrefs.GetString(Instance.QueueName);
            EditorPrefs.DeleteKey(Instance.QueueName);

            Queue queue = JsonUtility.FromJson<Queue>(fromJson);
            if (queue.workItems.Count <= 0)
            {
                return ret;
            }

            ret = queue.workItems[0];
            queue.workItems.Remove(ret);

            if (queue.workItems.Count > 0)
            {
                string json = JsonUtility.ToJson(queue);
                EditorPrefs.SetString(Instance.QueueName, json);
            }

            return ret;
        }

    }
}

