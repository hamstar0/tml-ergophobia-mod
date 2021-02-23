using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;


namespace Ergophobia {
	partial class ErgophobiaItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			this.AddCustomTooltips( item, tooltips );
		}


		////////////////

		private void AddCustomTooltips( Item item, List<TooltipLine> tooltips ) {
			ErgophobiaConfig config = ErgophobiaConfig.Instance;
			string modName = "[c/FFFF88:" + ErgophobiaMod.Instance.DisplayName + "] - ";

			//

			void addTip( string ctx, string desc ) {
				TooltipLine tip = new TooltipLine( this.mod, "Ergophobia"+ctx, modName + desc );
				ItemInformationAttributeHelpers.ApplyTooltipAt( tooltips, tip );
			}

			//

			switch( item.type ) {
			case ItemID.Wood:
				addTip( "Wood", "May be used to craft framing planks" );
				break;
			case ItemID.WoodPlatform:
				if( config.Get<int>( nameof(config.MaxPlatformBridgeLength) ) > 0 ) {
					addTip( "Platform", "Only placeable in short ledges attached to something solid" );
				}
				break;
			case ItemID.Rope:
			case ItemID.SilkRope:
			case ItemID.VineRope:
			case ItemID.WebRope:
			case ItemID.Chain:
				addTip( "Rope", "Can only be lowered, unless placed against walls" );
				break;
			case ItemID.MinecartTrack:
				addTip( "Track1", "Can only bridge gaps or be placed downwards" );
				addTip( "Track2", "May be used to craft track deployment kits" );
				break;
			}

			//

			if( item.createTile > -1 ) {
				bool canPlace = config.TilePlaceWhitelist.Contains( TileID.GetUniqueKey(item.createTile) );
				if( !canPlace ) {
					addTip( "Placeable", "This tile is not allowed to be placed" );
				}
			}
		}
	}
}
