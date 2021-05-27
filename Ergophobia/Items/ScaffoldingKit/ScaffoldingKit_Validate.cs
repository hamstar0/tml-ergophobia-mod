using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Tiles;
using Ergophobia.Tiles;


namespace Ergophobia.Items.ScaffoldingKit {
	public partial class ScaffoldingErectorKitItem : ModItem {
		public static int GetFurthestAllowedGroundTileY( int placementTileY ) {
			return placementTileY + (ScaffoldingErectorKitItem.ScaffoldHeight * 2) + 1;
		}


		////

		public static bool Validate( int tileX, int tileY, int offsetY, out Rectangle area ) {
			tileY += offsetY;

			int width = ScaffoldingErectorKitItem.ScaffoldWidth;
			int height = ScaffoldingErectorKitItem.ScaffoldHeight - offsetY;
			int leftTileX = Math.Max( tileX - (width / 2), 1 );
			int floorTileY = ScaffoldingErectorKitItem.FindScaffoldFloorY( leftTileX, tileY, width, height );

			if( (floorTileY - tileY) > height ) {
				area = new Rectangle();
				return false;
			}

			// Ensure minimum ground prereq
			if( !ScaffoldingErectorKitItem.ValidateBeneathFloor(leftTileX, floorTileY) ) {
				area = new Rectangle();
				return false;
			}

			area = new Rectangle(
				Math.Min( leftTileX, Main.maxTilesX - width - 1 ),
				Math.Max( floorTileY - height, 1 ),
				width,
				height
			);

			// Ensure clear space
			for( int j = 0; j < height; j++ ) {
				for( int i = 0; i < width; i++ ) {
					int x = i + area.X;
					int y = j + area.Y;
					if( !WorldGen.InWorld(x, y) ) {
						return false;
					}

					Tile tile = Main.tile[x, y];
					if( tile?.active() == true ) {
						switch( tile.type ) {
						case TileID.Grass:
						case TileID.CorruptGrass:
						case TileID.FleshGrass:
						case TileID.HallowedGrass:
						case TileID.JungleGrass:
						case TileID.MushroomGrass:
							//tile.active( false );
							break;
						default:
							return false;
						}
					}
				}
			}

			return true;
		}


		private static bool ValidateBeneathFloor( int leftTileX, int floorTileY ) {
			int maxX = leftTileX + ScaffoldingErectorKitItem.ScaffoldWidth;
			int maxY = ScaffoldingErectorKitItem.GetFurthestAllowedGroundTileY( floorTileY );
			int framingPlankType = ModContent.TileType<FramingPlankTile>();

			// Find at least one 'earth' tile beneath
			for( int x=leftTileX; x<maxX; x++ ) {
				for( int y=floorTileY; y<maxY; y++ ) {
					Tile tile = Main.tile[ x, y ];
					if( tile?.active() != true ) {
						continue;
					}

					if( TileLibraries.IsSolid(tile, false, false) && tile.type != framingPlankType ) {
						return true;
					}
				}
			}

			return false;
		}


		////

		private static int FindScaffoldFloorY( int leftTileX, int tileY, int width, int height ) {
			int maxX = leftTileX + width;
			int maxY = Math.Min( tileY + (height + 1), Main.maxTilesY );

			// Find immediate 'ground' (any solid, non-actuated matter)
			for( int y = tileY; y < maxY; y++ ) {
				for( int x = leftTileX; x < maxX; x++ ) {
					if( TileLibraries.IsSolid( Main.tile[x, y], true, false ) ) {
						return y;
					}
				}
			}

			return maxY;
		}
	}
}
