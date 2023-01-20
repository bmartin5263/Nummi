using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nummi.Core.Util;

namespace Nummi.Core.Database; 

public static class Extensions {
    
    public static T FindById<T>(this DbSet<T> set, object id) where T : class {
        var obj = set.Find(id);
        if (obj == null) {
            throw new EntityNotFoundException<Guid>(id);
        }
        return obj;
    }

    public static void UseEnumStrings<TEntity, TProperty>(
        this ModelBuilder builder,
        Expression<Func<TEntity, TProperty>> propertyExpression
    ) where TEntity : class where TProperty : struct {
        builder.Entity<TEntity>()
            .Property(propertyExpression)
            .HasConversion(new EnumToStringConverter<TProperty>());
    }

    public static void SerializeToJson<T>(this ModelConfigurationBuilder builder) {
        builder
            .Properties<T>()
            .HaveConversion<NummiJsonConverter<T>>();     
    }

    public static void ConvertProperties<T, C>(this ModelConfigurationBuilder builder) {
        builder
            .Properties<T>()
            .HaveConversion<C>();     
    }

    public static void OneToOne<E, R>(this ModelBuilder builder, string foreignKey, Expression<Func<E, R?>>? navigationExpression = null) where E : class where R : class {
        builder.Entity<E>().Property<string?>(foreignKey);
        builder.Entity<E>()
            .HasOne(navigationExpression)
            .WithOne()
            .HasForeignKey<E>(foreignKey);
    }
    
    public static void OneToMany<E, R>(
        this ModelBuilder builder, 
        string foreignKey, 
        Expression<Func<E, R?>> hasOne,
        Expression<Func<R, IEnumerable<E>?>>? withMany
    ) where E : class where R : class {
        builder.Entity<E>()
            .HasOne(hasOne)
            .WithMany(withMany)
            .HasForeignKey(foreignKey)
            .OnDelete(DeleteBehavior.SetNull);
    }
    
    public static void RegisterJsonProperty<E, P>(this ModelBuilder builder, Expression<Func<E, P>> propertyExpression) where E : class where P : class? {
        builder.Entity<E>()
            .Property(propertyExpression)
            .HasJsonConversion();
    }
    
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class? {
        ValueComparer<T?> comparer = new ValueComparer<T?>
        (
            (l, r) => Serializer.ToJson(l) == Serializer.ToJson(r),
            v => v == null ? 0 : Serializer.ToJson(v).GetHashCode(),
            v => Serializer.FromJson<T>(Serializer.ToJson(v))
        );

        propertyBuilder.HasConversion<NummiJsonConverter<T>>();
        propertyBuilder.Metadata.SetValueComparer(comparer);
        propertyBuilder.HasColumnType("text");

        return propertyBuilder;
    }
    
}