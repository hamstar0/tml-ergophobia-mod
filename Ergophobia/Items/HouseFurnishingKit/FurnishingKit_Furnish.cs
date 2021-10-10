using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Timers;
using Ergophobia.Network;


namespace Ergophobia.Items.HouseFurnishingKit {
	public partial class HouseFurnishingKitItem : ModItem {
		public delegate bool PreFurnishHouse( int tileX, int tileY );


		public delegate void OnFurnishHouse(
			(int x, int y) innerTopLeft,
			(int x, int y) innerTopRight,
			(int x, int y) outerTopLeft,
			(int x, int y) outerTopRight,
			int floorLeft,
			int floorRight,
			int floorY,
			(int x, int y) farTopLeft,
			(int x, int y) farTopRight
		);



		////////////////

		public static void FurnishHouse(
					Player player,
					ISet<(ushort TileX, ushort TileY)> innerHouseSpace,
					ISet<(ushort TileX, ushort TileY)> fullHouseSpace,
					int floorX,
					int floorY,
					OnFurnishHouse onFinish ) {
			(int x, int y) innerTopLeft, innerTopRight;
			(int x, int y) outerTopLeft, outerTopRight;
			int floorLeft, floorRight;
			(int x, int y) farTopLeft, farTopRight;
			IDictionary<int, ISet<int>> furnishedTiles = new Dictionary<int, ISet<int>>();

			HouseFurnishingKitItem.FindHousePoints(
				innerHouseSpace: innerHouseSpace,
				outerHouseSpace: fullHouseSpace,
				floorX: floorX,
				floorY: floorY,
				outerTopLeft: out outerTopLeft,
				outerTopRight: out outerTopRight,
				innerTopLeft: out innerTopLeft,
				innerTopRight: out innerTopRight,
				floorLeft: out floorLeft,
				floorRight: out floorRight,
				farTopLeft: out farTopLeft,
				farTopRight: out farTopRight
			);

			HouseFurnishingKitItem.CleanHouse( fullHouseSpace );

			//

			(bool success, int _) checkPlacement( int x, int y, int width ) {
				for( int x2 = x; x2 < x+width; x2++ ) {
					for( int y2 = y; y2 >= y-3; y2-- ) {
						if( Main.tile[x2, y2].active() && Main.tileSolid[Main.tile[x2, y2].type] ) {
							return (false, -1);
						}
					}
				}
				return (true, -1);
			}
			
			(bool success, int tileType) placeBed( int x, int y ) {
				if( !checkPlacement( x, y, 4 ).success ) {
					return (false, TileID.Beds);
				}
				bool success = HouseFurnishingKitItem.MakeHouseTile( x, y, TileID.Beds, 0, 1, fullHouseSpace, furnishedTiles );
				return (success, TileID.Beds);
			}
			(bool success, int tileType) placeWorkbench( int x, int y ) {
				if( !checkPlacement(x, y, 2).success ) {
					return (false, TileID.WorkBenches);
				}
				return (
					HouseFurnishingKitItem.MakeHouseTile( x, y, TileID.WorkBenches, 0, 1, fullHouseSpace, furnishedTiles ),
					TileID.WorkBenches
				);
			}
			(bool success, int tileType) placeChair( int x, int y ) {
				if( !checkPlacement(x, y, 1).success ) {
					return (false, TileID.Chairs);
				}
				return (
					HouseFurnishingKitItem.MakeHouseTile( x, y, TileID.Chairs, 0, 1, fullHouseSpace, furnishedTiles ),
					TileID.Chairs
				);
			}
			(bool success, int tileType) placeTorch( int x, int y ) {
				return (WorldGen.PlaceTile( x, y, TileID.Torches ), TileID.Torches);
			}

			//

			var config = ErgophobiaConfig.Instance;

			Timers.SetTimer( "HouseKitsFurnishingDelay", 1, false, () => {
				HouseFurnishingKitItem.MakeHouseWalls( fullHouseSpace );
				HouseFurnishingKitItem.MakeHouseTileNear( placeBed,			floorLeft,		floorY,			fullHouseSpace, furnishedTiles );
				HouseFurnishingKitItem.MakeHouseTileNear( placeWorkbench,	floorRight - 2,	floorY,			fullHouseSpace, furnishedTiles );
				HouseFurnishingKitItem.MakeHouseTileNear( placeChair,		floorRight - 3,	floorY,			fullHouseSpace, furnishedTiles );
				HouseFurnishingKitItem.MakeHouseCustomFurnishings( floorLeft, floorRight,	floorY,			fullHouseSpace, furnishedTiles );
				HouseFurnishingKitItem.MakeHouseTileNear( placeTorch,		innerTopLeft.x,	innerTopLeft.y, fullHouseSpace, furnishedTiles );
				HouseFurnishingKitItem.MakeHouseTileNear( placeTorch,		innerTopRight.x,innerTopRight.y,fullHouseSpace, furnishedTiles );

				if( config.Get<int>( nameof(config.FurnishedCustomFloorTile) ) > 0 ) {
					HouseFurnishingKitItem.ChangeFlooring(
						(ushort)config.Get<int>( nameof(config.FurnishedCustomFloorTile) ),
						floorLeft,
						floorRight,
						floorY
					);
				}

				onFinish( innerTopLeft, innerTopRight, outerTopLeft, outerTopRight, floorLeft, floorRight, floorY, farTopLeft, farTopRight );

				if( Main.netMode == NetmodeID.Server ) {
					int width = outerTopRight.x - outerTopLeft.x;
					int height = (floorY - outerTopLeft.y) + 2;

					Timers.SetTimer( "PrefabKitsFurnishingKitLeft", 30, false, () => {
//LogLibraries.Log( "!!!FurnishHouse 1 " + outerTopLeft.ToString()+", "+(width / 2)+", "+height );
						TileRectangleModPacketProtocol.Send( new Rectangle(
							x: outerTopLeft.x,
							y: outerTopLeft.y,
							width: width / 2,
							height: height
						) );
						return false;
					} );

					Timers.SetTimer( "PrefabKitsFurnishingKitRight", 45, false, () => {
//LogLibraries.Log( "!!!FurnishHouse 2 "+(outerTopLeft.x + (width/2))+", "+outerTopLeft.y+", "+(width / 2)+", "+height );
						TileRectangleModPacketProtocol.Send( new Rectangle(
							x: outerTopLeft.x + ( width / 2 ),
							y: outerTopLeft.y,
							width: (width - (width/2)) + 1,
							height: height
						) );
						return false;
					} );
				}

				return false;
			} );
		}


		////////////////

		private static void OutputPlacementError( int tileX, int tileY, int tileType, string context ) {
			if( ErgophobiaConfig.Instance.DebugModeSuppressPlacementErrors ) {
				return;
			}

			LogLibraries.Log( "Could not place "+context+" "
				+ ( tileType >= TileID.Count || tileType < 0
					? tileType.ToString()
					: TileID.Search.GetName(tileType) )
				+ " at "+tileX+", "+tileY
			);

			LogLibraries.Log( "  "+(tileX-1)+", "+(tileY-1)+" - "+Main.tile[tileX-1, tileY-1].ToString() );
			LogLibraries.Log( "  "+(tileX)+", "+(tileY-1)+" - "+Main.tile[tileX, tileY-1].ToString() );
			LogLibraries.Log( "  "+(tileX+1)+", "+(tileY-1)+" - "+Main.tile[tileX+1, tileY-1].ToString() );
			LogLibraries.Log( "  "+(tileX-1)+", "+(tileY)+" - "+Main.tile[tileX-1, tileY].ToString() );
			LogLibraries.Log( "  "+(tileX)+", "+(tileY)+" - "+Main.tile[tileX, tileY].ToString() );
			LogLibraries.Log( "  "+(tileX+1)+", "+(tileY)+" - "+Main.tile[tileX+1, tileY].ToString() );
			LogLibraries.Log( "  "+(tileX-1)+", "+(tileY+1)+" - "+Main.tile[tileX-1, tileY+1].ToString() );
			LogLibraries.Log( "  "+(tileX)+", "+(tileY+1)+" - "+Main.tile[tileX, tileY+1].ToString() );
			LogLibraries.Log( "  "+(tileX+1)+", "+(tileY+1)+" - "+Main.tile[tileX+1, tileY+1].ToString() );
		}
	}
}
