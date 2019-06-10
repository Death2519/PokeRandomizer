﻿using System.Threading.Tasks;
using NUnit.Framework;
using PokeRandomizer.Common.Game;
using PokeRandomizer.Tests.Common;
using GameVersion = PokeRandomizer.Common.Game.GameVersion;

namespace PokeRandomizer.Tests.ORAS
{
	[ TestFixture ]
	public class OrasScriptSearchTests : ScriptSearchTestBase
	{
		protected override uint[] SearchValues => new uint[] {
			0x0A0AF1E0, // Code (32-bit)
			0x0A0AF1E1, // Code (64-bit)
			0x0A0AF1E2, // Code (16-bit)
			0x0A0AF1EF, // Debug
		};

		protected override byte[] SearchBytes => new byte[] {
			0x5A,
			0x4F,
			0x04,
			0x00,
			0x18,
			0x00,
		};

		protected override string[] SkipGarcs => new[] {
			@"a\0\0\8", // Really really massive (1+GB) file
		};

		protected override int MaxGarcIndex => 298;

		[ OneTimeSetUp ]
		public async Task SetUpGame()
		{
			this.Game = new GameConfig( GameVersion.ORAS );

			await this.Game.Initialize( Settings.RomPathOras, Language.English );
		}

		[ Test ]
		public async Task FindGarcScripts()
		{
			await this.DoFindGarcScripts( 029, 029 );
		}
	}
}