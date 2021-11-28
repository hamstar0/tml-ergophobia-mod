using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsGeneral.Libraries.Tiles;
using ModLibsTiles.Classes.Tiles.TilePattern;


namespace Ergophobia.Items.HouseFramingKit {
	public partial class HouseFramingKitItem : ModItem {
		public static bool Validate(
					ref int leftTileX,
					ref int floorTileY,
					out ISet<(int, int)> validTiles,
					out ISet<(int, int)> inValidTiles ) {
			validTiles = new HashSet<(int, int)>();
			inValidTiles = new HashSet<(int, int)>();

			int width = HouseFramingKitItem.FrameWidth;
			int height = HouseFramingKitItem.FrameHeight;

			int dropped = 0;

			// Find ground
			while( !TileLibraries.IsSolid( Main.tile[leftTileX, floorTileY], true, true ) ) {
				floorTileY++;
				dropped++;

				if( floorTileY >= Main.maxTilesY ) {
					return false;
				}
			}

			if( dropped > height ) {
				return false;
			}

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

					if( TilePattern.NonActive.Check(x, y) ) {
						/*int timer = 50;
						Timers.SetTimer( "HFK_"+x+"_"+y, 2, false, () => {
							Dust.QuickDust( new Point(x, y), Color.Lime );
							return timer-- > 0;
						} );*/
						validTiles.Add( (x, y) );
					} else {
						inValidTiles.Add( (x, y) );
					}
				}
			}

			return !isOoB && inValidTiles.Count == 0;
		}
	}
}
