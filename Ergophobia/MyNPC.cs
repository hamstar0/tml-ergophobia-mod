using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
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
					LogLibraries.Alert( "Merchant shop could not finish setup." );
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


		////

		public override void AI( NPC npc ) {
			if( !npc.townNPC ) {
				return;
			}

			int tileX = (int)npc.Center.X / 16;
			int tileY = (int)npc.Center.Y / 16;

			switch( Framing.GetTileSafely( tileX, tileY ).type ) {
			case TileID.Beds:
			case TileID.Containers:
			case TileID.Containers2:
				this.Avoid( npc );
				return;
			}
			switch( Framing.GetTileSafely( tileX, tileY+1 ).type ) {
			case TileID.Beds:
			case TileID.Containers:
			case TileID.Containers2:
				this.Avoid( npc );
				return;
			}
			switch( Framing.GetTileSafely( tileX, tileY+2 ).type ) {
			case TileID.Beds:
			case TileID.Containers:
			case TileID.Containers2:
				this.Avoid( npc );
				return;
			}
		}


		private void Avoid( NPC npc ) {
			npc.velocity.X *= 1.1f;
		}
	}
}
