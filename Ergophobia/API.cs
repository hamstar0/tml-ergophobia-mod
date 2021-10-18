using System;
using Terraria;
using Ergophobia.Items.HouseFurnishingKit;
using Ergophobia.Logic;


namespace Ergophobia {
	public class ErgophobiaAPI {
		public static void OnPreHouseFurnish( HouseFurnishingKitItem.PreFurnishHouse func ) {
			ErgophobiaMod.Instance.OnPreHouseFurnish.Add( func );
		}
		

		public static void OnPostHouseFurnish( HouseFurnishingKitItem.OnFurnishHouse action ) {
			ErgophobiaMod.Instance.OnPostHouseFurnish.Add( action );
		}


		////////////////
		
		public static bool CanPlaceRope( int tileX, int tileY ) {
			return TileLogic.CanPlaceRope( tileX, tileY );
		}
	}
}