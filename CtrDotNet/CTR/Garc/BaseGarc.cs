﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace CtrDotNet.CTR.Garc
{
	public abstract class BaseGarc
	{
		protected BaseGarc() { }

		public byte[] Data { get; protected set; }
		public GarcDef Def { get; protected set; }
		public int FileCount => this.Def.Fato.EntryCount;

		public virtual void Read( byte[] data )
		{
			this.Data = data;
			this.Def = CTR.Garc.GarcUtil.UnpackGarc( data );
		}

		public virtual async Task<byte[][]> GetFiles()
		{
			byte[][] data = new byte[ this.FileCount ][];

			for ( int i = 0; i < data.Length; i++ )
				data[ i ] = await this.GetFile( i );

			return data;
		}

		public virtual async Task SetFiles( byte[][] files )
		{
			if ( files == null || files.Length != this.FileCount )
				throw new ArgumentException();

			var memGarc = await CTR.Garc.GarcUtil.PackGarc( files, this.Def.Version, (int) this.Def.ContentPadToNearest );
			this.Def = memGarc.Def;
			this.Data = memGarc.Data;
		}

		protected virtual async Task<byte[]> GetFile( int file, int subfile = 0 )
		{
			var entry = this.Def.Fatb.Entries[ file ];
			var subEntry = entry.SubEntries[ subfile ];

			if ( !subEntry.Exists )
				throw new ArgumentException( "SubFile does not exist." );

			long offset = subEntry.Start + this.Def.DataOffset;
			byte[] data = new byte[ subEntry.Length ];

			using ( var mainStr = new MemoryStream( this.Data ) )
			{
				mainStr.Seek( offset, SeekOrigin.Begin );
				await mainStr.ReadAsync( data, 0, data.Length );
			}

			return data;
		}

		public async Task<byte[]> Save()
		{
			byte[][] data = new byte[ this.FileCount ][];

			for ( int i = 0; i < data.Length; i++ )
			{
				data[ i ] = await this.GetFile( i );
			}

			var memGarc = await GarcUtil.PackGarc( data, this.Def.Version, (int) this.Def.ContentPadToNearest );
			this.Def = memGarc.Def;
			this.Data = memGarc.Data;
			return this.Data;
		}
	}
}