﻿#nullable enable
namespace Ordering.Core.Entities.Base
{
    public abstract class EntityBase<TId> : IEntityBase<TId>
    {
        public virtual TId Id { get; protected set; } = default!;

        private int? _requestedHashCode;

        public bool IsTransient() => Id!.Equals(default(TId));

        public override bool Equals(object? obj)
        {
            if (!(obj is EntityBase<TId>)) return false;

            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;

            var item = (EntityBase<TId>) obj;
            if (item.IsTransient() || IsTransient()) return false;

            return item == this;
        }

        public override int GetHashCode()
        {
            if (IsTransient()) return base.GetHashCode();

            _requestedHashCode ??= Id!.GetHashCode() ^ 31;

            return _requestedHashCode.Value;
        }

        public static bool operator ==(EntityBase<TId> left, EntityBase<TId> right) => left.Equals(right);

        public static bool operator !=(EntityBase<TId> left, EntityBase<TId> right) => !(left == right);
    }
}
