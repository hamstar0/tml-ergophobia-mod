using System;
using Terraria;


namespace Ergophobia {
	public class ErgophobiaAPI {
		public static void OnPreHouseCreate( Func<int, int, bool> func ) {
			ErgophobiaMod.Instance.OnPreHouseCreate.Add( func );
		}


		public static void OnPostHouseCreate( Action<int, int> action ) {
			ErgophobiaMod.Instance.OnPostHouseCreate.Add( action );
		}
	}
}