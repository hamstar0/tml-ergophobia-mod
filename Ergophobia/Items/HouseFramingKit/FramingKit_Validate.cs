using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Tiles;


namespace Ergophobia.Items.HouseFramingKit {
	public partial class HouseFramingKitItem : ModItem {
		public static bool Validate(
					ref int leftTileX,
					ref int floorTileY,
					out ISet<(int, int)> validTiles,
					out ISet<(int, int)> inValidTiles,
					out string result ) {
			validTiles = new HashSet<(int, int)>();
			var badTiles = new List<(int, int)>();	// <- Solves a "bug" I can't figure out?

			int width = HouseFramingKitItem.FrameWidth;
			int height = HouseFramingKitItem.FrameHeight;

			//

			int dropped = 0;

			// Find ground
			while( !TileLibraries.IsSolid( Main.tile[leftTileX, floorTileY], true, true ) ) {
				floorTileY++;
				dropped++;

				if( floorTileY >= Main.maxTilesY ) {
					inValidTiles = new HashSet<(int, int)>();
					result = "Floor not found.";

					return false;
				}
			}

			if( dropped > height ) {
				inValidTiles = new HashSet<(int, int)>();
				result = "Floor too low.";

				return false;
			}

			//

			bool isOoB = false;
			var rect = new Rectangle(
				leftTileX - (width / 2),
				floorTileY - height,
				width,
				height
			);
			
			// Find clear tiles within area
			for( int y=rect.Top; y<rect.Bottom; y++ ) {
				for( int x=rect.Left; x<rect.Right; x++ ) {
					if( !WorldGen.InWorld(x, y) ) {
						isOoB = true;

						continue;
					}

					if( Main.tile[x, y].active() == true ) {
						badTiles.Add( (x, y) );
					} else {
						/*int timer = 50;
						Timers.SetTimer( "HFK_"+x+"_"+y, 2, false, () => {
							Dust.QuickDust( new Point(x, y), Color.Lime );
							return timer-- > 0;
						} );*/
						validTiles.Add( (x, y) );
					}
				}
			}

			inValidTiles = new HashSet<(int, int)>( badTiles );

			//

			if( isOoB ) {
				result = "Out of bounds.";

				return false;
			}
			
//if( SPECIAL ) {
//LogLibraries.Log(
//	$"invalid tiles {inValidTiles.Count} vs {validTiles.Count},"
//	+$" {inValidTiles.GetHashCode()} vs {validTiles.GetHashCode()}"
//);
//}
			if( badTiles.Count != 0 ) {
				result = "Invalid tiles within build space.";

				return false;
			}

			result = "Success.";
			return true;
		}
	}
}
