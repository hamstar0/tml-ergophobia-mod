using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Tiles.Walls;
using HamstarHelpers.Services.Timers;


namespace Ergophobia.Items.HouseFurnishingKit {
	public enum HouseViabilityState {
		Good,
		TooSmall,
		TooSmallInner,
		TooLarge,
		SmallFloor,
		ContainsOccupiedChest
	}




	public partial class HouseFurnishingKitItem : ModItem {
		public static string GetViabilityStateMessage(
					HouseViabilityState state,
					int fullSpace,
					int innerSpace,
					bool verbose,
					out Color color ) {
			int minFurnishArea;
			var config = ErgophobiaConfig.Instance;

			switch( state ) {
			case HouseViabilityState.Good:
				color = Color.Lime;
				string msg = "Valid town house space found. It can be furnished with a kit, if needed.";
				if( verbose ) {
					msg += "\nNote: Only above ground houses automatically gain occupants.";
				}
				return msg;
			case HouseViabilityState.TooSmall:
				minFurnishArea = config.Get<int>( nameof(config.MinimumFurnishableHouseArea) );
				color = Color.Yellow;
				return "House too small ("+fullSpace+" of "+minFurnishArea+" blocks needed).";
			case HouseViabilityState.TooSmallInner:
				minFurnishArea = config.Get<int>( nameof(config.MinimumFurnishableHouseArea) );
				color = Color.Yellow;
				return "House too small inside ("+innerSpace+" of "+(minFurnishArea/2)+" blocks needed).";
			case HouseViabilityState.TooLarge:
				color = Color.Yellow;
				return "House too large or not a closed space.";
			case HouseViabilityState.SmallFloor:
				color = Color.Yellow;
				return "Not enough floor space.";
			}

			color = Color.Transparent;
			return "";
		}

		////

		public static bool IsCleanableTile( Tile tile ) {
			if( !tile.active() ) {
				return false;
			}

			switch( tile.type ) {
			case TileID.Platforms:
				return tile.slope() != 0;	// stairs
			//
			case TileID.MinecartTrack:
			case TileID.Torches:
			case TileID.Rope:
			case TileID.SilkRope:
			case TileID.VineRope:
			case TileID.WebRope:
			case TileID.Chain:
			//
			case TileID.Tombstones:
			case TileID.CopperCoinPile:
			case TileID.SilverCoinPile:
			case TileID.GoldCoinPile:
			case TileID.PlatinumCoinPile:
			case TileID.Stalactite:
			case TileID.SmallPiles:
			case TileID.LargePiles:
			case TileID.LargePiles2:
			case TileID.ExposedGems:
			//
			//case TileID.Heart:
			case TileID.Pots:
			//case TileID.ShadowOrbs:
			//case TileID.DemonAltar:
			case TileID.LifeFruit:
			//case TileID.PlanteraBulb:
			case TileID.Bottles:
			case TileID.Books:
			case TileID.WaterCandle:
			case TileID.PeaceCandle:
			//
			case TileID.HoneyDrip:
			case TileID.LavaDrip:
			case TileID.SandDrip:
			case TileID.WaterDrip:
			//
			case TileID.BreakableIce:
			case TileID.MagicalIceBlock:
				return true;
			default:
				if( TileGroupIdentityHelpers.VanillaShrubTiles.Contains( tile.type ) ) {
					return true;
				}
				break;
			}

			return !Main.tileSolid[ tile.type ]
				|| (tile.type != TileID.Platforms && Main.tileSolidTop[ tile.type ]);
		}


		////////////////

		public static HouseViabilityState IsValidHouse( int tileX, int tileY ) {
			ISet<(ushort TileX, ushort TileY)> innerHouseSpace;
			ISet<(ushort TileX, ushort TileY)> fullHouseSpace;
			int floorX, floorY;

			return HouseFurnishingKitItem.IsValidHouse(
				tileX,
				tileY,
				out innerHouseSpace,
				out fullHouseSpace,
				out floorX,
				out floorY
			);
		}


		public static HouseViabilityState IsValidHouse(
					int tileX,
					int tileY,
					out ISet<(ushort TileX, ushort TileY)> innerHouseSpace,
					out ISet<(ushort TileX, ushort TileY)> fullHouseSpace,
					out int floorX,
					out int floorY ) {
			HouseViabilityState state;

			//

			bool containsUnsafeChest( int x, int y ) {
				Tile tile = Main.tile[x, y];
				if( tile.type == TileID.Containers || tile.type == TileID.Containers2 || tile.type == TileID.FakeContainers || tile.type == TileID.FakeContainers2 ) {
					int chestIdx = Chest.FindChestByGuessing( tileX, tileY );
					if( chestIdx != -1 && Main.chest[chestIdx].item.Any( i => i?.IsAir == false ) ) {
						return true;   // No non-empty chests
					}
				}
				return false;
			}

			bool isStairOrNotSolid( int x, int y ) {
				Tile tile = Main.tile[ x, y ];
				if( TileWallGroupIdentityHelpers.UnsafeDungeonWallTypes.Contains( tile.wall ) ) {
					return false;
				}
				if( tile.wall == WallID.LihzahrdBrickUnsafe ) {
					return false;
				}
				if( !tile.active() ) {
					return true;
				}
				if( tile.type == TileID.OpenDoor || tile.type == TileID.TallGateOpen || tile.type == TileID.TrapdoorOpen ) {
					return false;
				}
				return !Main.tileSolid[ tile.type ]
					|| (Main.tileSolidTop[tile.type] && tile.slope() != 0)  //stair
					|| HouseFurnishingKitItem.IsCleanableTile( tile );
			}

			//

			TilePattern fillPattern = new TilePattern( new TilePatternBuilder {
				AreaFromCenter = new Rectangle( -1, -1, 3, 3 ),
				HasWater = false,
				HasHoney = false,
				HasLava = false,
				IsActuated = false,
				CustomCheck = isStairOrNotSolid
			} );

			//

			var config = ErgophobiaConfig.Instance;
			int minVolume = config.Get<int>( nameof(config.MinimumFurnishableHouseArea) );	//78
			int minFloorWid = config.Get<int>( nameof(config.MinimumFurnishableHouseFloorWidth) );    //12

			state = HouseFurnishingKitItem.IsValidHouseByCriteria(
				pattern: new TilePattern( new TilePatternBuilder {
					CustomCheck = isStairOrNotSolid
				} ),
				tileX: tileX,
				tileY: tileY,
				minimumVolume: minVolume,
				minimumFloorWidth: minFloorWid,
				houseSpace: out fullHouseSpace,
				floorX: out floorX,
				floorY: out floorY
			);

			if( ErgophobiaConfig.Instance.DebugModeInfo ) {
				Main.NewText( "Full house space: " + fullHouseSpace.Count + " of 80" );
			}

			if( state != HouseViabilityState.Good ) {
				innerHouseSpace = fullHouseSpace;
				return state;
			}

			//

			IList<(ushort TileX, ushort TileY)> unsafeChestTiles = TileFinderHelpers.GetTileMatchesInWorldRectangle(
				pattern: new TilePattern( new TilePatternBuilder {
					CustomCheck = containsUnsafeChest
				} ),
				worldRect: new Rectangle(
					tileX,
					tileY,
					(int)fullHouseSpace.Aggregate( (xy1, xy2) => xy1.TileX > xy2.TileX ? xy1 : xy2 ).TileX - tileX,
					(int)fullHouseSpace.Aggregate( (xy1, xy2) => xy1.TileY > xy2.TileY ? xy1 : xy2 ).TileY - tileY
				)
			);
			if( unsafeChestTiles.Count > 0 ) {
				innerHouseSpace = fullHouseSpace;
				return HouseViabilityState.ContainsOccupiedChest;
			}

			//

			int altFloorY = floorY;
			state = HouseFurnishingKitItem.IsValidHouseByCriteria(
				pattern: fillPattern,
				tileX: floorX,
				tileY: tileY,
				minimumVolume: minVolume / 2,
				minimumFloorWidth: minFloorWid - 1,
				houseSpace: out innerHouseSpace,
				floorX: out floorX,
				floorY: out altFloorY
			);

			if( ErgophobiaConfig.Instance.DebugModeInfo ) {
				var myInnerHouseSpace = innerHouseSpace;
				int timer = 120;

				Timers.SetTimer( "HouseKitsInnerSpace", 2, false, () => {
					foreach( (int x, int y) in myInnerHouseSpace ) {
						Dust.QuickDust( new Point(x, y), Color.Lime );
					}
					return timer-- > 0;
				} );

				Main.NewText( "Inner house space: " + innerHouseSpace.Count + " of 60" );
			}

			if( state != HouseViabilityState.Good ) {
				return state == HouseViabilityState.TooSmall
					? HouseViabilityState.TooSmallInner
					: state;
			}

			//

			return state;
		}


		////

		private static HouseViabilityState IsValidHouseByCriteria(
					TilePattern pattern,
					int tileX,
					int tileY,
					int minimumVolume,
					int minimumFloorWidth,
					out ISet<(ushort TileX, ushort TileY)> houseSpace,
					out int floorX,
					out int floorY ) {
			ISet<(ushort TileX, ushort TileY)> unclosedTiles;
			houseSpace = TileFinderHelpers.GetAllContiguousMatchingTilesAt(
				pattern: pattern,
				tileX: tileX,
				tileY: tileY,
				excessTiles: out unclosedTiles,
				maxRadius: 64
			);

			if( unclosedTiles.Count > 0 ) {
				floorX = floorY = 0;
				return HouseViabilityState.TooLarge;
			}

			if( houseSpace.Count < minimumVolume ) {
				floorX = floorY = 0;
				return HouseViabilityState.TooSmall;
			}

			int floorWidth = TileFinderHelpers.GetFloorWidth(
				nonFloorPattern: pattern,
				tileX: tileX,
				tileY: tileY - 2,
				maxFallRange: 32,
				floorX: out floorX,
				floorY: out floorY
			);
			floorX += floorWidth / 2;

			if( floorWidth < minimumFloorWidth ) {
				return HouseViabilityState.SmallFloor;
			}

			return HouseViabilityState.Good;
		}
	}
}
