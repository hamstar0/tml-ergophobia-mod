using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Tiles.Draw;
using HamstarHelpers.Helpers.Tiles;


namespace Ergophobia.Items.HouseFramingKit {
	public partial class HouseFramingKitItem : ModItem {
		public static bool Validate( ref int leftTileX, ref int floorTileY, out ISet<(int, int)> tiles ) {
			int width = HouseFramingKitItem.FrameWidth;
			int height = HouseFramingKitItem.FrameHeight;
			var myTiles = new HashSet<(int, int)>();

			int dropped = 0;

			// Find ground
			while( !TileHelpers.IsSolid( Main.tile[leftTileX, floorTileY], true, true ) ) {
				floorTileY++;
				dropped++;

				if( floorTileY >= Main.maxTilesY ) {
					tiles = myTiles;
					return false;
				}
			}

			if( dropped > height ) {
				tiles = myTiles;
				return false;
			}

			var outerRect = new Rectangle(
				leftTileX - (width / 2),
				floorTileY - height,
				width,
				height
			);
			int availableArea = 0;

			TileDrawPrimitivesHelpers.DrawRectangle(
				filter: TilePattern.NonActive,
				area: outerRect,
				hollow: null,//innerRect,
				place: ( x, y ) => {
					/*int timer = 50;
					Timers.SetTimer( "HFK_"+x+"_"+y, 2, false, () => {
						Dust.QuickDust( new Point(x, y), Color.Lime );
						return timer-- > 0;
					} );*/
					myTiles.Add( (x, y) );
					availableArea++;
					return null;
				}
			);

			tiles = myTiles;
			return availableArea >= (width * height);
		}
	}
}
