using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;
using ModLibsCore.Services.Timers;
using Ergophobia.Logic;
using Ergophobia.Tiles;


namespace Ergophobia {
	partial class ErgophobiaTile : GlobalTile {
		private static bool IsAlertingToPlacementFail = false;



		////////////////

		public override bool CanPlace( int i, int j, int type ) {
			if( Main.netMode != NetmodeID.Server && !Main.dedServ ) {
				// World gen?
				if( Main.gameMenu || !LoadLibraries.IsCurrentPlayerInGame() ) {
					return true;
				}
			}

			// General checks:
			if( !TileLogic.CanPlace(type) ) {
				return false;
			}

			bool canPlace = true;
			string alertIfFail = "";

			// Situational checks:
			switch( type ) {
			case TileID.Platforms:
				canPlace = TileLogic.CanPlacePlatform( i, j );
				alertIfFail = "Must connect in a straight line to solid nearby ground";
				break;
			case TileID.Rope:
			case TileID.SilkRope:
			case TileID.VineRope:
			case TileID.WebRope:
				canPlace = TileLogic.CanPlaceRope( i, j );
				alertIfFail = "Ropes do not levitate";
				break;
			case TileID.MinecartTrack:
				canPlace = TileLogic.CanPlaceTrack( i, j );
				alertIfFail = "Must be placed on ground or else descendingly from other tracks";
				break;
			default:
				if( type == ModContent.TileType<FramingPlankTile>() ) {
					canPlace = TileLogic.CanPlaceFramingPlank( i, j );
					alertIfFail = "Must connect in a straight line to solid nearby ground";
				}
				break;
			}

			//

			if( !canPlace ) {
				if( !ErgophobiaTile.IsAlertingToPlacementFail ) {
					ErgophobiaTile.IsAlertingToPlacementFail = true;

					Main.NewText( alertIfFail, Color.Yellow );
				}

				Timers.SetTimer( "ErgophobiaPlankFailAlert", 3, false, () => {
					ErgophobiaTile.IsAlertingToPlacementFail = false;
					return false;
				} );
			}

			return canPlace;
		}
	}
}