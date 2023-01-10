// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Collections.ObjectModel;

/// <summary>
/// Provides utility methods.
/// </summary>
internal static class ExtensionMethods
{
    /// <summary>
    /// Converts the given list of <paramref name="items"/> to a <see cref="ReadOnlyCollection{T}"/>.
    /// </summary>
    /// <param name="items">The list of items to convert.</param>
    /// <typeparam name="T">The data type of the items.</typeparam>
    /// <returns>The given <paramref name="items"/> as a <see cref="ToReadOnlyCollection{T}"/>.</returns>
    public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> items)
        => new (items.ToArray());
}
