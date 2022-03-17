using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Services.Network.SimplePacket;
using Ergophobia.Items.HouseFramingKit;


namespace Ergophobia.Network {
	[Serializable]
	class FramingKitProtocol : SimplePacketPayload {
		public static void SendToServer( int tileX, int tileY ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModLibsException( "Not client" ); }

			var packet = new FramingKitProtocol(  tileX, tileY );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		public int TileX;
		public int TileY;



		////////////////

		private FramingKitProtocol() { }

		private FramingKitProtocol( int tileX, int tileY ) {
			this.TileX = tileX;
			this.TileY = tileY;
		}

		////

		public override void ReceiveOnServer( int fromWho ) {
			bool isValid = HouseFramingKitItem.Validate(
				ref this.TileX,
				ref this.TileY,
				out _,
				out _,
				out string result
			);

			if( isValid ) {
				HouseFramingKitItem.MakeHouseFrame( this.TileX, this.TileY );
			} else {
				LogLibraries.Alert( "Could not place house frame: "+result );
			}
		}

		public override void ReceiveOnClient() {
			throw new NotImplementedException();
		}
	}
}
