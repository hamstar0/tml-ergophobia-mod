using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using Ergophobia.Items;
using Ergophobia.Protocols;


namespace Ergophobia {
	partial class ErgophobiaPlayer : ModPlayer {
		private void CheckFurnishableHouse() {
			(int x, int y) pos = (this.CurrentHouseChunkX, this.CurrentHouseChunkY);
			if( !this.ChartedHouseSpaces.Contains(pos) ) {
				this.ChartedHouseSpaces.Add( pos );
			} else {
				return;
			}

			HouseViabilityState state = HouseFurnishingKitItem.IsValidHouse(
				(int)this.player.Center.X / 16,
				(int)this.player.Center.Y / 16
			);

			if( state == HouseViabilityState.Good ) {
				if( !this.HasPreviousHouseViableAlert ) {
					this.HasPreviousHouseViableAlert = true;

					Color color;
					string msg = HouseFurnishingKitItem.GetViabilityStateMessage( HouseViabilityState.Good, 0, 0, out color );

					Main.NewText( msg, color );
				}
			} else {
				this.HasPreviousHouseViableAlert = false;
			}
		}

		private void CheckFrameableHouse() {
			int tileX = (int)this.player.Center.X >> 4;
			int tileY = (int)this.player.position.Y >> 4;

			ISet<(int, int)> tiles;
			bool canErect = HouseFramingKitItem.Validate( ref tileX, ref tileY, out tiles );

			Color color;
			Vector2 pos;
			foreach( (int x, int y) in tiles ) {
				pos = new Vector2( (x<<4) + 8, (y<<4) + 8 );
				color = canErect ? Color.Lime : Color.Red;

				Dust dust = Dust.NewDustPerfect(
					Position: pos,
					Velocity: default(Vector2),
					Type: 59,
					Alpha: 0,
					newColor: color,
					Scale: 1.25f
				);
				dust.noGravity = true;
				dust.noLight = true;
			}
		}


		private void CheckScaffoldArea() {
			int tileX = (int)this.player.Center.X / 16;
			int tileY = (int)this.player.position.Y / 16;

			Rectangle area;
			bool canErect = ScaffoldingErectorKitItem.Validate( tileX, tileY, out area );
			
			for( int x=area.Left; x<area.Right; x++ ) {
				for( int y=area.Top; y<area.Bottom; y++ ) {
					Dust dust = Dust.NewDustPerfect(
						Position: new Vector2( (x*16) + 8, (y*16) + 8 ),
						Velocity: default( Vector2 ),
						Type: 59,
						Alpha: 0,
						newColor: canErect ? Color.Lime : Color.Red,
						Scale: 1.25f
					);
					dust.noGravity = true;
					dust.noLight = true;
				}
			}
		}


		private void CheckTrackKitResume( int heldTrackKitItemType ) {
			if( !this.player.mount.Active || !this.player.mount.Cart ) {
				return;
			}

			var trackKitSingleton = ModContent.GetInstance<TrackDeploymentKitItem>();
			(int x, int y, int dir) resume = trackKitSingleton.ResumeDeploymentAt;
			var resumeWldPos = new Vector2( (resume.x << 4) + 8, (resume.y << 4) + 8 );

			if( Vector2.DistanceSquared(this.player.Center, resumeWldPos) >= 4096 ) { // 4 tiles
				return;
			}

			PlayerItemHelpers.RemoveInventoryItemQuantity( this.player, heldTrackKitItemType, 1 );

			int leftovers = TrackDeploymentKitItem.Redeploy( this.player.whoAmI );
			if( leftovers == 0 ) {
				return;
			}

			int itemWho = Item.NewItem( resumeWldPos, ItemID.MinecartTrack, leftovers );

			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				NetMessage.SendData( MessageID.SyncItem, -1, -1, null, itemWho, 1f );
			} else {
				TrackKitDeployProtocol.SendToServer( resume.dir > 0, resume.x, resume.y, true );
			}
		}
	}
}
