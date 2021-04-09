using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Ergophobia.Tiles;


namespace Ergophobia.Logic {
	static partial class TilesInterfaceLogic {
		private static void DrawPlankTilePlacementOutline( float outlineIntensity ) {
			var config = ErgophobiaConfig.Instance;
			int tileX = ( (int)Main.screenPosition.X + Main.mouseX ) >> 4;
			int tileY = ( (int)Main.screenPosition.Y + Main.mouseY ) >> 4;
			int plankTileType = ModContent.TileType<FramingPlankTile>();

			//

			bool isAnchor( int x, int y ) {
				Tile tile = Main.tile[x, y];
				bool isMyAnchor = tile.active()
					&& Main.tileSolid[tile.type]
					&& !Main.tileSolidTop[tile.type]
					&& tile.type != plankTileType;

				return isMyAnchor;
			}

			int trace( int dirX, int dirY ) {
				int max = dirY != 0
					? config.Get<int>( nameof(config.MaxFramingPlankVerticalLength) )
					: config.Get<int>( nameof(config.MaxFramingPlankHorizontalLength) );

				for( int i = 0; i < max; i++ ) {
					if( isAnchor( tileX + ( i * dirX ), tileY + ( i * dirY ) ) ) {
						return i;
					}
				}
				return max;
			}

			//

			if( !isAnchor( tileX, tileY ) ) {
				if( isAnchor( tileX - 1, tileY ) ) {
					TilesInterfaceLogic.DrawTilePlacementOutline( outlineIntensity, new Rectangle( tileX, tileY, trace( 1, 0 ), 1 ) );
				} else if( isAnchor( tileX + 1, tileY ) ) {
					int width = trace( -1, 0 );
					TilesInterfaceLogic.DrawTilePlacementOutline( outlineIntensity, new Rectangle( ( tileX - width ) + 1, tileY, width, 1 ) );
				}

				if( isAnchor( tileX, tileY - 1 ) ) {
					TilesInterfaceLogic.DrawTilePlacementOutline( outlineIntensity, new Rectangle( tileX, tileY, 1, trace( 0, 1 ) ) );
				} else if( isAnchor( tileX, tileY + 1 ) ) {
					int height = trace( 0, -1 );
					TilesInterfaceLogic.DrawTilePlacementOutline( outlineIntensity, new Rectangle( tileX, ( tileY - height ) + 1, 1, height ) );
				}
			}
		}
	}
}
