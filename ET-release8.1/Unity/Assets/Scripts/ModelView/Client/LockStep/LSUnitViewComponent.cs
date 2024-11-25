namespace ET
{
	[ComponentOf(typeof(Room))]
	public class LSUnitViewComponent: Entity, IAwake
	{
		public EntityRef<LSUnitView> myUnitView;
	}
}