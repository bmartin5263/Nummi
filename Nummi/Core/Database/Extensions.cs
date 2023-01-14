using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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
    
}