using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Services.AnimatedColor;
using Ergophobia.Recipes;
using Ergophobia.Items.HouseFurnishingKit;
using Ergophobia.Items.TrackDeploymentKit;


namespace Ergophobia {
	public partial class ErgophobiaMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-ergophobia-mod";


		////////////////

		public static ErgophobiaMod Instance { get; private set; }




		////////////////

		internal IList<Func<int, int, bool>> OnPreHouseFurnish = new List<Func<int, int, bool>>();
		internal IList<HouseFurnishingKitItem.OnFurnishHouse> OnPostHouseFurnish = new List<HouseFurnishingKitItem.OnFurnishHouse>();



		////////////////

		public ErgophobiaMod() {
			ErgophobiaMod.Instance = this;
		}

		public override void Unload() {
			ErgophobiaMod.Instance = null;
		}


		////////////////

		public override void AddRecipes() {
			var trackKitRecipe = new TrackDeploymentKitRecipe();
			trackKitRecipe.AddRecipe();
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof(ErgophobiaAPI), args );
		}


		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			Item heldItem = Main.LocalPlayer.HeldItem;
			if( heldItem == null || heldItem.IsAir ) { return; }

			if( heldItem.type == ModContent.ItemType<TrackDeploymentKitItem>() ) {
				var position = new Vector2( Main.mouseX, Main.mouseY );
				position.X -= 14;
				position.Y -= 32;
				string dirText = Main.LocalPlayer.direction > 0
					? " >"
					: "< ";

				sb.DrawString(
					Main.fontMouseText,
					dirText,
					position,
					AnimatedColors.Strobe.CurrentColor * 0.75f,
					0f,
					default(Vector2),
					2f,
					SpriteEffects.None,
					1f
				);
			}
		}
	}
}