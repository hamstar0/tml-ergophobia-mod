using System;
using Terraria;
using Ergophobia.Items.HouseFurnishingKit;


namespace Ergophobia {
	public class ErgophobiaAPI {
		public static void OnPreHouseFurnish( Func<int, int, bool> func ) {
			ErgophobiaMod.Instance.OnPreHouseFurnish.Add( func );
		}


		public static void OnPostHouseFurnish( HouseFurnishingKitItem.OnFurnishHouse action ) {
			ErgophobiaMod.Instance.OnPostHouseFurnish.Add( action );
		}
	}
}