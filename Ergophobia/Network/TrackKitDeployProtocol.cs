using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using Ergophobia.Tiles;


namespace Ergophobia.Network {
	[Serializable]
	class TrackKitDeployProtocol : SimplePacketPayload {
		public static void SendToServer( bool isAimedRight, int tileX, int tileY, bool isRedeploy ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModLibsException( "Not client" ); }

			var packet = new TrackKitDeployProtocol( isAimedRight, tileX, tileY, isRedeploy );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		public bool IsAimedRight;
		public int TileX;
		public int TileY;
		public bool IsRedeploy;



		////////////////

		private TrackKitDeployProtocol() { }

		private TrackKitDeployProtocol( bool isAimedRight, int tileX, int tileY, bool isRedeploy ) {
			this.IsAimedRight = isAimedRight;
			this.TileX = tileX;
			this.TileY = tileY;
			this.IsRedeploy = isRedeploy;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			TrackDeploymentTile.DeployAt( this.TileX, this.TileY, this.IsAimedRight, fromWho );
		}

		public override void ReceiveOnClient() {
			throw new NotImplementedException();
		}
	}
}
