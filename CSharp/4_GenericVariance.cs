// =============================================================================
//  Feature     : Generic Covariance and Contravariance (in / out)
//  C# Version  : C# 4.0
//  .NET        : .NET Framework 4.0
//  Released    : 2010 (Visual Studio 2010)
// -----------------------------------------------------------------------------
//  Summary:
//    'out' and 'in' modifiers on generic INTERFACE and DELEGATE type parameters
//    let generic types follow the natural "is-a" relationship when it is safe:
//      Covariance     (out T): Producer<Dog>    usable as Producer<Animal>  (T only comes OUT)
//      Contravariance (in T) : Consumer<Animal> usable as Consumer<Dog>     (T only goes IN)
//      Invariance     (T)    : no conversion (e.g. List<T>, where T goes in AND out)
//    .NET 4 annotated IEnumerable<out T>, Func<out TResult>, Action<in T>,
//    IComparer<in T> and others, so these conversions "just work".
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class GenericVariance
    {
        public static void Run()
        {
            DemonstrateCovariance();
            DemonstrateContravariance();
            DemonstrateInvariance();
            DemonstrateCustomVariantInterfaces();
        }

        // A tiny class hierarchy used by all the demos: Dog and Cat are both Animals.
        public class Animal
        {
            public string Name { get; set; } = "";
            public virtual string Speak() => "...";
        }

        public class Dog : Animal
        {
            public override string Speak() => "Woof";
        }

        public class Cat : Animal
        {
            public override string Speak() => "Meow";
        }

        static void PrintAll(IEnumerable<Animal> animals)
        {
            foreach (var a in animals)
                Console.WriteLine($"  {a.Name} says {a.Speak()}");
        }

        // COVARIANCE ('out T')
        // - IEnumerable<out T> only ever hands T values OUT, so a sequence of Dogs can safely
        //   be treated as a sequence of Animals - every Dog you read IS an Animal.
        // - That's why List<Dog> can be passed to a method expecting IEnumerable<Animal>.
        // - Func<out TResult> is covariant in its return type: a Dog factory IS an Animal factory.
        public static void DemonstrateCovariance()
        {
            List<Dog> dogs = new List<Dog> { new Dog { Name = "Rex" }, new Dog { Name = "Fido" } };

            IEnumerable<Animal> animals = dogs;
            Console.WriteLine("Covariance (IEnumerable<Dog> as IEnumerable<Animal>):");
            PrintAll(animals);
            PrintAll(dogs);

            Func<Dog> makeDog = () => new Dog { Name = "Buddy" };
            Func<Animal> makeAnimal = makeDog;
            Console.WriteLine($"  Factory made: {makeAnimal().Name}");
        }

        // CONTRAVARIANCE ('in T')
        // - Action<in T> only takes T values IN. Code that can handle ANY Animal can certainly
        //   handle a Dog, so Action<Animal> converts to Action<Dog> (the direction is REVERSED
        //   compared with covariance).
        // - IComparer<in T> is contravariant: a single comparer for Animals can sort a List<Cat>,
        //   even though List<Cat>.Sort expects an IComparer<Cat>.
        public static void DemonstrateContravariance()
        {
            Action<Animal> feedAnimal = a => Console.WriteLine($"  Feeding {a.Name}");
            Action<Dog> feedDog = feedAnimal;
            Console.WriteLine("Contravariance (Action<Animal> as Action<Dog>):");
            feedDog(new Dog { Name = "Rex" });

            IComparer<Animal> byName = Comparer<Animal>.Create((x, y) => string.Compare(x.Name, y.Name));
            var cats = new List<Cat> { new Cat { Name = "Tom" }, new Cat { Name = "Felix" } };
            cats.Sort(byName);
            Console.WriteLine($"  Sorted cats: {cats[0].Name}, {cats[1].Name}"); // Felix, Tom
        }

        // INVARIANCE AND WHY IT MATTERS
        // - List<T> uses T both as input (Add) and output (indexer), so it can't be variant.
        //   If List<Dog> could become List<Animal>, you could then Add a Cat to a list of Dogs.
        // - If you only need to READ, accept IEnumerable<T> or IReadOnlyList<T> - both covariant.
        // - Arrays ARE covariant (a legacy design from .NET 1.0) and this is UNSAFE: the
        //   mistake compiles but throws ArrayTypeMismatchException at runtime.
        public static void DemonstrateInvariance()
        {
            List<Dog> dogs = new List<Dog>();

            // List<Animal> animals = dogs;   // COMPILE ERROR - otherwise:
            // animals.Add(new Cat());        // a Cat would end up in a List<Dog>!

            IReadOnlyList<Animal> readOnly = dogs;
            Console.WriteLine($"Invariance: List<Dog> can be viewed as IReadOnlyList<Animal> (count {readOnly.Count})");

            Animal[] animalArray = new Dog[1];
            try
            {
                animalArray[0] = new Cat();
            }
            catch (ArrayTypeMismatchException)
            {
                Console.WriteLine("  Array covariance blew up at runtime: ArrayTypeMismatchException");
            }
        }

        // DECLARING YOUR OWN VARIANT INTERFACES
        // - 'out T': T may only appear in OUTPUT positions (return types). Using it as a
        //   parameter is a compile error.
        // - 'in T' : T may only appear in INPUT positions (parameters). Returning it is a
        //   compile error.
        // The compiler enforces these rules, which is what makes the conversions type-safe.
        public interface IProducer<out T>
        {
            T Produce();
            // void Accept(T item);             // compile error: T is 'out'
        }

        public interface IConsumer<in T>
        {
            void Consume(T item);
            // T Get();                         // compile error: T is 'in'
        }

        public class DogShelter : IProducer<Dog>
        {
            public Dog Produce() => new Dog { Name = "Shelter dog" };
        }

        public class Vet : IConsumer<Animal>
        {
            public void Consume(Animal item) => Console.WriteLine($"  Vet checks {item.Name}");
        }

        // USING CUSTOM VARIANT INTERFACES
        // - DogShelter (IProducer<Dog>) is assigned to IProducer<Animal>  -> covariance.
        // - Vet (IConsumer<Animal>) is assigned to IConsumer<Dog>         -> contravariance.
        public static void DemonstrateCustomVariantInterfaces()
        {
            Console.WriteLine("Custom variant interfaces:");
            IProducer<Animal> producer = new DogShelter();
            IConsumer<Dog> consumer = new Vet();
            consumer.Consume(new Dog { Name = producer.Produce().Name });
        }
    }

    /*

    Memory trick:  OUT = things come OUT (return values)  -> covariance  -> Derived to Base
                   IN  = things go IN   (parameters)      -> contravariance -> Base to Derived

    Common variant types in .NET:
        Covariant:      IEnumerable<out T>, IEnumerator<out T>, IReadOnlyList<out T>,
                        IQueryable<out T>, Func<out TResult>
        Invariant:      List<T>, Dictionary<K,V>, Task<T> (classes are never variant)
        Contravariant:  Action<in T>, IComparer<in T>, IEqualityComparer<in T>, Predicate<in T>
        Both:           Func<in T, out TResult>

    //1. Variance only works on interfaces and delegates - NOT on classes (List<T>, Task<T>) or structs.
    //2. Variance only applies to REFERENCE types: IEnumerable<int> can't become IEnumerable<object>
    //   (int would need boxing, so the conversion isn't a simple reference conversion).

     */
}
