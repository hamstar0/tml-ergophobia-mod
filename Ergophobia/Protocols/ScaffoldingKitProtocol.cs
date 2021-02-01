using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using Ergophobia.Items;


namespace Ergophobia.Protocols {
	class ScaffoldingKitProtocol : PacketProtocolSendToServer {
		public static void SendToServer( int placeAtTileX, int placeAtTileY ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) { throw new ModHelpersException( "Not client" ); }

			var protocol = new ScaffoldingKitProtocol(  placeAtTileX, placeAtTileY );
			protocol.SendToServer( false );
		}



		////////////////

		public int PlaceAtTileX;
		public int PlaceAtTileY;



		////////////////

		private ScaffoldingKitProtocol() { }

		private ScaffoldingKitProtocol( int leftTileX, int floorTileY ) {
			this.PlaceAtTileX = leftTileX;
			this.PlaceAtTileY = floorTileY;
		}

		protected override void InitializeClientSendData() {
		}

		////

		protected override void Receive( int fromWho ) {
			Rectangle area;
			bool isValid = ScaffoldingErectorKitItem.Validate( this.PlaceAtTileX, this.PlaceAtTileY, out area );
			
			if( isValid ) {
				ScaffoldingErectorKitItem.MakeScaffold( area.Left, area.Bottom );
			} else {
				LogHelpers.Alert( "Could not place house frame" );
			}
		}
	}
}
