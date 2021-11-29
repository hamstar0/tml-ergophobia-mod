using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;


namespace Ergophobia.Logic {
	static partial class TileLogic {
		public static bool IsSuitableForTrack( int tileX, int tileY ) {
			bool isTrack( int x, int y ) {
				if( !WorldGen.InWorld(x, y) ) {
					return false;
				}
				Tile tile = Framing.GetTileSafely( x, y );
				return tile.type == TileID.MinecartTrack;
			}

			//

			var config = ErgophobiaConfig.Instance;

			int maxGapCheck = config.Get<int>( nameof( config.MaxTrackGapPatchWidth ) );
			if( maxGapCheck <+ -1 ) {
				return true;
			}

			// Allow patching holes from the left
			if( isTrack(tileX - 1, tileY - 1) || isTrack(tileX - 1, tileY) || isTrack(tileX - 1, tileY + 1) ) {

				for( int i = 1; i < maxGapCheck; i++ ) {
					if( isTrack(tileX + i, tileY - 1) || isTrack(tileX + i, tileY) || isTrack(tileX + i, tileY + 1) ) {
						return true;
					}
				}
			}

			// Allow patching holes from the right
			if( isTrack(tileX + 1, tileY - 1) || isTrack(tileX + 1, tileY) || isTrack(tileX + 1, tileY + 1) ) {
				for( int i = 1; i < maxGapCheck; i++ ) {
					if( isTrack(tileX - i, tileY - 1) || isTrack(tileX - i, tileY) || isTrack(tileX - i, tileY + 1) ) {
						return true;
					}
				}
			}

			// Allow placing on top of solid ground
			Tile ground = Framing.GetTileSafely( tileX, tileY + 1 );
			if( ground.active() && Main.tileSolid[ground.type] && ground.type != TileID.MinecartTrack ) {
				return true;
			}

			// Otherwise, only downward sloping tracks
			if( isTrack(tileX - 1, tileY - 1) || isTrack(tileX + 1, tileY - 1) ) {
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
