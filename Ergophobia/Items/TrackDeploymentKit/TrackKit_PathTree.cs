using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace Ergophobia.Items.TrackDeploymentKit {
	public partial class TrackDeploymentKitItem : ModItem {
		public class PathTree {
			public int TileX;
			public int TileY;
			public int HighestDepthCount;

			public PathTree Top;
			public PathTree Mid;
			public PathTree Bot;


			public int Count() {
				int count = this.HighestDepthCount > 0 ? 1 : 0;
				count += this.Top?.Count() ?? 0;
				count += this.Mid?.Count() ?? 0;
				count += this.Bot?.Count() ?? 0;

				return count;
			}
		}
	}
}
