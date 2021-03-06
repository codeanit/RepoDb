﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using RepoDb.Enumerations;
using RepoDb.Exceptions;

namespace RepoDb
{
    /// <summary>
    /// A static class used to map the .NET CLR Types into database types.
    /// </summary>
    public static class TypeMapper
    {
        private static readonly IList<TypeMapItem> m_cache = new List<TypeMapItem>();

        static TypeMapper()
        {
            ConversionType = ConversionType.Default;
            new List<TypeMapItem>();
        }

        /// <summary>
        /// Gets or sets the conversion type when converting the instance of <see cref="DbDataReader"/> object into its destination .NET CLR types.
        /// The default value is <see cref="ConversionType.Default"/>.
        /// </summary>
        public static ConversionType ConversionType { get; set; }

        /// <summary>
        /// Gets the list of type-mapping objects.
        /// </summary>
        public static IEnumerable<TypeMapItem> TypeMaps => m_cache;

        /// <summary>
        /// Adds a mapping between .NET CLR Type and database type.
        /// </summary>
        /// <param name="type">The .NET CLR Type to be mapped.</param>
        /// <param name="dbType">The database type where to map the .NET CLR Type.</param>
        public static void Map(Type type, DbType dbType)
        {
            Map(type, dbType, false);
        }

        /// <summary>
        /// Adds a mapping between .NET CLR Type and database type.
        /// </summary>
        /// <param name="type">The .NET CLR Type to be mapped.</param>
        /// <param name="dbType">The database type where to map the .NET CLR Type.</param>
        /// <param name="force">A value that indicates whether to force the mapping. If one is already exists, then it will be overwritten.</param>
        public static void Map(Type type, DbType dbType, bool force = false)
        {
            Map(new TypeMapItem(type, dbType), force);
        }

        /// <summary>
        /// Adds a mapping between .NET CLR Type and database type.
        /// </summary>
        /// <param name="item">The instance of type-mapping object that holds the mapping of .NET CLR Type and database type.</param>
        public static void Map(TypeMapItem item)
        {
            Map(item, false);
        }

        /// <summary>
        /// Adds a mapping between .NET CLR Type and database type.
        /// </summary>
        /// <param name="item">The instance of type-mapping object that holds the mapping of .NET CLR Type and database type.</param>
        /// <param name="force">A value that indicates whether to force the mapping. If one is already exists, then it will be overwritten.</param>
        public static void Map(TypeMapItem item, bool force = false)
        {
            var target = Get(item.Type);
            if (target == null)
            {
                m_cache.Add(item);
            }
            else
            {
                if (force == false)
                {
                    throw new DuplicateTypeMapException($"A mapping for type '{target.Type.FullName}' is already defined. It is currently mapped to '{target.DbType.GetType().FullName}' database type.");
                }
                else
                {
                    target.SetDbType(item.DbType);
                }
            }
        }

        /// <summary>
        /// Gets the instance of type-mapping object that holds the mapping of .NET CLR Type and database type.
        /// </summary>
        /// <param name="type">The .NET CLR Type used for mapping.</param>
        /// <returns>The instance of type-mapping object that holds the mapping of .NET CLR Type and database type.</returns>
        public static TypeMapItem Get(Type type)
        {
            return m_cache.FirstOrDefault(t => t.Type == type);
        }

        /// <summary>
        /// Gets the instance of type-mapping object that holds the mapping of .NET CLR Type and database type.
        /// </summary>
        /// <typeparam name="T">The dynamic .NET CLR Type used for mapping.</typeparam>
        /// <returns>The instance of type-mapping object that holds the mapping of .NET CLR Type and database type.</returns>
        public static TypeMapItem Get<T>()
        {
            return Get(typeof(T));
        }

        /// <summary>
        /// Removes a mapping of targetted .NET CLR Type from the collection.
        /// </summary>
        /// <param name="type">The .NET CLR Type mapping to be removed.</param>
        public static void Unmap(Type type)
        {
            var item = Get(type);
            if (item == null)
            {
                throw new InvalidOperationException($"The type mapping for type '{type.FullName}' is not found.");
            }
            m_cache.Remove(item);
        }
    }
}
