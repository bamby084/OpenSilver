﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if MIGRATION
using System.Windows;
#else
using Windows.UI.Xaml;
#endif

namespace OpenSilver.Internal;

// <summary>
// Abstract base class for all DefaultValueFactory implementations. Default 
// value factories may be registered with the property metadata in place of 
// a default value instance. When the property system resolve the default 
// value for a DP on a DO the factory’s CreateDefaultValue() method will be 
// invoked. The result will be cached per DP per DO.  Part of this pattern
// is to get past the requirement that all DefaultValues be free-threaded.
// For example, this allows using unfrozen Freezables as default values.
//
// For this to work it is expected that CreateDefaultValue returns a default
// value with either no thread affinity or (if it derives from DispatcherObject)
// with thread affinity to the currently executing thread. 
// This is done by simply creating a new instance of the default value type in 
// the call to CreateDefaultValue.  
// </summary>
internal abstract class DefaultValueFactory
{
    internal abstract object DefaultValue { get; }

    internal abstract object CreateDefaultValue(DependencyObject owner, DependencyProperty property);
}
