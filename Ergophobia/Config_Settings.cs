using System;
using System.ComponentModel;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Ergophobia {
	public partial class ErgophobiaConfig : ModConfig {
		public bool DebugModeInfo { get; set; } = false;

		public bool DebugModeSuppressPlacementErrors { get; set; } = false;

		////

		[DefaultValue( true )]
		public bool HouseFramingKitSoldByMerchant { get; set; } = true;

		[DefaultValue( true )]
		public bool HouseFurnishingKitSoldByMerchant { get; set; } = true;

		[DefaultValue( true )]
		public bool ScaffoldingKitSoldByMerchant { get; set; } = true;

		[DefaultValue( true )]
		public bool TrackKitSoldByMerchant { get; set; } = true;


		[Range(0, 99999999 )]
		[DefaultValue( 100000 )]
		public int HouseFramingKitPrice { get; set; } = 100000; //Item.buyPrice( 0, 10, 0, 0 );

		[Range( 0, 99999999 )]
		[DefaultValue( 100000 )]
		public int HouseFurnishingKitPrice { get; set; } = 100000;  //Item.buyPrice( 0, 10, 0, 0 );

		[Range( 0, 99999999 )]
		[DefaultValue( 15000 )]
		public int ScaffoldingKitPrice { get; set; } = 15000;	//Item.buyPrice( 0, 1, 50, 0 );

		[Range( 0, 99999999 )]
		[DefaultValue( 15000 )]
		public int TrackKitPrice { get; set; } = 15000;	//Item.buyPrice( 0, 1, 50, 0 );

		//

		[DefaultValue( true )]
		public bool TrackDeploymentKitEnabled { get; set; } = true;
		
		[Range( 0, 9999 )]
		[DefaultValue( 100 )]
		[ReloadRequired]
		public int TrackDeploymentKitTracks { get; set; } = 100;

		[Range( -1, 9999 )]
		[DefaultValue( TileID.WorkBenches )]
		[ReloadRequired]
		public int TrackDeploymentKitRecipeTile { get; set; } = TileID.WorkBenches;

		[ReloadRequired]
		public Dictionary<ItemDefinition, int> TrackDeploymentKitRecipeExtraIngredient { get; set; } = new Dictionary<ItemDefinition, int> {
			{ new ItemDefinition(ItemID.GrapplingHook), 1 },
			{ new ItemDefinition(ItemID.Minecart), 1 },
			{ new ItemDefinition(ItemID.WoodenBeam), 100 }
		};


		[Range( 0, 9999 )]
		public int FurnishedCustomFurnitureTile { get; set; } = 0;

		[Range( 0, 9999 )]
		public int FurnishedCustomWallMount1Tile { get; set; } = 0;

		[Range( 0, 9999 )]
		public int FurnishedCustomWallMount2Tile { get; set; } = 0;

		[Range( 0, 9999 )]
		public int FurnishedCustomFloorTile { get; set; } = 0;


		[Range( 16, 1024 )]
		[DefaultValue( 78 )]
		public int MinimumFurnishableHouseArea { get; set; } = 78;

		[Range( 4, 128 )]
		[DefaultValue( 12 )]
		public int MinimumFurnishableHouseFloorWidth { get; set; } = 12;


		////

		[Range( -1, 1024 )]
		[DefaultValue( 3 )]
		public int MaxFramingPlankVerticalLength { get; set; } = 3;

		[Range( -1, 1024 )]
		[DefaultValue( 5 )]
		public int MaxFramingPlankHorizontalLength { get; set; } = 5;

		[Range( -1, 1024 )]
		[DefaultValue( 8 )]
		public int MaxPlatformBridgeLength { get; set; } = 8;

		////

		[Range( -1, 64 )]
		[DefaultValue( 8 )]
		public int MaxTrackGapPatchWidth { get; set; } = 16;
	}
}
