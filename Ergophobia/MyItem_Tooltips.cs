using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Items.Attributes;
using Ergophobia.Logic;


namespace Ergophobia {
	partial class ErgophobiaItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			var config = ErgophobiaConfig.Instance;
			string modName = "[c/FFFF88:" + ErgophobiaMod.Instance.DisplayName + "] - ";

			//

			void addTip( string ctx, string desc ) {
				TooltipLine tip = new TooltipLine( this.mod, "Ergophobia"+ctx, modName + desc );
				ItemInformationAttributeLibraries.AppendTooltipAtEnd( tooltips, tip );
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

			if( item.createTile >= 0 ) {
				if( !TileLogic.CanPlace(item.createTile) ) {
					addTip( "Placeable", "This tile is not allowed to be placed" );
				}
			}
		}
	}
}
