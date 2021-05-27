using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Tiles;
using ModLibsTiles.Classes.Tiles.TilePattern;
using ModLibsTiles.Libraries.Tiles.Draw;
using Ergophobia.Network;


namespace Ergophobia.Items.HouseFramingKit {
	public partial class HouseFramingKitItem : ModItem {
		public static void MakeHouseFrame( int midTileX, int floorTileY ) {
			int width = HouseFramingKitItem.FrameWidth;
			int height = HouseFramingKitItem.FrameHeight;
			var outerRect = new Rectangle(
				midTileX - (width / 2),
				floorTileY - height,
				width,
				height
			);
			var innerRect = outerRect;
			innerRect.X += 1;
			innerRect.Y += 1;
			innerRect.Width -= 2;
			innerRect.Height -= 2;

			var frameTileDef = new TileDrawDefinition { TileType = TileID.WoodBlock };

			//

			bool isSolidFrame( int x, int y )
				=> HouseFramingKitItem.IsHouseFrameTileSolid( x, y, width, height, outerRect );

			//

			TileDrawPrimitivesLibraries.DrawRectangle(
				filter: TilePattern.NonActive,
				area: outerRect,
				hollow: innerRect,
				place: (x, y) => isSolidFrame(x, y) ? frameTileDef : null
			);
			TileDrawPrimitivesLibraries.DrawRectangle(
				filter: TilePattern.NonActive,
				area: outerRect,
				hollow: null,
				place: ( int x, int y ) => HouseFramingKitItem.GetHouseFrameTileDefAt1(x, y, width, height, outerRect)
			);
			TileDrawPrimitivesLibraries.DrawRectangle(
				filter: TilePattern.NonActive,
				area: outerRect,
				hollow: null,
				place: ( int x, int y ) => HouseFramingKitItem.GetHouseFrameTileDefAt2(x, y, width, height, outerRect)
			);

			/*int ceiling = floorTileY - height;

			Tile tile1 = Main.tile[ midTileX-3, ceiling+1 ];
			tile1.slope( 1 );
			Tile tile2 = Main.tile[ midTileX+2, ceiling+1 ];
			tile2.slope( 2 );*/

			if( Main.netMode == NetmodeID.Server ) {
//LogLibraries.Log( "!!!MakeHouseFrame "+outerRect.ToString() );
				TileRectangleModPacketProtocol.Send( outerRect );
			}

			//

			HouseFramingKitItem.MakeHouseSupports( outerRect, floorTileY );

			Main.PlaySound( SoundID.Item108, new Vector2(midTileX*16, floorTileY*16) );
		}


		////

		private static bool IsHouseFrameTileSolid(
					int x,
					int y,
					int width,
					int height,
					Rectangle outerRect ) {
			int offX = x - outerRect.X;
			int offY = y - outerRect.Y;
			int doorTop = height - 4;
			int doorBot = height - 2;
			int midLeft = (width / 2) - 3;
			int midRight = (width / 2) + 3;

			if( offX == 0 || offX == width - 1 ) {
				if( offY >= doorTop && offY <= doorBot ) {
					return false;
				}
			} else if( offX >= midLeft && offX < midRight ) {
				if( offY == 0 ) {
					return false;
				}
			}

			return true;
/*bool isActive = Main.tile[x, y].active();
int timer = 150;
Timers.SetTimer( "HFK0_"+x+"_"+y, 2, false, () => {
	Dust.QuickDust( new Point(x, y), isActive ? Color.Purple : Color.Blue );
	return timer-- > 0;
} );*/
		}


		private static TileDrawDefinition GetHouseFrameTileDefAt1(
					int x,
					int y,
					int width,
					int height,
					Rectangle outerRect ) {
			TileDrawDefinition myTileDef = null;
			int offX = x - outerRect.X;
			int offY = y - outerRect.Y;
			int doorTop = height - 4;
			int doorBot = height - 2;
			int midLeft = ( width / 2 ) - 3;
			int midRight = ( width / 2 ) + 3;

			// Side walls
			if( offX == 0 || offX == width - 1 ) {
				// Vertical just-above-just-above-floor area
				if( offY >= doorTop && offY <= (doorBot - 1) ) {
					myTileDef = null;
				}
			} else
			// Horizontal middle
			if( offX >= midLeft && offX < midRight ) {
				// Top
				if( offY == 0 ) {
					if( offX == (midLeft+1) ) {
						myTileDef = new TileDrawDefinition {
							TileType = TileID.Platforms,
							TileStyle = 0,
							Shape = TileShapeType.TopRightSlope
						};
					} else if( offX == (midRight-2) ) {
						myTileDef = new TileDrawDefinition {
							TileType = TileID.Platforms,
							TileStyle = 0,
							Shape = TileShapeType.TopLeftSlope
						};
					} else {
						myTileDef = new TileDrawDefinition {
							TileType = TileID.Platforms,
							TileStyle = 0
						};
					}
				}
				else if( offY == 1 ) {
					if( offX >= (midLeft+1) && offX < (midRight-1) ) {
						myTileDef = new TileDrawDefinition {
							TileType = TileID.Platforms,
							TileStyle = 0
						};
					}
				}
			}

			return myTileDef;
		}


		private static TileDrawDefinition GetHouseFrameTileDefAt2(
					int x,
					int y,
					int width,
					int height,
					Rectangle outerRect ) {
			TileDrawDefinition myTileDef = null;
			int offX = x - outerRect.X;
			int offY = y - outerRect.Y;
			int doorMid = height - 4;

			// Side walls
			if( offX == 0 || offX == width - 1 ) {
				// Vertical just-above-floor
				if( offY == doorMid ) {
					myTileDef = new TileDrawDefinition { TileType = TileID.ClosedDoor };
				}
			}

			return myTileDef;
		}
	}
}
