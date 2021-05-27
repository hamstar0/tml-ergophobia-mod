using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
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
			return ModBoilerplateLibraries.HandleModCall( typeof(ErgophobiaAPI), args );
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int whoAmI ) {
			TileRectangleModPacketProtocol.Receive( reader );
		}
	}
}