using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.Tiles.Draw;
using Ergophobia.Network;


namespace Ergophobia.Items.ScaffoldingKit {
	public partial class ScaffoldingErectorKitItem : ModItem {
		public static void MakeScaffold( int leftTileX, int floorTileY ) {
			int width = ScaffoldingErectorKitItem.ScaffoldWidth;
			int height = ScaffoldingErectorKitItem.ScaffoldHeight;
			var rect = new Rectangle(
				leftTileX,
				floorTileY - height,
				width,
				height
			);

			var postTileDef = new TileDrawDefinition {
				NotActive = true,
				WallType = WallID.RichMahoganyFence
			};
			var platTileDef = new TileDrawDefinition {
				SkipWall = true,
				TileType = TileID.Platforms
			};

			//
			
			int findFloor( int myTileX, int myTileY ) {
				int y;
				for( y = myTileY; !TileHelpers.IsSolid(Main.tile[myTileX, y], true, true); y++ ) {
					if( y >= Main.maxTilesY-1 ) {
						break;
					}
				}
				return y;
			}

			//

			int rightTileX = rect.X + rect.Width - 1;
			int lPostFloorY = findFloor( leftTileX, rect.Bottom );
			int rPostFloorY = findFloor( rightTileX, rect.Bottom );

			// Posts
			if( Main.tile[leftTileX-1, rect.Y].wall != WallID.RichMahoganyFence ) {
				TileDrawPrimitivesHelpers.DrawRectangle(
					filter: TilePattern.NonSolid,
					area: new Rectangle( leftTileX, rect.Y, 1, lPostFloorY - rect.Y ),
					hollow: null,
					place: ( x, y ) => postTileDef
				);
			}
			if( Main.tile[rightTileX + 1, rect.Y].wall != WallID.RichMahoganyFence ) {
				TileDrawPrimitivesHelpers.DrawRectangle(
					filter: TilePattern.NonSolid,
					area: new Rectangle( rightTileX, rect.Y, 1, rPostFloorY - rect.Y ),
					hollow: null,
					place: ( x, y ) => postTileDef
				);
			}

			// Platforms
			TileDrawPrimitivesHelpers.DrawRectangle(
				filter: TilePattern.NonSolid,
				area: new Rectangle( rect.X, rect.Y, width, 1 ),
				hollow: null,
				place: ( x, y ) => platTileDef
			);

			//

			Main.PlaySound( SoundID.Item69, rect.Center.ToVector2() * 16 );

			//

			if( Main.netMode == NetmodeID.Server ) {
LogHelpers.Log( "!!!MakeScaffold " + rect.ToString() );
				TileRectangleModPacket.Send( rect );
			}
		}
	}
}
