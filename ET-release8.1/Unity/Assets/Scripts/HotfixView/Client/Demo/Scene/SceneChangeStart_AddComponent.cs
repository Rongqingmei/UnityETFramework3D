using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class SceneChangeStart_AddComponent: AEvent<Scene, SceneChangeStart>
    {
        protected override async ETTask Run(Scene root, SceneChangeStart args)
        {
            try
            {
                Scene currentScene = root.CurrentScene();

                ResourcesLoaderComponent resourcesLoaderComponent = currentScene.GetComponent<ResourcesLoaderComponent>();
            
                // 加载场景资源
                await resourcesLoaderComponent.LoadSceneAsync($"Assets/Bundles/Scenes/{currentScene.Name}.unity", LoadSceneMode.Single);
                // 切换到map场景

                //await SceneManager.LoadSceneAsync(currentScene.Name);

                currentScene.AddComponent<OperaComponent>();
                
                // Debug.Log("加载Box2D ----------------1");
                // // 加载 Box2D
                // currentScene.AddComponent<Box3DControllerComponent>();
                //
                // Debug.Log("加载Box2D ----------------2");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }
    }
}