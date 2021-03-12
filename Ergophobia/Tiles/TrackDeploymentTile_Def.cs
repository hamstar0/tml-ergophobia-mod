using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using HamstarHelpers.Helpers.Debug;
using Ergophobia.Protocols;


namespace Ergophobia.Tiles {
	public partial class TrackDeploymentTile : ModTile {
		public override void SetDefaults() {
			//var flags = AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile | AnchorType.SolidWithTop;

			Main.tileFrameImportant[this.Type] = true;

			TileObjectData.newTile.CopyFrom( TileObjectData.Style1x1 );
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			//
			TileObjectData.newAlternate.CopyFrom( TileObjectData.Style1x1 );
			TileObjectData.newAlternate.StyleHorizontal = true;
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorWall = false;
			TileObjectData.newAlternate.UsesCustomCanPlace = true;
			TileObjectData.addAlternate( 1 );
			/*TileObjectData.newTile.AnchorBottom = new AnchorData( flags, TileObjectData.newTile.Width, 0 );
			TileObjectData.newTile.AnchorAlternateTiles = new[] { (int)TileID.MinecartTrack };
			//
			TileObjectData.newAlternate.CopyFrom( TileObjectData.Style1x1 );
			TileObjectData.newAlternate.StyleHorizontal = true;
			TileObjectData.newAlternate.AnchorLeft = new AnchorData( flags, TileObjectData.newTile.Height, 0 );
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.MinecartTrack };
			TileObjectData.addAlternate( 1 );
			//
			TileObjectData.newAlternate.CopyFrom( TileObjectData.Style1x1 );
			TileObjectData.newAlternate.StyleHorizontal = true;
			TileObjectData.newAlternate.AnchorRight = new AnchorData( flags, TileObjectData.newTile.Height, 0 );
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.MinecartTrack };
			TileObjectData.addAlternate( 2 );
			//
			TileObjectData.newAlternate.CopyFrom( TileObjectData.Style1x1 );
			TileObjectData.newAlternate.StyleHorizontal = true;
			TileObjectData.newAlternate.AnchorTop = new AnchorData( flags, TileObjectData.newTile.Height, 0 );
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.MinecartTrack };
			TileObjectData.addAlternate( 3 );
			//
			TileObjectData.newAlternate.CopyFrom( TileObjectData.Style1x1 );
			TileObjectData.newAlternate.StyleHorizontal = true;
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.MinecartTrack };
			TileObjectData.addAlternate( 0 );*/
			//
			TileObjectData.addTile( this.Type );
		}
	}
}
