using System;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Services.Hooks.ExtendedHooks;


namespace Ergophobia {
	class ErgophobiaTileLoadable : ILoadable {
		public bool IsCreatingHouse { get; private set; }



		////////////////

		public void OnModsLoad() {
			ErgophobiaAPI.OnPreHouseCreate( ( tileX, tileY ) => {
				this.IsCreatingHouse = true;
				return true;
			} );

			ErgophobiaAPI.OnPostHouseCreate( ( tileX, tileY ) => {
				this.IsCreatingHouse = false;
			} );

			if( ExtendedTileHooks.NonGameplayKillTileCondition == null ) {
				ExtendedTileHooks.NonGameplayKillTileCondition = () => this.IsCreatingHouse;
			} else {
				Func<bool> oldHook = ExtendedTileHooks.NonGameplayKillTileCondition;
				ExtendedTileHooks.NonGameplayKillTileCondition = () => {
					return this.IsCreatingHouse || oldHook();
				};
			}
		}


		public void OnModsUnload() { }

		public void OnPostModsLoad() { }
	}
}
