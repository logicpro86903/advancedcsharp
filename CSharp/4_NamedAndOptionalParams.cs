using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //.NET FW 4 & C#4 2010
    public static class NamedAndOptionalParams
    {

        public static void Run()
        {
            void Greet(string name = "World") => Console.WriteLine($"Hello {name}");
            
            Greet();  //Optional Parameter
            Greet(name: "Alice"); //Named Parameter
        }
    }
}


/*
 
    //1. Changing Default Values is a Breaking Change
    // Version 1
    public void Method(int x = 1) { }

    // Version 2 - this could break existing callers
    public void Method(int x = 2) { } // Default changed!

    //2. Named Parameters can lead to less readable code if overused

 */