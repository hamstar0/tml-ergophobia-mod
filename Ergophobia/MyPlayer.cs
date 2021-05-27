using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using Ergophobia.Items.HouseFramingKit;
using Ergophobia.Items.HouseFurnishingKit;
using Ergophobia.Items.ScaffoldingKit;
using Ergophobia.Items.TrackDeploymentKit;


namespace Ergophobia {
	/*class ErgophobiaCustomPlayer : CustomPlayerData {
		protected override void OnEnter( object data ) {
			if( Main.netMode == 1 ) {
				CustomFurnitureProtocol.QuickRequest();
			}
		}
	}*/




	partial class ErgophobiaPlayer : ModPlayer {
		private int CurrentHouseChunkX;
		private int CurrentHouseChunkY;

		private ISet<(int, int)> ChartedHouseSpaces = new HashSet<(int, int)>();


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void PreUpdate() {
			if( Main.netMode != NetmodeID.Server ) {
				if( Main.myPlayer == this.player.whoAmI ) {
					this.PreUpdateLocal();
				}
			}

			if( ErgophobiaConfig.Instance.DebugModeInfo ) {
				if( Main.mouseRight && Main.mouseRightRelease ) {
					Tile tile = Main.tile[
						(int)(Main.screenPosition.X + Main.mouseX) / 16,
						(int)(Main.screenPosition.Y + Main.mouseY) / 16
					];
					Main.NewText( tile.ToString() );
				}
			}
		}
		
		private void PreUpdateLocal() {
			if( this.UpdateHouseChunkCheckPosition() ) {
				int furnishKitItemType = ModContent.ItemType<HouseFurnishingKitItem>();

				if( this.player.inventory.Any( i => ((i?.active == true) && (i.type == furnishKitItemType)) ) ) {
					this.CheckFurnishableHouse();
				}
			}
			
			if( this.player.HeldItem?.active == true ) {
				int heldItemType = this.player.HeldItem.type;

				if( heldItemType == ModContent.ItemType<HouseFramingKitItem>() ) {
					this.CheckFrameableHouse();
				} else if( heldItemType == ModContent.ItemType<TrackDeploymentKitItem>() ) {
					this.CheckTrackKitResume( heldItemType );
				} else if( heldItemType == ModContent.ItemType<ScaffoldingErectorKitItem>() ) {
					this.CheckScaffoldArea();
				}
			}
		}

		////

		private bool UpdateHouseChunkCheckPosition() {
			int chunkX = (int)this.player.Center.X / 128;	// 8 tiles
			int chunkY = (int)this.player.Center.Y / 128;

			if( this.CurrentHouseChunkX == 0 ) {
				this.CurrentHouseChunkX = chunkX;
				this.CurrentHouseChunkY = chunkY;
				return true;
			} else {
				if( this.CurrentHouseChunkX != chunkX || this.CurrentHouseChunkY != chunkY ) {
					this.CurrentHouseChunkX = chunkX;
					this.CurrentHouseChunkY = chunkY;
					return true;
				}
			}
			return false;
		}
	}
}
