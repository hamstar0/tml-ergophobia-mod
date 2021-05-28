using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using Ergophobia.Items.TrackDeploymentKit;


namespace Ergophobia.Network {
	[Serializable]
	class TrackKitResumeProtocol : SimplePacketPayload {
		public static void SendToClient( int fromPlayerWho, int tileX, int tileY, bool isAimedRight ) {
			if( Main.netMode != NetmodeID.Server ) { throw new ModLibsException( "Not server" ); }

			var packet = new TrackKitResumeProtocol( fromPlayerWho, tileX, tileY, isAimedRight );

			SimplePacket.SendToClient( packet, -1, -1 );
		}



		////////////////

		public int FromPlayerWho;
		public int TileX;
		public int TileY;
		public bool IsAimedRight;



		////////////////

		private TrackKitResumeProtocol() { }

		private TrackKitResumeProtocol( int fromPlayerWho, int tileX, int tileY, bool isAimedRight ) {
			this.FromPlayerWho = fromPlayerWho;
			this.TileX = tileX;
			this.TileY = tileY;
			this.IsAimedRight = isAimedRight;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException();
		}

		public override void ReceiveOnClient() {
			TrackDeploymentKitItem.PlaceResumePoint( this.TileX, this.TileY, this.IsAimedRight );
		}
	}
}
