using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace Ergophobia.Logic {
	static partial class TilesInterfaceLogic {
		private static void DrawPlatformTilePlacementOutline( float outlineIntensity ) {
			var config = ErgophobiaConfig.Instance;
			int maxLength = config.Get<int>( nameof(config.MaxPlatformBridgeLength) );
			int tileX = ( (int)Main.screenPosition.X + Main.mouseX ) >> 4;
			int tileY = ( (int)Main.screenPosition.Y + Main.mouseY ) >> 4;

			//

			bool isAnchor( int x, int y ) {
				Tile tile = Main.tile[x, y];
				return tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type];
			}

			int traceRight() {
				for( int i = 0; i < maxLength; i++ ) {
					if( isAnchor( tileX + i, tileY ) ) {
						return i;
					}
				}
				return maxLength;
			}
			int traceLeft() {
				for( int i = 0; i < maxLength; i++ ) {
					if( isAnchor( tileX - i, tileY ) ) {
						return i;
					}
				}
				return maxLength;
			}

			//

			if( !isAnchor( tileX, tileY ) ) {
				if( isAnchor( tileX - 1, tileY ) ) {
					TilesInterfaceLogic.DrawTilePlacementOutline( outlineIntensity, new Rectangle( tileX, tileY, traceRight(), 1 ) );
				} else if( isAnchor( tileX + 1, tileY ) ) {
					int width = traceLeft();
					TilesInterfaceLogic.DrawTilePlacementOutline( outlineIntensity, new Rectangle( ( tileX - width ) + 1, tileY, width, 1 ) );
				} else {
					if( isAnchor( tileX, tileY - 1 ) || isAnchor( tileX, tileY + 1 ) ) {
						TilesInterfaceLogic.DrawTilePlacementOutline( outlineIntensity, new Rectangle( tileX, tileY, 1, 1 ), false );
					}
				}
			}
		}
	}
}
