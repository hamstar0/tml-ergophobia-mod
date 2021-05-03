using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using Ergophobia.Network;


namespace Ergophobia.Items.HouseFramingKit {
	public partial class HouseFramingKitItem : ModItem {
		public readonly static int ItemWidth = 24;
		public readonly static int ItemHeight = 22;

		public readonly static int FrameWidth = 16;
		public readonly static int FrameHeight = 8;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "House Framing Kit" );
			this.Tooltip.SetDefault(
				"Attempts to erect a skeletal house frame"
				+"\nFor use when no existing house structure is available"
			);
		}

		public override void SetDefaults() {
			this.item.width = HouseFramingKitItem.ItemWidth;
			this.item.height = HouseFramingKitItem.ItemHeight;
			this.item.consumable = true;
			this.item.useStyle = ItemUseStyleID.HoldingUp;
			this.item.useTime = 30;
			this.item.useAnimation = 30;
			//this.item.UseSound = SoundID.Item108;
			this.item.maxStack = 30;
			this.item.value = ErgophobiaConfig.Instance.Get<int>( nameof(ErgophobiaConfig.HouseFramingKitPrice) );
			this.item.rare = ItemRarityID.Green;
		}


		////////////////

		public override bool UseItem( Player player ) {
			if( player.itemAnimation > 0 && player.itemTime == 0 ) {
				player.itemTime = item.useTime;
				return true;
			}
			return base.UseItem( player );
		}

		public override bool ConsumeItem( Player player ) {
			int tileX = (int)player.Center.X >> 4;
			int tileY = (int)player.position.Y >> 4;

			ISet<(int, int)> _;
			bool canErect = HouseFramingKitItem.Validate( ref tileX, ref tileY, out _ );

			if( canErect ) {
				if( Main.netMode == NetmodeID.SinglePlayer ) {
					HouseFramingKitItem.MakeHouseFrame( tileX, tileY );
				} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
					FramingKitProtocol.SendToServer( tileX, tileY );
					return true;
				} else if( Main.netMode == NetmodeID.Server ) {
					LogHelpers.Alert( "Server?" );
				}
			} else {
				Main.NewText( "Not enough open space.", Color.Yellow );
			}

			return canErect;
		}
	}
}
