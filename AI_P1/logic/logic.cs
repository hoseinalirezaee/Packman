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

    public enum EnvType
    {
        Wall = 1,
        Empty = 2,
        Food = 3,
        Packman = 4
    }

    ////public class Problem
    ////{
    ////    private class ParsedData
    ////    {
    ////        public State InitialState { get; set; }
    ////        public byte[] Data { get; set; }
    ////    }

    ////    public byte[] Data
    ////    {
    ////        get;
    ////        private set;
    ////    }

    ////    private State initialState;
    ////    public State InitialState
    ////    {
    ////        get
    ////        {
    ////            return initialState;
    ////        }
    ////    }

    ////    public Problem(string inputString)
    ////    {
    ////        var parsedData = ParseInputString(inputString);
    ////        initialState = parsedData.InitialState;
    ////        Data = parsedData.Data;
    ////    }

    ////    public Problem(byte[] data)
    ////    {
    ////        Data = data;
    ////    }

    ////    public static Problem CreateProblem(string inputString)
    ////    {
    ////        Problem problem = new Problem(inputString);
    ////        return problem;
    ////    }

    ////    private static ParsedData ParseInputString(string inputString)
    ////    {
            
    ////        string[] lines = SplitStringByLine(inputString);

    ////        if (ValidateColumns(lines) == false)
    ////        {
    ////            throw new Exception("Columns sizes must be same.");
    ////        }

    ////        int rowSize = lines.Length;
    ////        int colSize = lines[0].Length;

    ////        State initialState = new State(rowSize, colSize);
            
    ////        MemoryStream memStream = new MemoryStream(rowSize * colSize + 8);

    ////        BinaryWriter writer = new BinaryWriter(memStream);

    ////        writer.Write(rowSize);
    ////        writer.Write(colSize);

    ////        for (int i = 0, k = 0; i < rowSize; i++)
    ////        {
    ////            for (int j = 0; j < colSize; j++, k++)
    ////            {
    ////                switch(lines[i][j])
    ////                {
    ////                    case '%':
    ////                        writer.Write((byte)EnvType.WALL);
    ////                        initialState[i, j] = EnvType.WALL;
    ////                        break;
    ////                    case '.':
    ////                        writer.Write((byte)EnvType.FOOD);
    ////                        initialState[i, j] = EnvType.FOOD;
    ////                        break;
    ////                    case 'p':
    ////                    case 'P':
    ////                        writer.Write((byte)EnvType.PACKMAN);
    ////                        initialState[i, j] = EnvType.PACKMAN;
    ////                        initialState.Packman.Row = i;
    ////                        initialState.Packman.Column = j;
    ////                        break;
    ////                    case ' ':
    ////                        writer.Write((byte)EnvType.EMPTY);
    ////                        initialState[i, j] = EnvType.EMPTY;
    ////                        break;
    ////                    default:
    ////                        throw new Exception("Input string contains an invalid symbol.");
    ////                }
    ////            }
    ////        }

    ////        memStream.Seek(0, SeekOrigin.Begin);

    ////        BinaryReader reader = new BinaryReader(memStream);

    ////        byte[] data = reader.ReadBytes(rowSize * colSize + 8);

    ////        ParsedData parsedData = new ParsedData { Data = data, InitialState = initialState };

    ////        return parsedData;
    ////    }

    ////    private static string[] SplitStringByLine(string str)
    ////    {
    ////        if (str != null)
    ////        {
    ////            List<string> strList = new List<string>();
    ////            StringReader strReader = new StringReader(str);
    ////            string temp;
    ////            while ((temp = strReader.ReadLine()) != null)
    ////            {
    ////                strList.Add(temp);
    ////            }
    ////            return strList.ToArray();
    ////        }
    ////        return null;
    ////    }

    ////    private static bool ValidateColumns(string[] lines)
    ////    {
    ////        for (int i = 0; i < lines.Length; i++)
    ////        {
    ////            for (int j = i + 1; j < lines.Length; j++)
    ////            {
    ////                if (lines[i].Length != lines[j].Length)
    ////                {
    ////                    return false;
    ////                }
    ////            }
    ////        }
    ////        return true;
    ////    }

    ////    public State Result(State state, Actions action)
    ////    {
    ////        State result = new State(state);

    ////        int pacRow = state.Packman.Row;
    ////        int pacCol = state.Packman.Column;

    ////        result[pacRow, pacCol] = EnvType.EMPTY;

    ////        switch (action)
    ////        {
    ////            case Actions.LEFT:
    ////                if (pacCol - 1 >= 0
    ////                    &&
    ////                    state[pacRow, pacCol - 1] != EnvType.WALL)
    ////                {
    ////                    pacCol--;
    ////                }
    ////                else
    ////                {
    ////                    return null;
    ////                }
    ////                break;
    ////            case Actions.UP:
    ////                if (pacRow - 1 >= 0
    ////                    &&
    ////                    state[pacRow - 1, pacCol] != EnvType.WALL)
    ////                {
    ////                    pacRow--;
    ////                }
    ////                else
    ////                {
    ////                    return null;
    ////                }
    ////                break;
    ////            case Actions.RIGHT:
    ////                if (pacCol + 1 < state.ColumnSize
    ////                    &&
    ////                    state[pacRow, pacCol + 1] != EnvType.WALL)
    ////                {
    ////                    pacCol++;
    ////                }
    ////                else
    ////                {
    ////                    return null;
    ////                }
    ////                break;
    ////            case Actions.DOWN:
    ////                if (pacRow + 1 < state.RowSize
    ////                    &&
    ////                    state[pacRow + 1, pacCol] != EnvType.WALL)
    ////                {
    ////                    pacRow++;
    ////                }
    ////                else
    ////                {
    ////                    return null;
    ////                }
    ////                break;
    ////        }

    ////        result[pacRow, pacCol] = EnvType.PACKMAN;
    ////        result.Packman.Row = pacRow;
    ////        result.Packman.Column = pacCol;

    ////        return result;
    ////    }

    ////    public new string ToString()
    ////    {
    ////        StringWriter p = new StringWriter();

    ////        for (int i = 0; i < initialState.RowSize; i++)
    ////        {
    ////            for (int j = 0; j < initialState.ColumnSize; j++)
    ////            {
    ////                switch(initialState[i, j])
    ////                {
    ////                    case EnvType.WALL:
    ////                        p.Write('%');
    ////                        break;
    ////                    case EnvType.PACKMAN:
    ////                        p.Write('P');
    ////                        break;
    ////                    case EnvType.FOOD:
    ////                        p.Write('.');
    ////                        break;
    ////                    case EnvType.EMPTY:
    ////                        p.Write(' ');
    ////                        break;
    ////                }
    ////                p.Write(p.NewLine);
    ////            }
    ////        }
    ////        return p.ToString();
    ////    }
    ////}

    public class Packman
    {
        public Packman(int row, int col)
        {
            Row = row;
            Column = col;
        }
        public int Row { get; private set; }
        public int Column { get; private set; }
    }

    public class State
    {
        private class ParsedData
        {
            public EnvType[,] Environment { get; set; }
            public byte[] Data { get; set; }

            public Packman Packman { get; set; }
        }

        private EnvType[,] env;

        private byte[] data;
        public byte[] Data
        {
            get
            {
                if (data == null)
                {
                    data = new byte[8 + RowSize * ColumnSize];
                    using (BinaryWriter writer = new BinaryWriter(new MemoryStream(data)))
                    {
                        writer.Write((int)RowSize);
                        writer.Write((int)ColumnSize);

                        for (int i = 0; i < RowSize; i++)
                        {
                            for (int j = 0; j < ColumnSize; j++)
                            {
                                writer.Write((byte)env[i, j]);
                            }
                        }
                    }
                }
                return data;
            }
            private set
            {
                data = value;
            }
        }
        public Packman Packman { get; private set; }

        public int RowSize { get { return env.GetLength(0); } }

        public int ColumnSize { get { return env.GetLength(1); } }

        public State(State state) : this(state.RowSize, state.ColumnSize)
        {
            for (int i = 0; i < RowSize; i++)
            {
                for (int j = 0; j < ColumnSize; j++)
                {
                    this.env[i, j] = state[i, j];
                }
            }
            Packman = new Packman(state.Packman.Row, state.Packman.Column);
            Data = state.Data;
        }

        public State(string str)
        {
            var parsedData = ParseInputString(str);
            env = parsedData.Environment;
            Packman = parsedData.Packman;
            Data = parsedData.Data;
        }

        //public State(byte[] data)
        //{

        //}

        public State(int rowSize, int colSize)
        {
            env = new EnvType[rowSize, colSize];
        }

        public EnvType this[int i, int j]
        {
            get
            {
                return env[i, j];
            }
            set
            {
                if (value == EnvType.Packman)
                {
                    if (Packman != null)
                        env[Packman.Row, Packman.Column] = EnvType.Empty;
                    Packman = new Packman(i, j);
                }
                env[i, j] = value;
            }
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
        private static ParsedData ParseInputString(string inputString)
        {
            string[] lines = SplitStringByLine(inputString);

            if (ValidateColumns(lines) == false)
            {
                throw new Exception("Columns sizes must be same.");
            }

            int rowSize = lines.Length;
            int colSize = lines[0].Length;

            EnvType[,] env = new EnvType[rowSize, colSize];

            MemoryStream memStream = new MemoryStream(rowSize * colSize + 8);

            BinaryWriter writer = new BinaryWriter(memStream);

            Packman packman = null;

            writer.Write(rowSize);
            writer.Write(colSize);

            for (int i = 0, k = 0; i < rowSize; i++)
            {
                for (int j = 0; j < colSize; j++, k++)
                {
                    switch (lines[i][j])
                    {
                        case '%':
                            writer.Write((byte)EnvType.Wall);
                            env[i, j] = EnvType.Wall;
                            break;
                        case '.':
                            writer.Write((byte)EnvType.Food);
                            env[i, j] = EnvType.Food;
                            break;
                        case 'p':
                        case 'P':
                            writer.Write((byte)EnvType.Packman);
                            env[i, j] = EnvType.Packman;
                            packman = new Packman(i, j);
                            break;
                        case ' ':
                            writer.Write((byte)EnvType.Empty);
                            env[i, j] = EnvType.Empty;
                            break;
                        default:
                            throw new Exception("Input string contains an invalid symbol.");
                    }
                }
            }

            memStream.Seek(0, SeekOrigin.Begin);

            BinaryReader reader = new BinaryReader(memStream);

            byte[] data = reader.ReadBytes(rowSize * colSize + 8);

            ParsedData parsedData = new ParsedData { Data = data, Environment = env, Packman = packman };

            return parsedData;
        }

        public new string ToString()
        {
            StringWriter strWriter = new StringWriter();

            for (int i = 0; i < env.GetLength(0); i++)
            {
                for (int j = 0; j < env.GetLength(1); j++)
                {
                    switch (env[i, j])
                    {
                        case EnvType.Wall:
                            strWriter.Write('%');
                            break;
                        case EnvType.Empty:
                            strWriter.Write(' ');
                            break;
                        case EnvType.Food:
                            strWriter.Write('.');
                            break;
                        case EnvType.Packman:
                            strWriter.Write('p');
                            break;
                    }
                }
                strWriter.Write(strWriter.NewLine);
            }

            return strWriter.ToString();
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

        public static ActionSecuence Solve(State initialState, Algorithms algorithm)
        {
            IntPtr inputData, outputData;
            int inputSize, outputSize;

            inputSize = initialState.Data.Length;

            inputData = Marshal.AllocHGlobal(inputSize);
            Marshal.Copy(initialState.Data, 0, inputData, inputSize);

            var status = nativeSolve(inputData, inputSize, out outputData, out outputSize, (int)algorithm);
          
            byte[] rawActionSeq = new byte[outputSize];

            Marshal.Copy(outputData, rawActionSeq, 0, outputSize);

            Marshal.FreeHGlobal(inputData);

            return new ActionSecuence(rawActionSeq);
        }


    }
}