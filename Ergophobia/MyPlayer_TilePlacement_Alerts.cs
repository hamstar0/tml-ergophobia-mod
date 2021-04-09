using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using Ergophobia.Items.HouseFramingKit;
using Ergophobia.Items.HouseFurnishingKit;
using Ergophobia.Items.ScaffoldingKit;


namespace Ergophobia {
	partial class ErgophobiaPlayer : ModPlayer {
		private bool _HasPreviousHouseViableAlert = false;
		private bool _FurnishingTipShown = false;



		////////////////

		private void CheckFurnishableHouse() {
			(int x, int y) pos = (this.CurrentHouseChunkX, this.CurrentHouseChunkY);
			if( !this.ChartedHouseSpaces.Contains(pos) ) {
				this.ChartedHouseSpaces.Add( pos );
			} else {
				return;
			}

			int tileX = (int)this.player.Center.X / 16;
			int tileY = (int)this.player.Center.Y / 16;
			HouseViabilityState state = HouseFurnishingKitItem.IsValidHouse( tileX, tileY );

			if( state == HouseViabilityState.Good ) {
				if( !this._HasPreviousHouseViableAlert ) {
					this._HasPreviousHouseViableAlert = true;

					Color color;
					string msg = HouseFurnishingKitItem.GetViabilityStateMessage(
						state: HouseViabilityState.Good,
						fullSpace: 0,
						innerSpace: 0,
						verbose: !this._FurnishingTipShown,
						color: out color
					);

					this._FurnishingTipShown = true;

					foreach( string subMsg in msg.Split('\n') ) {
						Main.NewText( subMsg, color );
					}
				}
			} else {
				this._HasPreviousHouseViableAlert = false;
			}
		}


		private void CheckFrameableHouse() {
			int tileX = (int)this.player.Center.X >> 4;
			int tileY = (int)this.player.position.Y >> 4;

			ISet<(int, int)> tiles;
			bool canErect = HouseFramingKitItem.Validate( ref tileX, ref tileY, out tiles );

			Color color;
			Vector2 pos;
			foreach( (int x, int y) in tiles ) {
				pos = new Vector2( (x<<4) + 8, (y<<4) + 8 );
				color = canErect ? Color.Lime : Color.Red;

				Dust dust = Dust.NewDustPerfect(
					Position: pos,
					Velocity: default(Vector2),
					Type: 59,
					Alpha: 0,
					newColor: color,
					Scale: 1.25f
				);
				dust.noGravity = true;
				dust.noLight = true;
			}
		}


		private void CheckScaffoldArea() {
			int tileX = (int)this.player.Center.X / 16;
			int tileY = (int)this.player.position.Y / 16;

			Rectangle area;
			bool canErect = ScaffoldingErectorKitItem.Validate(
				tileX: tileX,
				tileY: tileY,
				offsetY: ScaffoldingErectorKitItem.PlacementVerticalOffset,
				area: out area
			);

			ScaffoldingErectorKitItem.ExpectedPlacementArea = area;

			for( int x=area.Left; x<area.Right; x++ ) {
				for( int y=area.Top; y<area.Bottom; y++ ) {
					Dust dust = Dust.NewDustPerfect(
						Position: new Vector2( (x*16) + 8, (y*16) + 8 ),
						Velocity: default( Vector2 ),
						Type: 59,
						Alpha: 0,
						newColor: canErect ? Color.Lime : Color.Red,
						Scale: 1.25f
					);
					dust.noGravity = true;
					dust.noLight = true;
				}
			}
		}
	}
}
