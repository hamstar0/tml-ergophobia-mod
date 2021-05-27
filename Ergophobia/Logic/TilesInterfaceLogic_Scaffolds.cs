using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;
using Ergophobia.Items.ScaffoldingKit;


namespace Ergophobia.Logic {
	static partial class TilesInterfaceLogic {
		private static bool WasRightClickWhileScaffoldPlacing = false;



		////////////////

		private static void DrawScaffoldPlacementGuidesIf() {
			if( !ScaffoldingErectorKitItem.ExpectedPlacementArea.HasValue ) {
				return;
			}

			if( Main.mouseRight ) {
				if( !TilesInterfaceLogic.WasRightClickWhileScaffoldPlacing ) {
					TilesInterfaceLogic.WasRightClickWhileScaffoldPlacing = true;

					ScaffoldingErectorKitItem.PlacementVerticalOffset += 1;
					ScaffoldingErectorKitItem.PlacementVerticalOffset %= 4;
				}
			} else {
				TilesInterfaceLogic.WasRightClickWhileScaffoldPlacing = false;
			}

			Rectangle val = ScaffoldingErectorKitItem.ExpectedPlacementArea.Value;
			int maxY = ScaffoldingErectorKitItem.GetFurthestAllowedGroundTileY( val.Bottom );
			var scrArea = new Rectangle(
				x: (val.X * 16) - (int)Main.screenPosition.X - 2,
				y: (maxY * 16) - (int)Main.screenPosition.Y - 2,
				width: (val.Width * 16) + 4,
				height: 4
			);

			float pulse = (float)Main.mouseTextColor / 255f;
			var color = new Color( pulse, pulse, pulse, pulse );
			color *= pulse * pulse * pulse;

			Main.spriteBatch.Draw(
				texture: Main.magicPixel,
				destinationRectangle: scrArea,
				color: color
			);
		}
	}
}
