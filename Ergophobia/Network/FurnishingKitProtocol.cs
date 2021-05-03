using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using Ergophobia.Items.HouseFurnishingKit;


namespace Ergophobia.Network {
	class FurnishingKitProtocol : PacketProtocolSendToServer {
		public static void SendToServer( Player player, int tileX, int tileY ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModHelpersException( "Not client" ); }

			var protocol = new FurnishingKitProtocol( player, tileX, tileY );
			protocol.SendToServer( false );
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

		protected override void InitializeClientSendData() {
		}

		////

		protected override void Receive( int fromWho ) {
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
				LogHelpers.Alert( "Could not furnish house" );
			}
		}
	}
}
