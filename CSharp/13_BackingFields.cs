// =============================================================================
//  Feature     : The 'field' Keyword (field-backed properties)
//  C# Version  : C# 14.0 (first shipped as a preview feature in C# 13)
//  .NET        : .NET 10 (preview in .NET 9)
//  Released    : 2025 (Visual Studio 2022 v17.14 / Visual Studio 2026)
// -----------------------------------------------------------------------------
//  Summary:
//    Inside a property accessor, the contextual keyword 'field' refers to the
//    compiler-generated backing field. You can now add logic (validation,
//    trimming, change notification) to just ONE accessor while keeping the
//    auto-property's generated storage - no more declaring a private _name
//    field by hand the moment a setter needs a single line of logic.
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class BackingFields
    {
        // BEFORE: as soon as the setter needs logic (here: reject null), the auto-property must be
        // converted into a full property with a hand-written private backing field.
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        // WITH 'field': the compiler still generates the backing field; 'field' gives you access
        // to it inside the accessors.
        // - 'get => field;' returns the stored value (could also be written as just 'get;').
        // - 'set => field = value ?? throw ...' validates and stores without a manual _name field.
        // - Mixing is allowed: an auto 'get;' with a custom 'set { field = ...; }' and vice versa.
        public string NameF
        {
            get => field;
            set => field = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /*

    Common uses:
        public string Title { get; set => field = value.Trim(); }
        public int Age { get; set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(); }
        public string Status { get; set { field = value; OnPropertyChanged(); } }

    //1. If the class already has a member named 'field', the keyword takes precedence inside
    //   accessors - use @field or this.field to refer to the member.
    //2. A property using 'field' can have an initializer: public string Name { get; set => ... } = "";

     */
}
