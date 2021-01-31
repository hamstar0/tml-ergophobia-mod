using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using Ergophobia.Items;


namespace Ergophobia.Protocols {
	class ScaffoldingKitProtocol : PacketProtocolSendToServer {
		public static void SendToServer( int leftTileX, int floorTileY ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModHelpersException( "Not client" ); }

			var protocol = new ScaffoldingKitProtocol(  leftTileX, floorTileY );
			protocol.SendToServer( false );
		}



		////////////////

		public int LeftTileX;
		public int FloorTileY;



		////////////////

		private ScaffoldingKitProtocol() { }

		private ScaffoldingKitProtocol( int leftTileX, int floorTileY ) {
			this.LeftTileX = leftTileX;
			this.FloorTileY = floorTileY;
		}

		protected override void InitializeClientSendData() {
		}

		////

		protected override void Receive( int fromWho ) {
			bool isValid = ScaffoldingErectorKitItem.Validate( ref this.LeftTileX, ref this.FloorTileY, out _ );

			if( isValid ) {
				ScaffoldingErectorKitItem.MakeScaffold( this.LeftTileX, this.FloorTileY );
			} else {
				LogHelpers.Alert( "Could not place house frame" );
			}
		}
	}
}
