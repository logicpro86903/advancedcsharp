// =============================================================================
//  Feature     : Iterators (yield return / yield break)
//  C# Version  : C# 2.0
//  .NET        : .NET Framework 2.0
//  Released    : 2005 (Visual Studio 2005)
// -----------------------------------------------------------------------------
//  Summary:
//    'yield return' lets a method produce a sequence one item at a time. The
//    compiler rewrites the method into a state machine implementing
//    IEnumerable<T>/IEnumerator<T>, so you never write MoveNext/Current by hand.
//    Iterators are LAZY: code runs only as the consumer asks for the next item,
//    which makes infinite sequences possible. 'yield break' ends the sequence.
//    This lazy model is the foundation LINQ is built on.
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public class Iterators
    {
        public static void Run()
        {
            DemonstrateYieldReturn();
            DemonstrateLazyEvaluation();
            DemonstrateYieldBreak();
            DemonstrateCustomCollection();
        }

        // YIELD RETURN
        // - A method returning IEnumerable<T> that contains 'yield return' is an ITERATOR.
        // - Each 'yield return i' hands one value to the caller's foreach and PAUSES the
        //   method; it resumes from the same spot when the next value is requested.
        static IEnumerable<int> GetEvenNumbers(int max)
        {
            for (int i = 0; i <= max; i += 2)
            {
                yield return i;
            }
        }

        public static void DemonstrateYieldReturn()
        {
            foreach (int n in GetEvenNumbers(10))
                Console.Write(n + " ");
            Console.WriteLine();   // 0 2 4 6 8 10
        }

        // INFINITE, LAZY SEQUENCE
        // - 'while (true)' is fine here because values are only generated on demand.
        // - The "[gen]" output shows exactly when each value is produced.
        static IEnumerable<int> Fibonacci()
        {
            int a = 0, b = 1;
            while (true)
            {
                Console.Write("[gen] ");
                yield return a;
                int next = a + b;
                a = b;
                b = next;
            }
        }

        // LAZY (DEFERRED) EVALUATION
        // - Calling Fibonacci() runs NONE of its code; it just creates the iterator object.
        // - Generation interleaves with consumption: [gen] 0 [gen] 1 [gen] 1 ...
        // - When the consumer 'break's, the generator simply stops - no wasted work.
        public static void DemonstrateLazyEvaluation()
        {
            IEnumerable<int> fib = Fibonacci();
            Console.WriteLine("Sequence created, nothing generated yet");

            int count = 0;
            foreach (int f in fib)
            {
                Console.Write(f + " ");
                if (++count == 8) break;
            }
            Console.WriteLine();
        }

        // YIELD BREAK
        // - 'yield break' ends the sequence immediately (like 'return' for iterators).
        // - Here we stop reading at the first empty line; later lines are never processed.
        static IEnumerable<string> ReadUntilEmpty(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.Length == 0)
                    yield break;
                yield return line.ToUpper();
            }
        }

        public static void DemonstrateYieldBreak()
        {
            string[] lines = { "alpha", "beta", "", "never reached" };
            foreach (string line in ReadUntilEmpty(lines))
                Console.WriteLine(line);   // ALPHA, BETA
        }

        // MAKING A CUSTOM CLASS WORK WITH foreach
        // - Implement IEnumerable<T>; GetEnumerator() can itself be an iterator using yield.
        // - The non-generic IEnumerable.GetEnumerator() is required too; it just forwards.
        // - A class can expose ADDITIONAL iterators as ordinary methods (Reversed()).
        public class Playlist : IEnumerable<string>
        {
            private List<string> _songs = new List<string>();

            public void Add(string song) { _songs.Add(song); }

            public IEnumerator<string> GetEnumerator()
            {
                foreach (string song in _songs)
                    yield return "♪ " + song;
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

            public IEnumerable<string> Reversed()
            {
                for (int i = _songs.Count - 1; i >= 0; i--)
                    yield return _songs[i];
            }
        }

        public static void DemonstrateCustomCollection()
        {
            Playlist playlist = new Playlist();
            playlist.Add("Song A");
            playlist.Add("Song B");
            playlist.Add("Song C");

            foreach (string song in playlist)
                Console.WriteLine(song);

            foreach (string song in playlist.Reversed())
                Console.Write(song + " | ");
            Console.WriteLine();           // Song C | Song B | Song A |
        }
    }

    /*

    //1. Deferred execution: argument validation inside an iterator doesn't run until the first
    //   MoveNext(). Validate in a non-iterator wrapper method if you need eager errors.

    //2. Each foreach re-runs the iterator from the start (expensive work is repeated).

    //3. Can't yield inside a catch block, and can't use ref/out parameters in iterators
    //   (C# 13 relaxed some ref restrictions).

    C# 8 added async iterators (IAsyncEnumerable) - see 8_AsyncStreams.cs.

     */
}
