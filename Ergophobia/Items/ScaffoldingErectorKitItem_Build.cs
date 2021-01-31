using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Tiles.Draw;


namespace Ergophobia.Items {
	public partial class ScaffoldingErectorKitItem : ModItem {
		public static void MakeScaffold( int leftTileX, int floorTileY ) {
			int width = ScaffoldingErectorKitItem.ScaffoldWidth;
			int height = ScaffoldingErectorKitItem.ScaffoldHeight;
			var rect = new Rectangle(
				leftTileX - (width / 2),
				floorTileY - height,
				width,
				height
			);
			var hollow = new Rectangle(
				rect.X + 1,
				rect.Y + 1,
				width - 2,
				height - 1
			);

			var postTileDef = new TileDrawDefinition { WallType = WallID.RichMahoganyFence };
			var platTileDef = new TileDrawDefinition { TileType = TileID.Platforms };

			//

			TileDrawPrimitivesHelpers.DrawRectangle(
				filter: TilePattern.NonActive,
				area: rect,
				hollow: hollow,
				place: ( x, y ) => postTileDef
			);
			TileDrawPrimitivesHelpers.DrawRectangle(
				filter: TilePattern.NonActive,
				area: new Rectangle( rect.X, rect.Y, width, 1 ),
				hollow: null,
				place: ( x, y ) => platTileDef
			);

			//

			if( Main.netMode == NetmodeID.Server ) {
				NetMessage.SendTileRange(
					whoAmi: -1,
					tileX: rect.X,
					tileY: rect.Y ,
					xSize: rect.Width,
					ySize: rect.Height
				);
			}
		}
	}
}
