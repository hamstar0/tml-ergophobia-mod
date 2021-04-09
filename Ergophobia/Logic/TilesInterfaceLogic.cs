using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Draw;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.AnimatedColor;
using Ergophobia.Items.FramingPlank;


namespace Ergophobia.Logic {
	static partial class TilesInterfaceLogic {
		private static (int X, int Y) PlacementOutlineTile = (0, 0);
		private static int PlacementOutlineLinger = 0;



		////////////////

		public static void DrawCurrentTilePlacementOutline() {
			int mouseWorldX = (int)Main.screenPosition.X + Main.mouseX;
			int mouseWorldY = (int)Main.screenPosition.Y + Main.mouseY;

			if( !PlayerInteractionHelpers.IsWithinTilePlacementReach( mouseWorldX, mouseWorldY ) ) {
				return;
			}

			int mouseTileX = mouseWorldX >> 4;
			int mouseTileY = mouseWorldY >> 4;
			float outlineIntensity = 1f;

			if( TilesInterfaceLogic.PlacementOutlineTile.X == mouseTileX && TilesInterfaceLogic.PlacementOutlineTile.Y == mouseTileY ) {
				outlineIntensity = 1f - ( (float)TilesInterfaceLogic.PlacementOutlineLinger / 60f );
				if( outlineIntensity < 0.1f ) {
					outlineIntensity = 0.1f;
				}
				TilesInterfaceLogic.PlacementOutlineLinger++;
			} else {
				TilesInterfaceLogic.PlacementOutlineTile = (mouseTileX, mouseTileY);
				TilesInterfaceLogic.PlacementOutlineLinger = 0;
			}

			Item heldItem = Main.LocalPlayer.HeldItem;

			switch( heldItem.type ) {
			case ItemID.BlueBrickPlatform:
			case ItemID.BonePlatform:
			case ItemID.BorealWoodPlatform:
			case ItemID.CactusPlatform:
			case ItemID.CrystalPlatform:
			case ItemID.DynastyPlatform:
			case ItemID.EbonwoodPlatform:
			case ItemID.FleshPlatform:
			case ItemID.FrozenPlatform:
			case ItemID.GlassPlatform:
			case ItemID.GoldenPlatform:
			case ItemID.GranitePlatform:
			case ItemID.GreenBrickPlatform:
			case ItemID.HoneyPlatform:
			case ItemID.LihzahrdPlatform:
			case ItemID.LivingWoodPlatform:
			case ItemID.MarblePlatform:
			case ItemID.MartianPlatform:
			case ItemID.MeteoritePlatform:
			case ItemID.MushroomPlatform:
			case ItemID.ObsidianPlatform:
			case ItemID.PalmWoodPlatform:
			case ItemID.PearlwoodPlatform:
			case ItemID.PinkBrickPlatform:
			case ItemID.PumpkinPlatform:
			case ItemID.RichMahoganyPlatform:
			case ItemID.ShadewoodPlatform:
			case ItemID.SkywarePlatform:
			case ItemID.SlimePlatform:
			case ItemID.SpookyPlatform:
			case ItemID.SteampunkPlatform:
			case ItemID.TeamBlockBluePlatform:
			case ItemID.TeamBlockGreenPlatform:
			case ItemID.TeamBlockPinkPlatform:
			case ItemID.TeamBlockRedPlatform:
			case ItemID.TeamBlockWhitePlatform:
			case ItemID.TeamBlockYellowPlatform:
			case ItemID.WoodPlatform:
			case ItemID.BlinkrootPlanterBox:
			case ItemID.CorruptPlanterBox:
			case ItemID.CrimsonPlanterBox:
			case ItemID.DayBloomPlanterBox:
			case ItemID.FireBlossomPlanterBox:
			case ItemID.MoonglowPlanterBox:
			case ItemID.ShiverthornPlanterBox:
			case ItemID.WaterleafPlanterBox:
				TilesInterfaceLogic.DrawPlatformTilePlacementOutline( outlineIntensity );
				break;
			case ItemID.Rope:
			case ItemID.SilkRope:
			case ItemID.VineRope:
			case ItemID.WebRope:
				TilesInterfaceLogic.DrawRopeTilePlacementOutline( outlineIntensity );
				break;
			default:
				if( heldItem.type == ModContent.ItemType<FramingPlankItem>() ) {
					TilesInterfaceLogic.DrawPlankTilePlacementOutline( outlineIntensity );
				}
				break;
			}
		}


		////////////////

		private static void DrawTilePlacementOutline( float outlineIntensity, Rectangle tileRect, bool isValid = true ) {
			var scrRect = new Rectangle {
				X = ( tileRect.X << 4 ) - (int)Main.screenPosition.X,
				Y = ( tileRect.Y << 4 ) - (int)Main.screenPosition.Y,
				Width = tileRect.Width << 4,
				Height = tileRect.Height << 4
			};

			Color bgColor = Color.White * 0.15f * outlineIntensity;
			if( !isValid ) {
				bgColor.G = bgColor.B = 0;
			}

			Color edgeColor = AnimatedColors.Air.CurrentColor * 0.35f * outlineIntensity;
			if( !isValid ) {
				edgeColor.G = edgeColor.B = 0;
			}

			DrawHelpers.DrawBorderedRect( Main.spriteBatch, bgColor, edgeColor, scrRect, 2 );
		}
	}
}
