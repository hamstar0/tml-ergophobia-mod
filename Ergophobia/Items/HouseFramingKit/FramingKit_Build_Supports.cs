﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Tiles.Draw;


namespace Ergophobia.Items.HouseFramingKit {
	public partial class HouseFramingKitItem : ModItem {
		private static void MakeHouseSupports( Rectangle rect, int floorTileY ) {
			var supportLeft = new Rectangle( rect.X, floorTileY, 1, 256 );
			var supportRight = new Rectangle( rect.X + rect.Width - 1, floorTileY, 1, 256 );
			int floorLeft = floorTileY + 256;
			int floorRight = floorTileY + 256;

			var woodBeamDef = new TileDrawDefinition { TileType = TileID.WoodenBeam };

			//

			TileDrawDefinition getSupportLeftDef( int x, int y ) {
				if( y >= floorLeft ) {
					return null;
				}

				if( Main.tile[x, y].active() ) {
					if( HamstarHelpers.Helpers.Tiles.Attributes.TileAttributeHelpers.IsBreakable(x, y) ) {
						WorldGen.KillTile( x, y, false, false, true );
					} else {
						floorLeft = y;
						return null;
					}
				}
				return new TileDrawDefinition { TileType = TileID.WoodenBeam };
			}

			//

			TileDrawDefinition getSupportRightDef( int x, int y ) {
				if( y >= floorRight ) {
					return null;
				}

				if( Main.tile[x, y].active() ) {
					if( HamstarHelpers.Helpers.Tiles.Attributes.TileAttributeHelpers.IsBreakable(x, y) ) {
						WorldGen.KillTile( x, y, false, false, true );
					} else {
						floorRight = y;
						return null;
					}
				}
				return new TileDrawDefinition { TileType = TileID.WoodenBeam };
			}

			//

			TileDrawPrimitivesHelpers.DrawRectangle(
				filter: TilePattern.Any,
				area: supportLeft,
				hollow: null,
				place: getSupportLeftDef
			);
			TileDrawPrimitivesHelpers.DrawRectangle(
				filter: TilePattern.Any,
				area: supportRight,
				hollow: null,
				place: getSupportRightDef
			);

			if( Main.netMode == NetmodeID.Server ) {
				NetMessage.SendTileRange(
					whoAmi: -1,
					tileX: supportLeft.X,
					tileY: supportLeft.Y,
					xSize: supportLeft.Width,
					ySize: supportLeft.Height
				);
				NetMessage.SendTileRange(
					whoAmi: -1,
					tileX: supportRight.X,
					tileY: supportRight.Y,
					xSize: supportRight.Width,
					ySize: supportRight.Height
				);
			}
		}
	}
}