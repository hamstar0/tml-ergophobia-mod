using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using Ergophobia.Items.ScaffoldingKit;


namespace Ergophobia.Protocols {
	class ScaffoldingKitProtocol : PacketProtocolSendToServer {
		public static void SendToServer( int placeAtTileX, int placeAtTileY, int offsetTileY ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModHelpersException( "Not client" ); }

			var protocol = new ScaffoldingKitProtocol(  placeAtTileX, placeAtTileY, offsetTileY );
			protocol.SendToServer( false );
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

		protected override void InitializeClientSendData() {
		}

		////

		protected override void Receive( int fromWho ) {
			Rectangle area;
			bool isValid = ScaffoldingErectorKitItem.Validate(
				tileX: this.PlaceAtTileX,
				tileY: this.PlaceAtTileY,
				offsetY: this.OffsetTileY,
				area: out area
			);
			
			if( isValid ) {
				ScaffoldingErectorKitItem.MakeScaffold( area.Left, area.Bottom );
			} else {
				LogHelpers.Alert( "Could not place house frame" );
			}
		}
	}
}
