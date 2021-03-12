using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;


namespace Ergophobia.Logic {
	static partial class TileLogic {
		public static bool IsSuitableForTrack( int tileX, int tileY ) {
			bool isTrack( int x, int y ) {
				if( x < 0 || x >= Main.maxTilesX ) {
					return false;
				}
				if( y < 0 || y >= Main.maxTilesY ) {
					return false;
				}
				Tile tile = Framing.GetTileSafely( x, y );
				return tile.type == TileID.MinecartTrack;
			}

			var config = ErgophobiaConfig.Instance;

			bool foundGap = false;
			int maxGapCheck = config.Get<int>( nameof( config.MaxTrackGapPatchWidth ) );
			if( maxGapCheck == -1 ) {
				return true;
			}

			// Allow patching holes from the left
			if( isTrack( tileX - 1, tileY - 1 ) || isTrack( tileX - 1, tileY ) || isTrack( tileX - 1, tileY + 1 ) ) {

				for( int i = 1; i < maxGapCheck; i++ ) {
					if( isTrack( tileX + i, tileY - 1 ) || isTrack( tileX + i, tileY ) || isTrack( tileX + i, tileY + 1 ) ) {
						foundGap = true;
						break;
					}
				}
			}
			if( foundGap ) { return true; }

			// Allow patching holes from the right
			if( isTrack( tileX + 1, tileY - 1 ) || isTrack( tileX + 1, tileY ) || isTrack( tileX + 1, tileY + 1 ) ) {
				for( int i = 1; i < maxGapCheck; i++ ) {
					if( isTrack( tileX - i, tileY - 1 ) || isTrack( tileX - i, tileY ) || isTrack( tileX - i, tileY + 1 ) ) {
						foundGap = true;
						break;
					}
				}
			}
			if( foundGap ) { return true; }

			// Otherwise, only downward placement
			if( isTrack( tileX - 1, tileY - 1 ) || isTrack( tileX + 1, tileY - 1 ) ) {
				return true;
			}

			return false;
		}



		////////////////

		public static bool CanPlaceTrack( int i, int j ) {
			return TileLogic.IsSuitableForTrack( i, j );
		}
	}
}
