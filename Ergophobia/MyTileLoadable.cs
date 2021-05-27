using System;
using ModLibsCore.Classes.Loadable;
using ModLibsGeneral.Services.Hooks.ExtendedHooks;


namespace Ergophobia {
	class ErgophobiaTileLoadable : ILoadable {
		public bool IsCreatingHouse { get; private set; }



		////////////////

		public void OnModsLoad() {
			ErgophobiaAPI.OnPreHouseFurnish( ( tileX, tileY ) => {
				this.IsCreatingHouse = true;
				return true;
			} );

			ErgophobiaAPI.OnPostHouseFurnish( ( p1, p2, p3, p4, p5, p6, p7, p8, p9 ) => {
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
