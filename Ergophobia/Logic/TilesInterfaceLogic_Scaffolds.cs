using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using Ergophobia.Items.ScaffoldingKit;


namespace Ergophobia.Logic {
	static partial class TilesInterfaceLogic {
		private static void DrawScaffoldPlacementGuidesIf() {
			if( !ScaffoldingErectorKitItem.ExpectedPlacementArea.HasValue ) {
				return;
			}

			Rectangle val = ScaffoldingErectorKitItem.ExpectedPlacementArea.Value;
			int maxY = ScaffoldingErectorKitItem.GetFurthestAllowedGroundTileY( val.Bottom );
			var scrArea = new Rectangle(
				x: (val.X * 16) - (int)Main.screenPosition.X,
				y: (maxY * 16) - (int)Main.screenPosition.Y - 2,
				width: val.Width * 16,
				height: 4
			);

			float pulse = (float)Main.mouseTextColor / 255f;
			var color = new Color( Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor );
			color *= pulse * pulse * pulse;

			Main.spriteBatch.Draw(
				texture: Main.magicPixel,
				destinationRectangle: scrArea,
				color: color
			);
		}
	}
}
