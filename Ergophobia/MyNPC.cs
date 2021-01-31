using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Ergophobia.Items;


namespace Ergophobia {
	class ErgophobiaNPC : GlobalNPC {
		public override void SetupShop( int type, Chest shop, ref int nextSlot ) {
			if( type != NPCID.Merchant ) {
				return;
			}

			//

			void addToShop( int itemType, ref int myNextSlot ) {
				var newItem = new Item();
				newItem.SetDefaults( itemType );

				if( myNextSlot < shop.item.Length ) {
					shop.item[ myNextSlot++ ] = newItem;
				}
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
