﻿namespace CtrDotNet.Pokemon.Dynamic
{
	public abstract class DynamicType
	{
		protected DynamicType( int id, string name )
		{
			this.Id = id;
			this.Name = name;
		}

		public int Id { get; }
		public string Name { get; }

		#region Equality

		protected bool Equals( DynamicType other )
		{
			return this.Id == other.Id;
		}

		public override bool Equals( object obj )
		{
			if ( object.ReferenceEquals( null, obj ) )
				return false;
			if ( object.ReferenceEquals( this, obj ) )
				return true;

			return obj.GetType() == this.GetType() && this.Equals( (DynamicType) obj );
		}

		public override int GetHashCode()
		{
			return this.Id;
		}

		public static bool operator ==( DynamicType left, DynamicType right )
		{
			return object.Equals( left, right );
		}

		public static bool operator !=( DynamicType left, DynamicType right )
		{
			return !object.Equals( left, right );
		}

		#endregion
	}
}