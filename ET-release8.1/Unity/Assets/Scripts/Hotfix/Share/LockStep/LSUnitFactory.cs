using TrueSync;

namespace ET
{
    public static partial class LSUnitFactory
    {
        public static LSUnit Init(LSWorld lsWorld, LockStepUnitInfo unitInfo)
        {
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(unitInfo.PlayerId);
			
	        lsUnit.Position = unitInfo.Position;
	        lsUnit.Rotation = unitInfo.Rotation;

			lsUnit.AddComponent<LSInputComponent>();
			
            return lsUnit;
        }
        
        // 初始化公共事务
        public static LSUnit InitCommonThing(LSWorld lsWorld, TSVector Position, TSQuaternion Rotation, long thingId)
        {
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        
	        // 添加类似于Transform的基础组件
	        LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(thingId);
			
	        lsUnit.Position = Position;
	        lsUnit.Rotation = Rotation;/**/

	        lsUnit.AddComponent<LSInputComponent>();
	        return lsUnit;
        }
    }
}
