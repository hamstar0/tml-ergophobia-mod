using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using Ergophobia.Logic;
using Ergophobia.Tiles;


namespace Ergophobia {
	partial class AMTile : GlobalTile {
		public override bool CanPlace( int i, int j, int type ) {
			if( Main.netMode != NetmodeID.Server && !Main.dedServ ) {
				// World gen?
				if( Main.gameMenu || !LoadHelpers.IsCurrentPlayerInGame() ) {
					return true;
				}
			}

			if( !TileLogic.CanPlace(type) ) {
				return false;
			}

			switch( type ) {
			case TileID.Platforms:
				return TileLogic.CanPlacePlatform( i, j );
			case TileID.Rope:
			case TileID.SilkRope:
			case TileID.VineRope:
			case TileID.WebRope:
				return TileLogic.CanPlaceRope( i, j );
			case TileID.MinecartTrack:
				return TileLogic.CanPlaceTrack( i, j );
			default:
				if( type == ModContent.TileType<FramingPlankTile>() ) {
					return TileLogic.CanPlaceFramingPlank( i, j );
				}
				return true;
			}
		}
	}
}