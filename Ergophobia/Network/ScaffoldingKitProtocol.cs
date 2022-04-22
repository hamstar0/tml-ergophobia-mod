using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using Ergophobia.Items.ScaffoldingKit;


namespace Ergophobia.Network {
	[Serializable]
	class ScaffoldingKitProtocol : SimplePacketPayload {
		public static void BroadcastFromClientToEveryone( int placeAtTileX, int placeAtTileY, int offsetTileY ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Not client" );
			}

			//

			var packet = new ScaffoldingKitProtocol(  placeAtTileX, placeAtTileY, offsetTileY );

			SimplePacket.SendToServer( packet );
		}



		////////////////

		public int PlaceAtTileX;
		public int PlaceAtTileY;
		public int OffsetTileY;



		////////////////

		private ScaffoldingKitProtocol() { }

		private ScaffoldingKitProtocol( int leftTileX, int floorTileY, int offsetTileY ) {
			this.PlaceAtTileX = leftTileX;
			this.PlaceAtTileY = floorTileY;
			this.OffsetTileY = offsetTileY;
		}

		////

		public override void ReceiveOnServer( int fromWho ) {
			Rectangle area;
			bool isValid = ScaffoldingErectorKitItem.Validate(
				tileX: this.PlaceAtTileX,
				tileY: this.PlaceAtTileY,
				offsetY: this.OffsetTileY,
				area: out area
			);

			if( isValid ) {
				ScaffoldingErectorKitItem.MakeScaffold( area.Left, area.Bottom + this.OffsetTileY );
			} else {
				LogLibraries.Alert( "Could not place house frame" );
			}
		}

		public override void ReceiveOnClient() {
			throw new NotImplementedException();
		}
	}
}
