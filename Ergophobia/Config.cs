using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Ergophobia {
	public partial class ErgophobiaConfig : ModConfig {
		public static ErgophobiaConfig Instance => ModContent.GetInstance<ErgophobiaConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		public override ModConfig Clone() {
			var clone = base.Clone() as ErgophobiaConfig;

			clone.TrackDeploymentKitRecipeExtraIngredient = this.TrackDeploymentKitRecipeExtraIngredient.ToDictionary(
				kv => new ItemDefinition( kv.Key.mod, kv.Key.name ),
				kv => kv.Value
			);
			clone.TilePlaceWhitelist = this.TilePlaceWhitelist.ToList();

			return clone;
		}
	}
}
