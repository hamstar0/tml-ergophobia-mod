using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using Ergophobia.Network;
using Ergophobia.Items.TrackDeploymentKit;


namespace Ergophobia.Tiles {
	public partial class TrackDeploymentTile : ModTile {
		public static void DeployAt( int i, int j, bool isFacingRight, int fromPlayerWho ) {
			Main.tile[i, j].ClearTile();

			if( Main.netMode == NetmodeID.Server ) {
				NetMessage.SendTileSquare( -1, i, j, 1 );
			}

			int leftovers = TrackDeploymentKitItem.Deploy( fromPlayerWho, i, j, isFacingRight );
			TrackDeploymentKitItem.DropLeftovers( leftovers, i, j );
		}


		////////////////
		
		public static bool HasAnchor( int i, int j ) {
			// Let's just not anchor anything this close to the edge
			if( i <= 1 || i >= (Main.maxTilesX - 2) ) {
				return false;
			}
			if( j <= 1 || j >= (Main.maxTilesY - 2) ) {
				return false;
			}

			if( Main.tile[i - 1, j - 1]?.active() == true ) {   // upper left
				return true;
			}
			if( Main.tile[i - 1, j]?.active() == true ) {   // left
				return true;
			}
			if( Main.tile[i - 1, j + 1]?.active() == true ) {   // lower left
				return true;
			}

			if( Main.tile[i, j - 1]?.active() == true ) {   // above
				return true;
			}
			if( Main.tile[i, j + 1]?.active() == true ) {   // below
				return true;
			}

			if( Main.tile[i + 1, j - 1]?.active() == true ) {   // upper right
				return true;
			}
			if( Main.tile[i + 1, j]?.active() == true ) {   // right
				return true;
			}
			if( Main.tile[i + 1, j + 1]?.active() == true ) {   // lower right
				return true;
			}
			return false;
		}



		////////////////

		public override bool CanPlace( int i, int j ) {
			if( !TrackDeploymentTile.HasAnchor(i, j) ) {
				return false;
			}

			if( (Main.tile[i, j]?.liquid ?? 0) > 0 ) {
				return false;
			}

			if( Main.netMode != NetmodeID.Server ) {
				bool isFacingRight = Main.LocalPlayer.direction == 1;

				if( !isFacingRight ) {
					if( Main.tile[i - 1, j]?.active() == true ) {
						return false;
					}
					if( Main.tile[i - 2, j]?.active() == true ) {
						return false;
					}
				} else {
					if( Main.tile[i + 1, j]?.active() == true ) {
						return false;
					}
					if( Main.tile[i + 2, j]?.active() == true ) {
						return false;
					}
				}
			}

			return true;
		}


		public override void PlaceInWorld( int i, int j, Item item ) {
			bool isFacingRight = Main.LocalPlayer.direction == 1;

			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				TrackKitDeployProtocol.SendToServer( isFacingRight, i, j, false );
			} else if( Main.netMode == NetmodeID.SinglePlayer ) {
				TrackDeploymentTile.DeployAt( i, j, isFacingRight, Main.myPlayer );
			}
		}
	}
}
