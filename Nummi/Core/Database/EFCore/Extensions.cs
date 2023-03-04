using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nummi.Core.Exceptions;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore; 

public static class Extensions {
    
    public static T GetById<T>(this DbSet<T> set, object id) where T : class {
        return GetById(set, id, () => throw EntityNotFoundException<T>.IdNotFound(id));
    }

    public static T GetById<T>(this DbSet<T> set, object id, Func<Exception> onMissing) where T : class {
        var obj = set.Find(id);
        if (obj == null) {
            throw onMissing();
        }
        return obj;
    }

    public static T GetById<T>(this IQueryable<T> set, object id, Func<T, object> idProperty) where T : class {
        return GetById(set, id, idProperty, () => EntityNotFoundException<T>.IdNotFound(id));
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

    public static ReferenceReferenceBuilder<P, C> OneToOne<P, C, PKey>(this ModelBuilder builder, string foreignKey, Expression<Func<P, C?>> navigationExpression) where P : class where C : class {
        builder.Entity<C>().Property<PKey?>(foreignKey);
        return builder.Entity<P>()
            .HasOne(navigationExpression)
            .WithOne()
            .HasForeignKey<C>(foreignKey);
    }
    
    public static void DefineTable<E>(this ModelBuilder builder) where E : class {
        builder.DefineTable<E>(typeof(E).Name);
    }
    
    public static void DefineTable<E>(this ModelBuilder builder, string tableName) where E : class {
        builder.Entity<E>().ToTable(tableName);
        builder.Entity<E>()
            .HasKey("Id");
        builder.Entity<E>()
            .Property("Id")
            .HasColumnName(tableName + "Id");
    }
    
    public static void DefineTable<E>(this ModelBuilder builder, string tableName, Expression<Func<E, object?>> keyExpression) where E : class {
        builder.Entity<E>().ToTable(tableName);
        builder.Entity<E>()
            .HasKey(keyExpression);
    }

    public static ReferenceReferenceBuilder<P, C> OneToOne<P, C, ID>(this ModelBuilder builder, string foreignKey) where P : class where C : class {
        builder.Entity<C>().Property<ID?>(foreignKey);
        return builder.Entity<P>()
            .HasOne<C>()
            .WithOne()
            .HasForeignKey<C>(foreignKey);
    }

    public static ReferenceReferenceBuilder<P, C> OneToOneOptionalInverse<P, C, ID>(this ModelBuilder builder, string foreignKey, Expression<Func<P, C?>>? navigationExpression = null) where P : class where C : class {
        builder.Entity<P>().Property<ID?>(foreignKey);
        return builder.Entity<P>()
            .HasOne(navigationExpression)
            .WithOne()
            .HasForeignKey<P>(foreignKey);
    }

    // public static ReferenceReferenceBuilder<P, C> OneToOneInverse<P, C>(this ModelBuilder builder, string foreignKey, Expression<Func<P, C?>>? navigationExpression = null) where P : class where C : class {
    //     return builder.OneToOneInverse<P, C, ID>(foreignKey, navigationExpression);
    // }
    //
    // public static ReferenceReferenceBuilder<P, C> OneToOneInverse<P, C, K>(this ModelBuilder builder, string foreignKey, Expression<Func<P, C?>>? navigationExpression = null) where P : class where C : class {
    //     builder.Entity<P>().Property<K>(foreignKey);
    //     return builder.Entity<P>()
    //         .HasOne(navigationExpression)
    //         .WithOne()
    //         .HasForeignKey<P>(foreignKey);
    // }

    public static ReferenceCollectionBuilder<C, P> ManyToOne<P, C, CKey>(this ModelBuilder builder, string foreignKey, Expression<Func<P, C?>>? navigationExpression = null) where P : class where C : class {
        builder.Entity<P>().Property<CKey>(foreignKey);
        return builder.Entity<P>()
            .HasOne(navigationExpression)
            .WithMany()
            .HasForeignKey(foreignKey);
    }
    
    public static ReferenceCollectionBuilder<P, C> OneToMany<P, C, PKey>(
        this ModelBuilder builder, 
        string foreignKey,
        Expression<Func<P, IEnumerable<C>?>> navigationExpression
    ) where P : class where C : class {
        builder.Entity<C>().Property<PKey?>(foreignKey);
        return builder.Entity<P>()
            .HasMany(navigationExpression)
            .WithOne()
            .HasForeignKey(foreignKey);
    }
    
    public static ReferenceCollectionBuilder<P, C> OneToMany<P, C, K>(
        this ModelBuilder builder, 
        Expression<Func<C, object?>> foreignKeyExpression,
        Expression<Func<P, IEnumerable<C>?>> navigationExpression
    ) where P : class where C : class {
        return builder.Entity<P>()
            .HasMany(navigationExpression)
            .WithOne()
            .HasForeignKey(foreignKeyExpression);
    }
    
    public static ReferenceCollectionBuilder<P, C> OneToMany<P, C>(
        this ModelBuilder builder, 
        Expression<Func<C, object?>> foreignKeyExpression
    ) where P : class where C : class {
        return builder.Entity<P>()
            .HasMany<C>()
            .WithOne()
            .HasForeignKey(foreignKeyExpression);
    }

    
    // public static ReferenceCollectionBuilder<P, C> OneToMany<P, C>(
    //     this ModelBuilder builder, 
    //     string foreignKey
    // ) where P : class where C : class {
    //     builder.Entity<C>().Property<string?>(foreignKey);
    //     return builder.Entity<P>()
    //         .HasMany<C>()
    //         .WithOne()
    //         .HasForeignKey(c => c.GetHashCode());
    // }
    //
    // public static void ManyToOne<E, R>(this ModelBuilder builder,
    //     string foreignKey,
    //     Expression<Func<E, R?>> hasOne,
    //     Expression<Func<R, IEnumerable<E>?>>? withMany) where E : class where R : class {
    //     builder.Entity<E>()
    //         .HasOne(hasOne)
    //         .WithMany(withMany)
    //         .HasForeignKey(foreignKey)
    //         .OnDelete(DeleteBehavior.SetNull);
    // }
    //
    // public static void OneToMany<O, M>(this ModelBuilder builder,
    //     string foreignKey,
    //     Expression<Func<O, IEnumerable<M>?>>? hasMany,
    //     Expression<Func<M, O?>> withOne
    // ) where O : class where M : class {
    //     builder.Entity<O>()
    //         .HasMany(hasMany)
    //         .WithOne(withOne)
    //         .HasForeignKey(foreignKey)
    //         .OnDelete(DeleteBehavior.SetNull);
    // }

    public static void RegisterJsonProperty<E, P>(this ModelBuilder builder, Expression<Func<E, P>> propertyExpression) where E : class {
        builder.Entity<E>()
            .Property(propertyExpression)
            .HasJsonConversion();
    }
    
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) {
        ValueComparer<T?> comparer = new ValueComparer<T?>
        (
            (l, r) => Serializer.ToJson(l) == Serializer.ToJson(r),
            v => v == null ? 0 : Serializer.ToJson(v).GetHashCode(),
            v => Serializer.FromJson<T>(Serializer.ToJson(v))
        );

        propertyBuilder.HasConversion<NummiJsonConverter<T>>();
        propertyBuilder.Metadata.SetValueComparer(comparer);
        propertyBuilder.HasColumnType("jsonb");

        return propertyBuilder;
    }

    public static V GetOrInsert<D, K, V>(this D self, K key, Func<V> defaultValue) where D : IDictionary<K, V>? {
        if (self!.TryGetValue(key, out var value)) {
            return value;
        }
        value = defaultValue();
        if (value == null) {
            throw new SystemArgumentException("GetOrInsert() defaultValue returned null");
        }
        
        self[key] = value;
        return value;
    }
    
    public static V? GetOrInsertNullable<D, K, V>(this D self, K key, Func<V?> defaultValue) where D : IDictionary<K, V>? {
        if (self!.TryGetValue(key, out var value)) {
            return value;
        }
        value = defaultValue();
        if (value == null) {
            return default;
        }
        self[key] = value;
        return value;
    }
}