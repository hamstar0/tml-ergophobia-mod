using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.TModLoader.Mods;
using Ergophobia.Recipes;
using Ergophobia.Items.HouseFurnishingKit;
using Ergophobia.Network;


namespace Ergophobia {
	public partial class ErgophobiaMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-ergophobia-mod";


		////////////////

		public static ErgophobiaMod Instance { get; private set; }




		////////////////

		internal IList<HouseFurnishingKitItem.PreFurnishHouse> OnPreHouseFurnish = new List<HouseFurnishingKitItem.PreFurnishHouse>();
		internal IList<HouseFurnishingKitItem.OnFurnishHouse> OnPostHouseFurnish = new List<HouseFurnishingKitItem.OnFurnishHouse>();


		////////////////

		public Texture2D DisabledItemTex { get; private set; }



		////////////////

		public ErgophobiaMod() {
			ErgophobiaMod.Instance = this;
		}

		public override void Load() {
			if( Main.netMode != NetmodeID.Server && !Main.dedServ ) {
				this.DisabledItemTex = ModContent.GetTexture( "Terraria/MapDeath" );
			}
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
			return ModBoilerplateLibraries.HandleModCall( typeof(ErgophobiaAPI), args );
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int whoAmI ) {
			TileRectangleModPacketProtocol.Receive( reader );
		}
	}
}