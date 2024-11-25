using FixMath.NET;
using Fix64 = TrueSync.FP;
using TrueSync;

namespace BEPUutilities
{
#pragma warning disable F64_NUM, CS1591
	public static class F64
	{
		public static readonly TrueSync.FP C0 = (TrueSync.FP)0;
		public static readonly TrueSync.FP C1 = (TrueSync.FP)1;
		public static readonly TrueSync.FP C180 = (TrueSync.FP)180;
		public static readonly TrueSync.FP C2 = (TrueSync.FP)2;
		public static readonly TrueSync.FP C3 = (TrueSync.FP)3;
		public static readonly TrueSync.FP C5 = (TrueSync.FP)5;
		public static readonly TrueSync.FP C6 = (TrueSync.FP)6;
		public static readonly TrueSync.FP C16 = (TrueSync.FP)16;
		public static readonly TrueSync.FP C24 = (TrueSync.FP)24;
		public static readonly TrueSync.FP C50 = (TrueSync.FP)50;
		public static readonly TrueSync.FP C60 = (TrueSync.FP)60;
		public static readonly TrueSync.FP C120 = (TrueSync.FP)120;
		public static readonly TrueSync.FP C90000 = (TrueSync.FP)90000;
		public static readonly TrueSync.FP C600000 = (TrueSync.FP)600000;
		
		public static readonly TrueSync.FP C0p001 = TrueSync.FP.EN2;
		public static readonly TrueSync.FP C0p5 = (TrueSync.FP)1/(TrueSync.FP)2;
		public static readonly TrueSync.FP C0p25 = (TrueSync.FP)1/(TrueSync.FP)4;
		public static readonly TrueSync.FP C1em09 = FP.EN8;
		public static readonly TrueSync.FP C1em9 = FP.EN8;
		public static readonly TrueSync.FP Cm1em9 = -FP.EN8;
		public static readonly TrueSync.FP C1em14 = FP.EN8;
		public static readonly TrueSync.FP C0p1 = FP.EN1;
		public static readonly TrueSync.FP OneThird = (TrueSync.FP)1/(TrueSync.FP)3;
		public static readonly TrueSync.FP C0p75 = (TrueSync.FP)3/(TrueSync.FP)4;//(TrueSync.FP)0.75m;
		public static readonly TrueSync.FP C0p15 = (TrueSync.FP)15/(TrueSync.FP)100;//(TrueSync.FP)0.15m;
		public static readonly TrueSync.FP C0p3 = (TrueSync.FP)3/(TrueSync.FP)10;//(TrueSync.FP)0.3m;
		public static readonly TrueSync.FP C0p0625 = (TrueSync.FP)625/(TrueSync.FP)10000; //(TrueSync.FP)0.0625m;
		public static readonly TrueSync.FP C0p99 = (TrueSync.FP)99/(TrueSync.FP)100;//(TrueSync.FP).99m;
		public static readonly TrueSync.FP C0p9 = (TrueSync.FP)9/(TrueSync.FP)10;//(TrueSync.FP).9m;
		public static readonly TrueSync.FP C1p5 = (TrueSync.FP)15/(TrueSync.FP)10; //(TrueSync.FP)1.5m;
		public static readonly TrueSync.FP C1p1 = (TrueSync.FP)11/(TrueSync.FP)10;//(TrueSync.FP)1.1m;
		public static readonly TrueSync.FP OneEighth = (TrueSync.FP)1 / (FP)8;
		public static readonly TrueSync.FP FourThirds = (TrueSync.FP)4/(TrueSync.FP)3;
		public static readonly TrueSync.FP TwoFifths = (TrueSync.FP)2/(TrueSync.FP)5;
		public static readonly TrueSync.FP C0p2 = (TrueSync.FP)2/(TrueSync.FP)10;
		public static readonly TrueSync.FP C0p8 = (TrueSync.FP)8/(TrueSync.FP)10;
		public static readonly TrueSync.FP C0p01 = (TrueSync.FP)1/(TrueSync.FP)100;
		public static readonly TrueSync.FP C1em7 = FP.EN7;
		public static readonly TrueSync.FP C1em5 = FP.EN5;
		public static readonly TrueSync.FP C1em4 = FP.EN4;
		public static readonly TrueSync.FP C1em10 = FP.EN8;
		public static readonly TrueSync.FP Cm0p25 = -(TrueSync.FP)1/(TrueSync.FP)4;
		public static readonly TrueSync.FP Cm0p9999 = (TrueSync.FP)9999/(TrueSync.FP)10000;//(TrueSync.FP)(-0.9999m);
		public static readonly TrueSync.FP C1m1em12 = (TrueSync.FP)1 - FP.EN8;
		public static readonly TrueSync.FP GoldenRatio = (TrueSync.FP)1 + TrueSync.FP.Sqrt((TrueSync.FP)5) / (TrueSync.FP)2;
		public static readonly TrueSync.FP OneTwelfth = (TrueSync.FP)1 / (TrueSync.FP)12;
		public static readonly TrueSync.FP C0p0833333333 = (TrueSync.FP)1/(TrueSync.FP)12;
		
		// public static readonly Fix64 C0 = (Fix64)0;
		// public static readonly Fix64 C1 = (Fix64)1;
		// public static readonly Fix64 C180 = (Fix64)180;
		// public static readonly Fix64 C2 = (Fix64)2;
		// public static readonly Fix64 C3 = (Fix64)3;
		// public static readonly Fix64 C5 = (Fix64)5;
		// public static readonly Fix64 C6 = (Fix64)6;
		// public static readonly Fix64 C16 = (Fix64)16;
		// public static readonly Fix64 C24 = (Fix64)24;
		// public static readonly Fix64 C50 = (Fix64)50;
		// public static readonly Fix64 C60 = (Fix64)60;
		// public static readonly Fix64 C120 = (Fix64)120;
		// public static readonly Fix64 C0p001 = (Fix64)0.001m;
		// public static readonly Fix64 C0p5 = (Fix64)0.5m;
		// public static readonly Fix64 C0p25 = (Fix64)0.25m;
		// public static readonly Fix64 C1em09 = (Fix64)1e-9m;
		// public static readonly Fix64 C1em9 = (Fix64)1e-9m;
		// public static readonly Fix64 Cm1em9 = (Fix64)(-1e-9m);
		// public static readonly Fix64 C1em14 = (Fix64)(1e-14m);		
		// public static readonly Fix64 C0p1 = (Fix64)0.1m;
		// public static readonly Fix64 OneThird = (Fix64)1/(Fix64)3;
		// public static readonly Fix64 C0p75 = (Fix64)0.75m;
		// public static readonly Fix64 C0p15 = (Fix64)0.15m;
		// public static readonly Fix64 C0p3 = (Fix64)0.3m;
		// public static readonly Fix64 C0p0625 = (Fix64)0.0625m;
		// public static readonly Fix64 C0p99 = (Fix64).99m;
		// public static readonly Fix64 C0p9 = (Fix64).9m;
		// public static readonly Fix64 C1p5 = (Fix64)1.5m;
		// public static readonly Fix64 C1p1 = (Fix64)1.1m;
		// public static readonly Fix64 OneEighth = Fix64.One / 8;
		// public static readonly Fix64 FourThirds = new Fix64(4) / 3;
		// public static readonly Fix64 TwoFifths = new Fix64(2) / 5;
		// public static readonly Fix64 C0p2 = (Fix64)0.2m;
		// public static readonly Fix64 C0p8 = (Fix64)0.8m;
		// public static readonly Fix64 C0p01 = (Fix64)0.01m;
		// public static readonly Fix64 C1em7 = (Fix64)1e-7m;
		// public static readonly Fix64 C1em5 = (Fix64)1e-5m;
		// public static readonly Fix64 C1em4 = (Fix64)1e-4m;
		// public static readonly Fix64 C1em10 = (Fix64)1e-10m;
		// public static readonly Fix64 Cm0p25 = (Fix64)(-0.25m);
		// public static readonly Fix64 Cm0p9999 = (Fix64)(-0.9999m);
		// public static readonly Fix64 C1m1em12 = Fix64.One - (Fix64)1e-12m;
		// public static readonly Fix64 GoldenRatio = Fix64.One + Fix64.Sqrt((Fix64)5) / (Fix64)2;
		// public static readonly Fix64 OneTwelfth = Fix64.One / (Fix64)12;
		// public static readonly Fix64 C0p0833333333 = (Fix64).0833333333m;
		// public static readonly Fix64 C90000 = (Fix64)90000;
		// public static readonly Fix64 C600000 = (Fix64)600000;
	}
}
