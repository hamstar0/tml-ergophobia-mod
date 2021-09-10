using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Services.AnimatedColor;
using Ergophobia.Logic;
using Ergophobia.Items.TrackDeploymentKit;


namespace Ergophobia {
	partial class ErgophobiaMod : Mod {
		public override void PostDrawInterface( SpriteBatch sb ) {
			Item heldItem = Main.LocalPlayer.HeldItem;
			if( heldItem == null || heldItem.IsAir ) { return; }

			if( heldItem.type == ModContent.ItemType<TrackDeploymentKitItem>() ) {
				var position = new Vector2( Main.mouseX, Main.mouseY );
				position.X -= 14;
				position.Y -= 32;
				string dirText = Main.LocalPlayer.direction > 0
					? " >"
					: "< ";

				sb.DrawString(
					Main.fontMouseText,
					dirText,
					position,
					AnimatedColors.Strobe.CurrentColor * 0.75f,
					0f,
					default( Vector2 ),
					2f,
					SpriteEffects.None,
					1f
				);
			}
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals("Vanilla: Inventory") );
			if( idx != -1 ) {
				this.AddTilePlacementInterfaceLayer( layers, idx + 1 );
			}
		}


		private void AddTilePlacementInterfaceLayer( List<GameInterfaceLayer> layers, int layerIdx ) {
			bool placementUI() {
				TilesInterfaceLogic.DrawCurrentTilePlacementOutline();
				TilesInterfaceLogic.DrawCurrentTilePlacementGuides();
				return true;
			};

			var tradeLayer = new LegacyGameInterfaceLayer(
				"Ergophobia: Placement Indicators",
				placementUI,
				InterfaceScaleType.Game
			);
			layers.Insert( layerIdx, tradeLayer );
		}
	}
}
