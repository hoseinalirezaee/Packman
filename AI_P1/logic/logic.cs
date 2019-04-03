using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Logic
{
    public enum Algorithms
    {
        BFS = 1,
        DFS = 2
    }

    public enum Actions
    {
        RIGHT = 1,
        LEFT = 2,
        UP = 3,
        DOWN = 4
    }

    public class Problem
    {
        private enum EnvType
        {
            WALL = 1,
            EMPTY = 2,
            FOOD = 3,
            PACKMAN = 4
        }

        public byte[] Data
        {
            get;
            private set;
        }

        public Problem(string inputString)
        {
            Data = ParseInputString(inputString);
        }

        public Problem(byte[] data)
        {
            Data = data;
        }

        public static Problem CreateProblem(string inputString)
        {
            Problem problem = new Problem(inputString);
            return problem;
        }

        private static byte[] ParseInputString(string inputString)
        {
            string[] lines = SplitStringByLine(inputString);

            if (ValidateColumns(lines) == false)
            {
                throw new Exception("Columns sizes must be same.");
            }

            int rowSize = lines.Length;
            int colSize = lines[0].Length;

            MemoryStream memStream = new MemoryStream(rowSize * colSize + 8);

            BinaryWriter writer = new BinaryWriter(memStream);

            writer.Write(rowSize);
            writer.Write(colSize);

            for (int i = 0, k = 0; i < rowSize; i++)
            {
                for (int j = 0; j < colSize; j++, k++)
                {
                    switch(lines[i][j])
                    {
                        case '%':
                            writer.Write((byte)EnvType.WALL);
                            break;
                        case '.':
                            writer.Write((byte)EnvType.FOOD);
                            break;
                        case 'p':
                        case 'P':
                            writer.Write((byte)EnvType.PACKMAN);
                            break;
                        case ' ':
                            writer.Write((byte)EnvType.EMPTY);
                            break;
                        default:
                            throw new Exception("Input string contains an invalid symbol.");
                    }
                }
            }

            memStream.Seek(0, SeekOrigin.Begin);

            BinaryReader reader = new BinaryReader(memStream);

            byte[] data = reader.ReadBytes(rowSize * colSize + 8);
            
            return data;
        }

        private static string[] SplitStringByLine(string str)
        {
            if (str != null)
            {
                List<string> strList = new List<string>();
                StringReader strReader = new StringReader(str);
                string temp;
                while ((temp = strReader.ReadLine()) != null)
                {
                    strList.Add(temp);
                }
                return strList.ToArray();
            }
            return null;
        }

        private static bool ValidateColumns(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = i + 1; j < lines.Length; j++)
                {
                    if (lines[i].Length != lines[j].Length)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }

    public class ActionSecuence
    {
        private Actions[] actionSequence;

        public ActionSecuence(byte[] rawDara)
        {
            actionSequence = ParseRawData(rawDara);
        }

        public Actions this[int i]
        {
            get
            {
                return actionSequence[i];
            }
        }

        private Actions[] ParseRawData(byte[] rawData)
        {
            MemoryStream memStream = new MemoryStream(rawData);

            BinaryReader reader = new BinaryReader(memStream);

            int numberOfActions = reader.ReadInt32();

            Actions[] temp = new Actions[numberOfActions];

            for (int i = 0; i < numberOfActions; i++)
            {
                temp[i] = (Actions)reader.ReadByte();
            }

            return temp;
        }

        public int Size
        {
            get
            {
                return actionSequence.Length;
            }
        }

    }

    public class ProblemSolver
    {
        [DllImport(@"CoreModule.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int nativeSolve(IntPtr inputData, int inputSize, out IntPtr outputData, out int outputSize, int algorithm);

        public static ActionSecuence Solve(Problem problem, Algorithms algorithm)
        {
            IntPtr inputData, outputData;
            int inputSize, outputSize;

            inputSize = problem.Data.Length;

            inputData = Marshal.AllocHGlobal(inputSize);
            Marshal.Copy(problem.Data, 0, inputData, inputSize);
            while(true)
            {
                var status = nativeSolve(inputData, inputSize, out outputData, out outputSize, (int)algorithm);
            }
            

            byte[] rawActionSeq = new byte[outputSize];

            Marshal.Copy(outputData, rawActionSeq, 0, outputSize);

            //Marshal.FreeHGlobal(inputData);
            //Marshal.FreeHGlobal(outputData);

            return new ActionSecuence(rawActionSeq);
        }


    }
}