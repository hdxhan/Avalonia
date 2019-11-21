﻿using System;
using System.Collections.Generic;

#nullable enable

namespace Avalonia.Data
{
    /// <summary>
    /// An optional typed value.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <remarks>
    /// This struct is similar to <see cref="Nullable{T}"/> except it also accepts reference types:
    /// note that null is a valid value for reference types. It is also similar to
    /// <see cref="BindingValue{T}"/> but has only two states: "value present" and "value missing".
    /// 
    /// To create a new optional value you can:
    /// 
    /// - For a simple value, call the <see cref="Optional{T}"/> constructor or use an implicit
    ///   conversion from <typeparamref name="T"/>
    /// - For an missing value, use <see cref="Empty"/> or simply `default`
    /// </remarks>
    public readonly struct Optional<T>
    {
        private readonly T _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Optional{T}"/> struct with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public Optional(T value)
        {
            _value = value;
            HasValue = true;
        }

        /// <summary>
        /// Gets a value indicating whether a value is present.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="HasValue"/> is false.
        /// </exception>
        public T Value => HasValue ? _value : throw new InvalidOperationException("Optional has no value.");

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Optional<T> o && this == o;

        /// <inheritdoc/>
        public override int GetHashCode() => HasValue ? Value!.GetHashCode() : 0;

        /// <summary>
        /// Casts the value (if any) to an <see cref="object"/>.
        /// </summary>
        /// <returns>The cast optional value.</returns>
        public Optional<object> ToObject() => HasValue ? new Optional<object>(Value) : default;

        /// <inheritdoc/>
        public override string ToString() => HasValue ? Value?.ToString() ?? "(null)" : "(empty)";

        /// <summary>
        /// Gets the value if present, otherwise a default value.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value.</returns>
        public T ValueOrDefault(T defaultValue = default) => HasValue ? Value : defaultValue;

        /// <summary>
        /// Gets the value if present, otherwise a default value.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value if present and of the correct type, `default(TResult)` if the value is
        /// present but not of the correct type or null, or <paramref name="defaultValue"/> if the
        /// value is not present.
        /// </returns>
        public TResult ValueOrDefault<TResult>(TResult defaultValue = default)
        {
            return HasValue ?
                Value is TResult result ? result : default
                : defaultValue;
        }

        /// <summary>
        /// Creates an <see cref="Optional{T}"/> from an instance of the underlying value type.
        /// </summary>
        /// <param name="value">The value.</param>
        public static implicit operator Optional<T>(T value) => new Optional<T>(value);

        /// <summary>
        /// Compares two <see cref="Optional{T}"/>s for inequality.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>True if the values are unequal; otherwise false.</returns>
        public static bool operator !=(Optional<T> x, Optional<T> y) => !(x == y);

        /// <summary>
        /// Compares two <see cref="Optional{T}"/>s for equality.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>True if the values are equal; otherwise false.</returns>
        public static bool operator==(Optional<T> x, Optional<T> y)
        {
            if (!x.HasValue && !y.HasValue)
            {
                return true;
            }
            else if (x.HasValue && y.HasValue)
            {
                return EqualityComparer<T>.Default.Equals(x.Value, y.Value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns an <see cref="Optional{T}"/> without a value.
        /// </summary>
        public static Optional<T> Empty => default;
    }
}
