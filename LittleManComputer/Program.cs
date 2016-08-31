using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LittleManComputer
{
    class Program
    {
        private static bool _isEditingStarted = false;
        private static bool _isDebugEnabled = false;

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome! This is the Little Man Computer.\n" +
                              "You have only one register(A) and 100 bytes of memory to work on.\n" +
                              "To give instructions, type in \"start\".\nUse \"help\" or \"?\" to get an overview of instructions.\n" +
                              "Type in \"stop\" when you are done.");
            List<string> instructionList = new List<string>();
            const int memorySize = 100;

            string input = Console.ReadLine().ToLower();

            while (input != "start")
            {
                EditCheck(input);
                input = Console.ReadLine().ToLower();
            }

            do
            {
                input = Console.ReadLine().ToLower();
                if (EditCheck(input))
                    instructionList.Add(input);

            } while (input != "stop");

            Console.WriteLine("Editing phase ended. You have the following options:\n" +
                              " run\n" +
                              " debug");

            input = Console.ReadLine().ToLower();

            while (!Prerunning(input))
            {
                input = Console.ReadLine().ToLower();
            }

            Computer(instructionList,memorySize);

            Console.ReadKey();
        }

        static bool EditCheck(string instruction)
        {
            string[] inst = instruction.Split(' ');

            switch (inst[0])
            {
                case "start":
                    if (_isEditingStarted)
                        _isEditingStarted = true;
                    else
                        Console.WriteLine("The editing phase has already started.");
                    return false;
                case "stop":
                    return false;
                case "help":
                case "?":
                    Showhelp();
                    return false;
                case "":
                    return false;
                case "hlt":
                case "add":
                case "sub":
                case "sto":
                case "lda":
                case "bra":
                case "brz":
                case "brp":
                case "inp":
                case "out":
                    return true;
            }

            Console.WriteLine("Invalid input entry.");
            return false;
        }

        private static bool Prerunning(string input)
        {
            if (input == "run")
            {
                _isDebugEnabled = false;
                return true;
            }
            else if (input == "debug")
            {
                _isDebugEnabled = true;
                return true;
            }
            else if (input == "help" || input == "?")
                Showhelp();
        
            Console.WriteLine("Command not recognized. Use \"run\" or \"debug\".");
            return false;
        }

        private static void Showhelp()
        {
            Console.WriteLine("Help:");
            Console.WriteLine("HLT       - Halts the program.");
            Console.WriteLine("ADD [loc] - Add value at [loc] to register value.");
            Console.WriteLine("SUB [loc] - Subtracts value at [loc] from register.");
            Console.WriteLine("STO [loc] - Store value at memory location.");
            Console.WriteLine("LDA [loc] - Load value from memory to register.");
            Console.WriteLine("BRA [loc] - Set PC to value.");
            Console.WriteLine("BRZ [loc] - Set PC to value if register is 0.");
            Console.WriteLine("BRP [loc] - Set PC to value if register is positive.");
            Console.WriteLine("INP  x     - Sets register to x.");
            Console.WriteLine("OUT       - Writes out register value to console.");
        }

        static void Computer(List<string> instructionList,int memorySize)
        {
            int[] memory = new int[memorySize];
            int A = 0;

            for (int pc = 0; pc < instructionList.Count; pc++)
            {
                var complexInstruction = instructionList[pc];
                var splittedInstruction = complexInstruction.Split(' ');
                string instruction = splittedInstruction[0];
                int loc = 0;

                if (splittedInstruction.Length > 1)
                    loc = Convert.ToInt32(splittedInstruction[1]);

                switch (instruction)
                {
                    case "hlt":
                        return;
                    case "add":
                        A += memory[loc];
                        break;
                    case "sub":
                        A -= memory[loc];
                        break;
                    case "sto":
                        memory[loc] = A;
                        break;
                    case "lda":
                        A = memory[loc];
                        break;
                    case "bra":
                        pc = loc;
                        break;
                    case "brz":
                        if (A == 0) pc = loc;
                        break;
                    case "brp":
                        if (A >= 0) pc = loc;
                        break;
                    case "inp":
                        A = loc;
                        break;
                    case "out":
                        Console.WriteLine(Convert.ToChar(A));
                        break;
                }
                if(_isDebugEnabled)
                    foreach (var m in memory)
                    {
                        Console.Write(m + " ");
                    }
            }
        }
    }
}
