using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MTFrame.MTScene
{
    /// <summary>
    ///unity加载方式
    /// </summary>
    public class UnityLocal_SceneLoading
    {
        //加载对应回调
        private Action startLoading,  endLoading;
        private Action<float> updateLoading;
        //卸载对应回调
        private Action startUnload,  endUnload;
        private Action<float> updateUnload;

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="levelName">场景名字</param>
        /// <param name="loadSceneMode">加载类型</param>
        public void LoadScene(string levelName, LoadingType loadingType = LoadingType.Single)
        {
            switch (loadingType)
            {
                case LoadingType.Additive:
                    UnityEngine.SceneManagement.SceneManager.LoadScene(levelName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    break;
                case LoadingType.Single:
                    UnityEngine.SceneManagement.SceneManager.LoadScene(levelName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="levelName">场景名字</param>
        /// <param name="startLoading">开始回调</param>
        /// <param name="endLoading">结束回调</param>
        /// <param name="loadSceneMode">加载类型</param>
        public IEnumerator LoadSceneAsync(string levelName, Action _startLoading = null, Action<float> _updateLoading = null, Action _endLoading = null,
              LoadingType loadingType = LoadingType.Single)
        {
            UnityEngine.AsyncOperation asyncOperation = default(UnityEngine.AsyncOperation);
            switch (loadingType)
            {
                case LoadingType.Additive:
                    asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    break;
                case LoadingType.Single:
                    asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                    break;
                default:
                    break;
            }


            asyncOperation.completed += AsyncLoading_completed;
            startLoading = _startLoading;
            updateLoading = _updateLoading;
            endLoading = _endLoading;

            startLoading?.Invoke();

            while (!asyncOperation.isDone)
            {
                updateLoading?.Invoke(asyncOperation.progress);

                MTEvent.EventParamete eventParamete = new MTEvent.EventParamete();
                eventParamete.AddParameter(asyncOperation.progress);
                EventManager.TriggerEvent(MTEvent.GenericEventEnumType.Message, LoadingMessageType.LoadingProcess.ToString(), eventParamete);

                yield return new WaitForFixedUpdate();
            }

            yield return asyncOperation;
        }

        //异步场景加载完成
        private void AsyncLoading_completed(UnityEngine.AsyncOperation obj)
        {
            updateLoading?.Invoke(obj.progress);

            endLoading?.Invoke();

            MTEvent.EventParamete eventParamete = new MTEvent.EventParamete();
            eventParamete.AddParameter(obj.progress);
            EventManager.TriggerEvent(MTEvent.GenericEventEnumType.Message, LoadingMessageType.LoadingProcess.ToString(), eventParamete);

        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="levelName">要卸载场景的名字</param>
        /// <param name="endUnload">结束卸载</param>
        /// <param name="startUnload">开始卸载</param>
        public IEnumerator UnloadScene(string levelName, Action _startUnload = null, Action<float> _updateUnload = null, Action _endUnload = null)
        {
            UnityEngine.AsyncOperation asyncOperation = default(UnityEngine.AsyncOperation);
            try
            {
                 asyncOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(levelName);
                asyncOperation.completed += AsyncUnload_completed; ;

                startUnload = _startUnload;
                updateUnload = _updateUnload;
                endUnload = _endUnload;
                startUnload?.Invoke();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("错误：活动场景中不存在场景==" + levelName + "====" + ex.Data);
                GC.Collect();
                Resources.UnloadUnusedAssets();
            }
            if(asyncOperation!=null)
             while (!asyncOperation.isDone)
            {
                updateUnload?.Invoke(asyncOperation.progress);
                yield return new WaitForFixedUpdate();
            }

            yield return null;
        }
        //卸载完成
        private void AsyncUnload_completed(UnityEngine.AsyncOperation obj)
        {
            endUnload?.Invoke();
            GC.Collect();
            SourcesManager.UnloadUnusedAssets();
        }


    }
}