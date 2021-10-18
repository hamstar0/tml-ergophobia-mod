using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;


namespace Ergophobia.Logic {
	static partial class TileLogic {
		public static bool IsSuitableForRope( int tileX, int tileY ) {
			bool isRope( int x, int y ) {
				Tile tile = Framing.GetTileSafely( x, y );
				return isRopeTile( tile );
			}
			bool isRopeTile( Tile tile ) {
				return tile.type == TileID.Rope
					|| tile.type == TileID.SilkRope
					|| tile.type == TileID.VineRope
					|| tile.type == TileID.WebRope
					|| tile.type == TileID.Chain;
			}

			//

			if( Framing.GetTileSafely( tileX, tileY ).wall != 0 ) {
				return true;
			}

			if( isRope( tileX, tileY ) ) {
				return true;
			}

			if( !isRope( tileX, tileY + 1 ) ) {
				return true;
			}

			if( Framing.GetTileSafely( tileX, tileY - 1 ).active() ) {  //isRope(tileX, tileY-1) ) {
				return true;
			}

			return false;
		}



		////////////////

		public static bool CanPlaceRope( int i, int j ) {
			return TileLogic.IsSuitableForRope( i, j );
		}
	}
}
