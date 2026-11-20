using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET 9 / C# 13.0 (2024)
    public class BackingFields
    {
        // Before: Required explicit backing field
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }


        public string NameF
        {
            get => field;
            set => field = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
   
 }
