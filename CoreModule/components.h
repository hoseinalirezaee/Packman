#pragma once
#include <string>
#include <sstream>
#include <vector>

using namespace std;

namespace components
{

	enum class EnvType { WALL = 1, EMPTY = 2, FOOD = 3, PACKMAN = 4 };
	
	enum class Actions { RIGHT = 1, LEFT = 2, UP = 3, DOWN = 4 };

	class State
	{
		EnvType **env;
		size_t envRowCount, envColCount;
		int row, column, remainingFood;
	public:
		State();
		State(int envRowCount, int envColCount);
		State(const State &state);
		State(State &&state);
		~State();

		State &operator=(const State &state);
		State &operator=(State &&state);

		bool operator==(const State &state) const;

		int getRow() const;
		int getColumn() const;
		int getRemainingFood() const;
		size_t getEnvRowSize() const;
		size_t getEnvColSize() const;

		void setRow(int row);
		void setColumn(int col);
		void setRemainingFood(int remainingFood);

		EnvType &at(int i, int j) const;

		string toString()
		{
			stringstream s;
			for (int i = 0; i < envRowCount; i++)
			{
				for (int j = 0; j < envColCount; j++)
				{
					s << static_cast<int>(env[i][j]);
				}
				s << std::endl;
			}
			return s.str();
		}
	};
	
	class Problem
	{
		State initialState;
	public:
		
		Problem(const char *inputData, size_t len);

		bool result(State state, Actions action, State &result) const;
		bool isGoal(State state) const;
		State getInitialState() const;

	private:
		State deserializeInput(const char *inputData, size_t len);

		bool isValidState(State state) const;
	};


	class LookupTable
	{
		
		vector<State> **table;
		size_t rowSize, colSize;
	public:
		LookupTable(size_t rowSize, size_t colSize);
		~LookupTable();

		void insert(const State &state);
		//void remove(const State &state);

		bool has(const State &state) const;

		
	};
}


