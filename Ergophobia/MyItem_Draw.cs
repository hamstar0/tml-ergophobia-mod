using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using Ergophobia.Logic;


namespace Ergophobia {
	partial class ErgophobiaItem : GlobalItem {
		/*public override void PostDrawInInventory(
					Item item,
					SpriteBatch sb,
					Vector2 position,
					Rectangle frame,
					Color drawColor,
					Color itemColor,
					Vector2 origin,
					float scale ) {
			if( item?.IsAir != false ) { return; }
			if( item.createTile <= -1 ) { return; }

			if( !TileLogic.CanPlace(item.createTile) ) {
				Texture2D tex = ErgophobiaMod.Instance.DisabledItemTex;

				float posX = position.X + ( ((float)frame.Width / 2f) * scale ) - ( ((float)tex.Width / 2f) * scale );
				float posY = position.Y + ( ((float)frame.Height / 2f) * scale ) - ( ((float)tex.Height / 2f) * scale );
				var pos = new Vector2( posX, posY );
				var rect = new Rectangle( 0, 0, tex.Width, tex.Height );
				var color = Color.White * 0.625f;

				sb.Draw( tex, pos, rect, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f );
			}
		}*/
	}
}
