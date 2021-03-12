using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;


namespace Ergophobia.Logic {
	static partial class TileLogic {
		public static bool IsSuitableForPlatform( int tileX, int tileY, int dirX, out bool isStair ) {
			var config = ErgophobiaConfig.Instance;
			int max = config.Get<int>( nameof(config.MaxPlatformBridgeLength) );
			if( max < 0 ) {
				isStair = false;
				return true;
			}

			// Find anchor:
			for( int i = 1; i <= max; i++ ) {
				Tile tile = Framing.GetTileSafely( tileX + (i * dirX), tileY );
				if( !tile.active() || !Main.tileSolid[tile.type] ) {
					tile = Framing.GetTileSafely( tileX + (i * dirX), tileY - 1 );	// above
					if( !tile.active() || !Main.tileSolid[tile.type] ) {
						isStair = false;
						return false;
					}

					isStair = true;
				} else {
					isStair = false;
				}

				// Anchor found if:
				if( !Main.tileSolidTop[tile.type] && tile.type != TileID.MagicalIceBlock ) {
					return true;
				}
			}

			isStair = false;
			return false;
		}



		////////////////

		public static bool CanPlacePlatform( int i, int j, out bool isStair ) {
			if( TileLogic.IsSuitableForPlatform(i, j, -1, out isStair) ) {
				return true;
			}
			if( TileLogic.IsSuitableForPlatform(i, j, 1, out isStair) ) {
				return true;
			}
			isStair = false;
			return false;
		}
	}
}
