using UnityEngine;

namespace ET.Client
{
    // [LSEntitySystemOf(typeof(LSUnitViewComponent))]
    [EntitySystemOf(typeof(LSUnitViewComponent))]
    public static partial class LSUnitViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSUnitViewComponent self)
        {
            // 给每个LSUnit加上Box2D 再在LSROOM
            // Room room = self.Room();
            // LSUnitComponent lsUnitComponent = room.LSWorld.GetComponent<LSUnitComponent>();
            // Scene root = self.Root();
            // foreach (long playerId in room.PlayerIds)
            // {
            //     LSUnit lsUnit = lsUnitComponent.GetChild<LSUnit>(playerId);
            //     // 给 lsUnit 添加上 Box2D组件
            //     lsUnit.AddComponent<Box3DComponent>();
            // }
            

            
        }

        // [LSEntitySystem]
        // private static void LSUpdate(this LSUnitViewComponent self)
        // {
        //     
        // }

        // [EntitySystem]
        // private static void Destroy(this LSUnitViewComponent self)
        // {
        //
        // }

        public static async ETTask InitAsync(this LSUnitViewComponent self)
        {
            Room room = self.Room();
            LSUnitComponent lsUnitComponent = room.LSWorld.GetComponent<LSUnitComponent>();
            Scene root = self.Root();
            foreach (long playerId in room.PlayerIds)
            {
                LSUnit lsUnit = lsUnitComponent.GetChild<LSUnit>(playerId);
                string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
                GameObject bundleGameObject = await room.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
                GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");

                GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
                GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
                unitGo.transform.position = lsUnit.Position.ToVector();

                LSUnitView lsUnitView = self.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
                lsUnitView.AddComponent<LSAnimatorComponent>();
            }
            
            // 客户端创建一个地板对象 100001
            LSUnit groundUnit = lsUnitComponent.GetChild<LSUnit>(100001);
            // 加载 AB
            string gAssetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject gBundleGameObject = await room.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(gAssetsName);
            GameObject gPrefab = gBundleGameObject.Get<GameObject>("GroundBox");
            // 根据 AB 生成 prefab，在 GlobalComponent 底下。
            GlobalComponent gGlobalComponent = root.GetComponent<GlobalComponent>();
            GameObject gUnitGo = UnityEngine.Object.Instantiate(gPrefab, gGlobalComponent.Unit, true);
            gUnitGo.transform.position = groundUnit.Position.ToVector();
            gUnitGo.transform.localScale = new Vector3(100.0f, 0.1f, 100.0f);
            // 本地 LSUnitViewComponent 根节点下添加一个 LSUnitView 节点
            LSUnitView gLsUnitView = self.AddChildWithId<LSUnitView, GameObject>(groundUnit.Id, gUnitGo);

            // 客户端创建一个地板对象 100001
            LSUnit groundUnit2 = lsUnitComponent.GetChild<LSUnit>(100002);
            // 根据 AB 生成 prefab，在 GlobalComponent 底下。
            GameObject gUnitGo2 = UnityEngine.Object.Instantiate(gPrefab, gGlobalComponent.Unit, true);
            gUnitGo2.transform.position = groundUnit2.Position.ToVector();
            gUnitGo2.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // 本地 LSUnitViewComponent 根节点下添加一个 LSUnitView2 节点
            LSUnitView gLsUnitView2 = self.AddChildWithId<LSUnitView, GameObject>(groundUnit2.Id, gUnitGo2);
        }
    }
}