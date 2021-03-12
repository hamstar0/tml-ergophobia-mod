using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using Ergophobia.Tiles;


namespace Ergophobia.Logic {
	static partial class TileLogic {
		public static bool IsSuitableForFramingPlank( int tileX, int tileY, int dirX, int dirY ) {
			int max = dirY != 0
				? ErgophobiaConfig.Instance.MaxFramingPlankVerticalLength
				: ErgophobiaConfig.Instance.MaxFramingPlankHorizontalLength;
			if( max < 0 ) {
				return true;
			}

			int plankType = ModContent.TileType<FramingPlankTile>();

			for( int i = 1; i <= max; i++ ) {
				Tile tile = Framing.GetTileSafely( tileX + ( i * dirX ), tileY + ( i * dirY ) );
				if( !tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] ) {
					break;
				}

				// Anchor found if:
				if( tile.type != plankType ) {
					return true;
				}
			}

			return false;
		}



		////////////////
		
		public static bool CanPlaceFramingPlank( int i, int j ) {
			return TileLogic.IsSuitableForFramingPlank( i, j, -1, 0 )
				|| TileLogic.IsSuitableForFramingPlank( i, j, 1, 0 )
				|| TileLogic.IsSuitableForFramingPlank( i, j, 0, -1 )
				|| TileLogic.IsSuitableForFramingPlank( i, j, 0, 1 );
		}
	}
}
