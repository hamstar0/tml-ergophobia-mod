using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;


namespace Ergophobia.Network {
	class TileRectangleModPacket {
		public static void Send( Rectangle area ) {
			ModPacket packet = ErgophobiaMod.Instance.GetPacket();

			packet.Write( (int)area.X );
			packet.Write( (int)area.Y );
			packet.Write( (ushort)area.Width );
			packet.Write( (ushort)area.Height );
			
			for( int i=area.Left; i<area.Right; i++ ) {
				for( int j=area.Top; j<area.Bottom; j++ ) {
					TileHelpers.ToStream( packet, Main.tile[i, j], true, true, false );
				}
			}

			packet.Send();
		}

		public static void Receive( BinaryReader reader ) {
			int x = reader.ReadInt32();
			int y = reader.ReadInt32();
			int width = reader.ReadUInt16();
			int height = reader.ReadUInt16();
			var area = new Rectangle( x, y, width, height );

			for( int i=area.Left; i<area.Right; i++ ) {
				for( int j=area.Top; j<area.Bottom; j++ ) {
					TileHelpers.FromStream( reader, ref Main.tile[i, j], true, true, false );
				}
			}
		}
	}
}
