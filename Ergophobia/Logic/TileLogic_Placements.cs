using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using Ergophobia.Tiles;


namespace Ergophobia.Logic {
	static partial class TileLogic {
		public static bool CanPlace( int tileType ) {
			var config = ErgophobiaConfig.Instance;
			var wl = config.Get<List<string>>( nameof( config.TilePlaceWhitelist ) );

			if( wl.Contains( TileID.GetUniqueKey(tileType) ) ) {
				return true;
			}

			if( tileType == ModContent.TileType<FramingPlankTile>() ) {
				return config.Get<bool>( nameof(config.IsFramingPlankWhitelisted) );
			}
			if( tileType == ModContent.TileType<TrackDeploymentTile>() ) {
				return config.Get<bool>( nameof(config.IsTrackDeploymentWhitelisted) );
			}
			
			return false;
		}
	}
}
