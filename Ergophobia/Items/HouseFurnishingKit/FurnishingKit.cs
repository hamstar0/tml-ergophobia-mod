using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using Ergophobia.Network;


namespace Ergophobia.Items.HouseFurnishingKit {
	public partial class HouseFurnishingKitItem : ModItem {
		public static int Width = 36;
		public static int Height = 28;



		////////////////

		public static bool FurnishHouseFull(
					Player player,
					int tileX,
					int tileY,
					ISet<(ushort TileX, ushort TileY)> innerHouseSpace,
					ISet<(ushort TileX, ushort TileY)> fullHouseSpace,
					int floorX,
					int floorY ) {
			foreach( HouseFurnishingKitItem.PreFurnishHouse func in ErgophobiaMod.Instance.OnPreHouseFurnish ) {
				if( !func( tileX, tileY ) ) {
					return false;
				}
			}

			HouseFurnishingKitItem.FurnishHouse( player, innerHouseSpace, fullHouseSpace, floorX, floorY, (p1, p2, p3, p4, p5, p6, p7, p8, p9) => {
				foreach( OnFurnishHouse action in ErgophobiaMod.Instance.OnPostHouseFurnish ) {
					action( p1, p2, p3, p4, p5, p6, p7, p8, p9 );
				}
			} );

			return true;
		}



		////////////////

		public override void SetStaticDefaults() {
			string tooltip = "Attempts to transform a given space into a spawn point and NPC living area"
				+ "\nRequires a closed, minimally-sized, unobstructed area"
				+ "\nTip: Plug gaps with platforms or framing planks to make an area furnishable"
				+ "\nWarning: This will remove ALL objects within the given area";

			this.DisplayName.SetDefault( "House Furnishing Kit" );
			this.Tooltip.SetDefault( tooltip );
		}

		public override void SetDefaults() {
			this.item.width = HouseFurnishingKitItem.Width;
			this.item.height = HouseFurnishingKitItem.Height;
			this.item.consumable = true;
			this.item.useStyle = ItemUseStyleID.HoldingUp;
			this.item.useTime = 30;
			this.item.useAnimation = 30;
			//this.item.UseSound = SoundID.Item108;
			this.item.maxStack = 30;
			this.item.value = ErgophobiaConfig.Instance.Get<int>( nameof(ErgophobiaConfig.HouseFurnishingKitPrice) );
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
			int tileY = (int)player.Center.Y >> 4;
			ISet<(ushort TileX, ushort TileY)> innerHouseSpace, fullHouseSpace;
			int floorX, floorY;
			
			HouseViabilityState state = HouseFurnishingKitItem.IsValidHouse(
				tileX,
				tileY,
				out innerHouseSpace,
				out fullHouseSpace,
				out floorX,
				out floorY
			);

			if( state == HouseViabilityState.Good ) {
				if( Main.netMode == NetmodeID.SinglePlayer ) {
					return HouseFurnishingKitItem.FurnishHouseFull(
						player,
						tileX,
						tileY,
						innerHouseSpace,
						fullHouseSpace,
						floorX,
						floorY
					);
				} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
					FurnishingKitProtocol.SendToServer( player, tileX, tileY );
					return true;
				} else if( Main.netMode == NetmodeID.Server ) {
					LogLibraries.Alert( "Server?" );
				}
			} else {
				Color color;
				String msg = HouseFurnishingKitItem.GetViabilityStateMessage(
					state,
					fullHouseSpace.Count,
					innerHouseSpace.Count,
					false,
					out color
				);

				foreach( string subMsg in msg.Split('\n') ) {
					Main.NewText( subMsg, color );
				}
			}

			return false;
		}
	}
}
