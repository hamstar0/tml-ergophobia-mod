using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using Ergophobia.Items.TrackDeploymentKit;


namespace Ergophobia.Network {
	[Serializable]
	class TrackKitTileProtocol : SimplePacketPayload {
		public static void SendToClients( int tileX, int tileY ) {
			if( Main.netMode != NetmodeID.Server ) { throw new ModLibsException( "Not server" ); }

			var packet = new TrackKitTileProtocol( tileX, tileY );

			SimplePacket.SendToClient( packet, - 1, -1 );
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


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException();
		}

		public override void ReceiveOnClient() {
			TrackDeploymentKitItem.PlaceTrack( this.TileX, this.TileY );
		}
	}
}
