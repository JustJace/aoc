using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Aoc2017.D6
{
    public class D6Solver
    {
        public int SolveP1() => BruteForceP1(File.ReadAllText(@"D6\D6.input"));

        private int BruteForceP1(string input)
        {
            _registerHash.Clear();

            var registers = input.Split(' ').Select(int.Parse).ToArray();
            var toInfinityAndBeyond = 0;

            while (true)
            {
                Redistribute(registers);
                toInfinityAndBeyond++;

                if (NoteStateAndCheckSeen(registers))
                    break;
            }

            return toInfinityAndBeyond;
        }

        public int SolveP2() => BruteForceP2(File.ReadAllText(@"D6\D6.input"));

        private int BruteForceP2(string input)
        {
            _registerHash.Clear();
            var registers = input.Split(' ').Select(int.Parse).ToArray();
            var cycleCount = 0;
            var cycleFound = false;

            while (true)
            {
                Redistribute(registers);

                if (cycleFound)
                    cycleCount++;

                if (NoteStateAndCheckSeen(registers))
                {
                    if (cycleFound)
                    {
                        // Second time we've seen the cycle, so we done
                        break;
                    }
                    else
                    {
                        // Basically start over and look for this one again
                        cycleFound = true;
                        _registerHash.Clear();
                        NoteStateAndCheckSeen(registers);
                    }
                }
            }

            return cycleCount;
        }

        private void Redistribute(int[] registers)
        {
            var maxIndex = IndexOfMax(registers);
            var distributeCount = registers[maxIndex];
            registers[maxIndex] = 0;
            var nextIndex = maxIndex + 1;

            while (distributeCount > 0)
            {
                if (nextIndex >= registers.Length)
                    nextIndex = 0;

                registers[nextIndex]++;

                nextIndex++;
                distributeCount--;
            }
        }

        private int IndexOfMax(int[] registers)
        {
            var maxIndex = 0;
            for (var i = 1; i < registers.Length; i++)
                if (registers[i] > registers[maxIndex])
                    maxIndex = i;
            return maxIndex;
        }

        private HashSet<int> _registerHash = new HashSet<int>();

        private bool NoteStateAndCheckSeen(int[] registers)
        {
            var hash = HashIntArray(registers);
            if (_registerHash.Contains(hash))
                return true;

            _registerHash.Add(hash);
            return false;
        }

        // Stolen from https://stackoverflow.com/questions/3404715/c-sharp-hashcode-for-array-of-ints
        private int HashIntArray(int[] array)
        {
            int hc = array.Length;
            for (int i = 0; i < array.Length; ++i)
            {
                hc = unchecked(hc * 314159 + array[i]);
            }
            return hc;
        }
    }
}
