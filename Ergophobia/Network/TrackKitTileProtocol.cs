using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using HamstarHelpers.Helpers.Debug;
using Ergophobia.Items.TrackDeploymentKit;


namespace Ergophobia.Network {
	class TrackKitTileProtocol : PacketProtocolSendToClient {
		public static void SendToClients( int tileX, int tileY ) {
			if( Main.netMode != NetmodeID.Server ) { throw new ModHelpersException( "Not server" ); }

			var protocol = new TrackKitTileProtocol( tileX, tileY );

			protocol.SendToClient( -1, -1 );
		}



		////////////////

		public int TileX;
		public int TileY;



		////////////////

		private TrackKitTileProtocol() { }

		private TrackKitTileProtocol( int tileX, int tileY ) {
			this.TileX = tileX;
			this.TileY = tileY;
		}

		protected override void InitializeServerSendData( int toWho ) { }


		////////////////

		protected override void Receive() {
			TrackDeploymentKitItem.PlaceTrack( this.TileX, this.TileY );
		}
	}
}
