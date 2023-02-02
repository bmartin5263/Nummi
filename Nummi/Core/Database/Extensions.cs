using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nummi.Core.Domain.Crypto.Data;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Database; 

public static class Extensions {
    
    public static T GetById<T>(this DbSet<T> set, object id) where T : class {
        return GetById(set, id, () => throw new EntityNotFoundException<T>(id));
    }

    public static T GetById<T>(this DbSet<T> set, object id, Func<Exception> onMissing) where T : class {
        var obj = set.Find(id);
        if (obj == null) {
            throw onMissing();
        }
        return obj;
    }

    public static T GetById<T>(this IQueryable<T> set, object id, Func<T, object> idProperty) where T : class {
        return GetById(set, id, idProperty, () => throw new EntityNotFoundException<T>(id));
    }

    public static T GetById<T>(this IQueryable<T> set, object id, Func<T, object> idProperty, Func<Exception> onMissing) where T : class {
        var obj = set.FirstOrDefault(o => id == idProperty(o));
        if (obj == null) {
            throw onMissing();
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
    
    public static int AddRangeIfNotExists(this DbSet<Bar> dbSet, IEnumerable<Bar> entities) {
        var allEntities = entities.ToList();
        var symbols = allEntities.Select(e => e.Symbol);
        var openTimes = allEntities.Select(e => e.OpenTimeUnixMs);
        var periods = allEntities.Select(e => e.PeriodMs);
        
        var rawSelection = from entity in dbSet
            where symbols.Contains(entity.Symbol) && openTimes.Contains(entity.OpenTimeUnixMs) && periods.Contains(entity.PeriodMs)
            select entity;

        var refined = from entity in rawSelection.AsEnumerable()
            join pair in allEntities on new { entity.Symbol, entity.OpenTimeUnixMs, entity.PeriodMs }
                                     equals new { pair.Symbol, pair.OpenTimeUnixMs, pair.PeriodMs}
            select entity;

        var entitiesToAdd = allEntities.Except(refined).ToList();
        dbSet.AddRange(entitiesToAdd);

        return entitiesToAdd.Count;
    }
}