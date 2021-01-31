using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Tiles;
using Ergophobia.Tiles;


namespace Ergophobia.Items {
	public partial class ScaffoldingErectorKitItem : ModItem {
		public static bool Validate( ref int leftTileX, ref int floorTileY, out Rectangle area ) {
			int width = ScaffoldingErectorKitItem.ScaffoldWidth;
			int height = ScaffoldingErectorKitItem.ScaffoldHeight;

			int dropped = 0;

			// Find ground
			while( !TileHelpers.IsSolid(Main.tile[leftTileX, floorTileY], true, false) ) {
				floorTileY++;
				dropped++;

				if( floorTileY >= Main.maxTilesY ) {
					area = new Rectangle();
					return false;
				}
			}

			if( dropped > height ) {
				area = new Rectangle();
				return false;
			}

			// Ensure minimum ground
			if( !ScaffoldingErectorKitItem.ValidateBeneath(leftTileX, floorTileY) ) {
				area = new Rectangle();
				return false;
			}

			area = new Rectangle( leftTileX, floorTileY, width, height );

			// Enure clear space
			for( int j = 0; j < height; j++ ) {
				for( int i = 0; i < width; i++ ) {
					if( Main.tile[i, j].active() ) {
						return false;
					}
				}
			}

			return true;
		}


		public static bool ValidateBeneath( int leftTileX, int floorTileY ) {
			int width = ScaffoldingErectorKitItem.ScaffoldWidth;
			int height = ScaffoldingErectorKitItem.ScaffoldHeight;
			int maxX = leftTileX + width;
			int maxY = floorTileY + (height * 3);
			int framingPlankType = ModContent.TileType<FramingPlankTile>();

			bool foundGround = false;

			for( int x=leftTileX; x<maxX; x++ ) {
				foundGround = false;

				for( int y=floorTileY; y<maxY; y++ ) {
					Tile tile = Main.tile[x, y];

					if( TileHelpers.IsSolid(tile, false, false) && tile.type != framingPlankType ) {
						foundGround = true;
						break;
					}
				}

				if( !foundGround ) {
					break;
				}
			}

			return foundGround;
		}
	}
}
