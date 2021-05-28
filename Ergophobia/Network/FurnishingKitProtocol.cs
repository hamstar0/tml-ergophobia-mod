using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Services.Network.SimplePacket;
using Ergophobia.Items.HouseFurnishingKit;


namespace Ergophobia.Network {
	[Serializable]
	class FurnishingKitProtocol : SimplePacketPayload {
		public static void SendToServer( Player player, int tileX, int tileY ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModLibsException( "Not client" ); }

			var packet = new FurnishingKitProtocol( player, tileX, tileY );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		public int PlayerWho;
		public int TileX;
		public int TileY;



		////////////////

		private FurnishingKitProtocol() { }

		private FurnishingKitProtocol( Player player, int tileX, int tileY ) {
			this.PlayerWho = player.whoAmI;
			this.TileX = tileX;
			this.TileY = tileY;
		}

		////

		public override void ReceiveOnServer( int fromWho ) {
			ISet<(ushort TileX, ushort TileY)> innerHouseSpace, fullHouseSpace;
			int floorX, floorY;

			HouseViabilityState state = HouseFurnishingKitItem.IsValidHouse(
				this.TileX,
				this.TileY,
				out innerHouseSpace,
				out fullHouseSpace,
				out floorX,
				out floorY
			);

			if( state == HouseViabilityState.Good ) {
				bool aborted = HouseFurnishingKitItem.FurnishHouseFull(
					Main.player[this.PlayerWho],
					this.TileX,
					this.TileY,
					innerHouseSpace,
					fullHouseSpace,
					floorX,
					floorY
				);
			} else {
				LogLibraries.Alert( "Could not furnish house" );
			}
		}

		public override void ReceiveOnClient() {
			throw new NotImplementedException();
		}
	}
}
