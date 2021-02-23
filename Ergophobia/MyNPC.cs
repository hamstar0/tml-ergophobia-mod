using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using Ergophobia.Items.HouseFramingKit;
using Ergophobia.Items.HouseFurnishingKit;
using Ergophobia.Items.ScaffoldingKit;
using Ergophobia.Items.TrackDeploymentKit;


namespace Ergophobia {
	class ErgophobiaNPC : GlobalNPC {
		public override void SetupShop( int type, Chest shop, ref int nextSlot ) {
			if( type != NPCID.Merchant ) {
				return;
			}

			//

			void addToShop( int itemType, ref int myNextSlot ) {
				if( myNextSlot >= shop.item.Length ) {
					LogHelpers.Alert( "Merchant shop could not finish setup." );
					return;
				}
				
				var newItem = new Item();
				newItem.SetDefaults( itemType );
				
				shop.item[ myNextSlot++ ] = newItem;
			}

			//

			var config = ErgophobiaConfig.Instance;

			if( config.Get<bool>( nameof(config.HouseFramingKitSoldByMerchant) ) ) {
				addToShop( ModContent.ItemType<HouseFramingKitItem>(), ref nextSlot );
			}
			if( config.Get<bool>( nameof(config.HouseFurnishingKitSoldByMerchant) ) ) {
				addToShop( ModContent.ItemType<HouseFurnishingKitItem>(), ref nextSlot );
			}
			if( config.Get<bool>( nameof(config.ScaffoldingKitSoldByMerchant) ) ) {
				addToShop( ModContent.ItemType<ScaffoldingErectorKitItem>(), ref nextSlot );
			}
			if( config.Get<bool>( nameof(config.TrackKitSoldByMerchant) ) ) {
				addToShop( ModContent.ItemType<TrackDeploymentKitItem>(), ref nextSlot );
			}
		}
	}
}
