using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Tiles;


namespace Ergophobia.Items.HouseFurnishingKit {
	public partial class HouseFurnishingKitItem : ModItem {
		private static void MakeHouseCustomFurnishings(
					int leftTileX,
					int rightTileX,
					int floorTileY,
					ISet<(ushort TileX, ushort TileY)> houseTiles,
					IDictionary<int, ISet<int>> furnishedTiles ) {
			var config = ErgophobiaConfig.Instance;

			ushort custFurnType = (ushort)config.Get<int>( nameof(config.FurnishedCustomFurnitureTile) );
			if( custFurnType > 0 ) {
				HouseFurnishingKitItem.MakeHouseCustomMainFurniture(
					custFurnType,
					leftTileX,
					floorTileY,
					houseTiles,
					furnishedTiles
				);
			}

			ushort custWallMount1 = (ushort)config.Get<int>( nameof(config.FurnishedCustomWallMount1Tile) );
			if( custWallMount1 != 0 ) {
				HouseFurnishingKitItem.MakeHouseTileNear(
					( x, y ) => {
						if( y >= floorTileY - 1 ) { //floorTileY - 4
							return (false, custWallMount1);
						}
						return (
							HouseFurnishingKitItem.MakeHouseWallTile3x3( x, y, custWallMount1, furnishedTiles ),
							custWallMount1
						);
					},
					leftTileX + 1,
					floorTileY - 3,
					houseTiles,
					furnishedTiles
				);
			}

			ushort custWallMount2 = (ushort)config.Get<int>( nameof(config.FurnishedCustomWallMount2Tile) );
			if( custWallMount2 != 0 ) {
				HouseFurnishingKitItem.MakeHouseTileNear(
					( x, y ) => {
						if( y >= floorTileY - 1 ) { //floorTileY - 4
							return (false, custWallMount2);
						}
						return (
							HouseFurnishingKitItem.MakeHouseWallTile3x3( x, y, custWallMount2, furnishedTiles ),
							custWallMount2
						);
					},
					rightTileX - 3,
					floorTileY - 4,
					houseTiles,
					furnishedTiles
				);
			}
		}


		private static void MakeHouseCustomMainFurniture(
					ushort tileType,
					int leftTileX,
					int floorTileY,
					ISet<(ushort TileX, ushort TileY)> houseTiles,
					IDictionary<int, ISet<int>> occupiedTiles ) {
			(bool, int) placeTile( int x, int y ) {
				switch( tileType ) {
				case TileID.Bottles:
					TilePlacementLibraries.Place2x1( x, y, TileID.WorkBenches );
					if( WorldGen.PlaceTile( x, y - 1, TileID.Bottles ) ) {
						HouseFurnishingKitItem.MarkFurnishedTiles( x, y - 1, 2, 2, occupiedTiles );
						return (true, tileType);
					}
					break;
				case TileID.PiggyBank:
					TilePlacementLibraries.Place2x1( x, y, TileID.WorkBenches );
					TilePlacementLibraries.Place2x1( x, y - 1, TileID.PiggyBank );
					if( Main.tile[x, y].type == TileID.WorkBenches ) {
						HouseFurnishingKitItem.MarkFurnishedTiles( x, y - 1, 2, 2, occupiedTiles );
						return (true, tileType);
					}
					break;
				default:
					return (
						HouseFurnishingKitItem.MakeHouseTile( x, y, tileType, 0, -1, houseTiles, occupiedTiles ),
						tileType
					);
				}

				HouseFurnishingKitItem.OutputPlacementError( x, y, tileType, "custom main furniture" );

				return (false, tileType);
			}

			HouseFurnishingKitItem.MakeHouseTileNear(
				placeTile,
				leftTileX + 4,
				floorTileY,
				houseTiles,
				occupiedTiles
			);
		}
	}
}
