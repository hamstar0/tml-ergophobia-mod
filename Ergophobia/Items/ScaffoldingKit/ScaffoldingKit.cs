using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using Ergophobia.Protocols;


namespace Ergophobia.Items.ScaffoldingKit {
	public partial class ScaffoldingErectorKitItem : ModItem {
		public readonly static int ItemWidth = 24;
		public readonly static int ItemHeight = 22;
		public readonly static int ScaffoldWidth = 5;
		public readonly static int ScaffoldHeight = 6;


		////

		internal static Rectangle? ExpectedPlacementArea = null;

		internal static int PlacementVerticalOffset = 0;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Scaffolding Erector Kit" );
			this.Tooltip.SetDefault(
				"Attempts to erect a scaffold piece"
				+"\nFor use in making arenas, bridges, or aiding in house construction"
				+"\nRight-click to adjust placement height"
			);
		}

		public override void SetDefaults() {
			this.item.width = ScaffoldingErectorKitItem.ItemWidth;
			this.item.height = ScaffoldingErectorKitItem.ItemHeight;
			this.item.consumable = true;
			this.item.useStyle = ItemUseStyleID.HoldingUp;
			this.item.useTime = 30;
			this.item.useAnimation = 30;
			//this.item.UseSound = SoundID.Item108;
			this.item.maxStack = 30;
			this.item.value = ErgophobiaConfig.Instance.Get<int>( nameof(ErgophobiaConfig.ScaffoldingKitPrice) );
			this.item.rare = ItemRarityID.Green;
		}


		////////////////

		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			int offsetY = ScaffoldingErectorKitItem.PlacementVerticalOffset;

			tooltips.Add( new TooltipLine(this.mod, "ScaffoldingOffset", "Vertical position offset: "+offsetY) );
		}


		////////////////

		public override bool UseItem( Player player ) {
			if( player.itemAnimation > 0 && player.itemTime == 0 ) {
				player.itemTime = item.useTime;
			}

			return base.UseItem( player );
		}
		
		////

		public override bool ConsumeItem( Player player ) {
			int tileX = (int)player.Center.X / 16;
			int tileY = (int)player.position.Y / 16;
			int offsetY = ScaffoldingErectorKitItem.PlacementVerticalOffset;
			Rectangle area;

			bool canErect = ScaffoldingErectorKitItem.Validate(
				tileX: tileX,
				tileY: tileY,
				offsetY: offsetY, 
				area: out area
			);

			if( canErect ) {
				if( Main.netMode == NetmodeID.SinglePlayer ) {
					ScaffoldingErectorKitItem.MakeScaffold( area.Left, area.Bottom + offsetY );
				} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
					ScaffoldingKitProtocol.SendToServer( tileX, tileY, offsetY );
				} else if( Main.netMode == NetmodeID.Server ) {
					LogHelpers.Alert( "Server?" );
				}
			} else {
				Main.NewText( "Invalid location.", Color.Yellow );
			}

			return canErect;
		}
	}
}
