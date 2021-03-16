using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using Ergophobia.Tiles;


namespace Ergophobia.Logic {
	static partial class TileLogic {
		public static bool CanPlace( int type ) {
			var config = ErgophobiaConfig.Instance;
			var wl = config.Get<List<string>>( nameof( config.TilePlaceWhitelist ) );

			if( wl.Contains( TileID.GetUniqueKey(type) ) ) {
				return true;
			}

			if( type == ModContent.TileType<FramingPlankTile>() ) {
				return config.Get<bool>( nameof(config.IsFramingPlankWhitelisted) );
			}
			if( type == ModContent.TileType<TrackDeploymentTile>() ) {
				return config.Get<bool>( nameof(config.IsTrackDeploymentWhitelisted) );
			}
			
			return false;
		}
	}
}
