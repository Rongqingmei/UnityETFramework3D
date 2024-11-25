using TrueSync;
using Unity.Mathematics;

namespace ET
{
    public struct ChangePosition
    {
        public Unit Unit;
        public float3 OldPos;
    }

    public struct ChangeRotation
    {
        public Unit Unit;
    }
    
    // 更新帧
    public struct UpdateBox2D
    {
        public long[] LSUnitIds;
    }
    
    // 创建新的BOX2D的碰撞物体
    public struct InitBox3DBody
    {
        public long LSUnitId;
        public TrueSync.TSVector Position;
        public TSQuaternion Rotation;
    }
    
    // 输入信息，更新BOX2D物体的位置
    public struct InputBox2DBody
    {
        public long LSUnitId;
        public LSInput lsInput;
    }
    
    // 输入信息，更新BOX2D物体的位置
    public struct RollBackBox2D
    {
        public long[] LSUnitIds;
    }
}