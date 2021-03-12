﻿using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;


namespace Ergophobia.Logic {
	static partial class TileLogic {
		public static bool IsSuitableForPlatform( int tileX, int tileY, int dirX ) {
			var config = ErgophobiaConfig.Instance;
			int max = config.Get<int>( nameof(config.MaxPlatformBridgeLength) );
			if( max < 0 ) {
				return true;
			}

			// Find anchor horizontally:
			for( int i = 1; i <= max; i++ ) {
				Tile tile = Framing.GetTileSafely( tileX + (i * dirX), tileY );
				if( !tile.active() || !Main.tileSolid[tile.type] ) {
					return false;
				}

				// Anchor found if:
				if( !Main.tileSolidTop[tile.type] && tile.type != TileID.MagicalIceBlock ) {
					return true;
				}
			}

			return false;
		}



		////////////////

		public static bool CanPlacePlatform( int i, int j ) {
			return TileLogic.IsSuitableForPlatform( i, j, -1 )
				|| TileLogic.IsSuitableForPlatform( i, j, 1 );
		}
	}
}