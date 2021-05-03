using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using Ergophobia.Items.TrackDeploymentKit;
using Ergophobia.Network;


namespace Ergophobia {
	partial class ErgophobiaPlayer : ModPlayer {
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
