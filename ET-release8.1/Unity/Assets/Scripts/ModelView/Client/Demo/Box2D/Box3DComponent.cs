using System;
// using Box2DSharp.Dynamics;
using ET.Client;

namespace ET
{
    [ChildOf(typeof(Box3DControllerComponent))]
    public class Box3DComponent: Entity, IAwake<InitBox3DBody>
    {
        public BEPUphysics.Entities.Entity body;
        public EntityRef<LSUnit> Unit;
    }
}
