// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;
using System;
using System.IO;
using System.Linq;

public class HotPatcherEditor : ModuleRules
{
	public HotPatcherEditor(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
		
		PublicIncludePaths.AddRange(
			new string[] {
				// ... add public include paths required here ...
			}
			);
				
		
		PrivateIncludePaths.AddRange(
			new string[] {
				// ... add other private include paths required here ...
			}
			);
			
		
		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
                "Json",
                "ContentBrowser",
                "SandboxFile",
                "JsonUtilities",
                "TargetPlatform",
                "PropertyEditor",
                "Settings",
                "AssetManagerEx",
                "PakFileUtilities",
                "HotPatcherRuntime",
                "BinariesPatchFeature"
				// ... add other public dependencies that you statically link with here ...
			}
			);

		if (Target.Version.MajorVersion > 4 || Target.Version.MinorVersion > 23)
		{
			PublicDependencyModuleNames.Add("ToolMenus");
		}

		System.Func<string, bool,bool> AddPublicDefinitions = (string MacroName,bool bEnable) =>
		{
			PublicDefinitions.Add(string.Format("{0}={1}",MacroName, bEnable ? 1 : 0));
			return true;
		};
		
		bool bIOStoreSupport = Target.Version.MajorVersion > 4 || Target.Version.MinorVersion > 25;
		if (bIOStoreSupport)
		{
			PublicDependencyModuleNames.AddRange(new string[]
			{
				"IoStoreUtilities"
			});
		}
		AddPublicDefinitions("WITH_IO_STORE_SUPPORT", bIOStoreSupport);
		
		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
                "Core",
                "UnrealEd",
                "Projects",
                "DesktopPlatform",
				"InputCore",
                "EditorStyle",
                "LevelEditor",
				"CoreUObject",
				"Engine",
				"Slate",
				"SlateCore",
				"RenderCore"
				// ... add private dependencies that you statically link with here ...	
			}
			);
		
		switch (Target.Configuration)
		{
			case UnrealTargetConfiguration.DebugGame:
			{
				PublicDefinitions.Add("WITH_HOTPATCHER_DEBUGGAME");
				break;
			}
			case UnrealTargetConfiguration.Development:
			{
				PublicDefinitions.Add("WITH_HOTPATCHER_DEVELOPMENT");
				break;
			}
		};
		
		PublicDefinitions.AddRange(new string[]
		{
			"ENABLE_COOK_ENGINE_MAP=0",
			"ENABLE_COOK_PLUGIN_MAP=0"
		});
		
		DynamicallyLoadedModuleNames.AddRange(
			new string[]
			{
				// ... add any modules that your module loads dynamically here ...
			}
			);
		if (Target.Version.MajorVersion < 5 && Target.Version.MinorVersion <= 21)
		{
			bUseRTTI = true;
		}
		
		BuildVersion Version;
		BuildVersion.TryRead(BuildVersion.GetDefaultFileName(), out Version);
		// PackageContext

		AddPublicDefinitions("WITH_EDITOR_SECTION", Version.MajorVersion > 4 || Version.MinorVersion > 24);
		
		System.Console.WriteLine("MajorVersion {0} MinorVersion: {1} PatchVersion {2}",Target.Version.MajorVersion,Target.Version.MinorVersion,Target.Version.PatchVersion);
		bLegacyPublicIncludePaths = false;
		OptimizeCode = CodeOptimization.InShippingBuildsOnly;
        
	}
}
