using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Helpers.DotNET.Extensions;
using Ergophobia.Items;


namespace Ergophobia.Recipes {
	class TrackDeploymentKitRecipe : ModRecipe {
		public TrackDeploymentKitRecipe() : base( ErgophobiaMod.Instance ) {
			var config = ErgophobiaConfig.Instance;

			if( config.Get<int>( nameof(config.TrackDeploymentKitRecipeTile) ) >= 0 ) {
				this.AddTile( config.Get<int>( nameof(config.TrackDeploymentKitRecipeTile) ) );
			}
			
			//
			
			int tracks = config.Get<int>( nameof(config.TrackDeploymentKitTracks) );
			this.AddIngredient( ItemID.MinecartTrack, tracks );

			string ingredItemsConfigEntry = nameof( config.TrackDeploymentKitRecipeExtraIngredient );
			var ingredItems = config.Get<Dictionary<ItemDefinition, int>>( ingredItemsConfigEntry );
			foreach( (ItemDefinition itemDef, int stack) in ingredItems ) {
				this.AddIngredient( itemDef.Type, stack );
			}

			//

			this.SetResult( ModContent.ItemType<TrackDeploymentKitItem>() );
		}


		public override bool RecipeAvailable() {
			return ErgophobiaConfig.Instance.Get<bool>( nameof(ErgophobiaConfig.TrackDeploymentKitEnabled) );
		}
	}
}
