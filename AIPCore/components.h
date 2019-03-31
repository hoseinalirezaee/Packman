#pragma once
#include <string>
#include <sstream>

using namespace std;

namespace components
{

	enum class EnvType { WALL = 1, EMPTY = 2, FOOD = 3, PACKMAN = 4 };
	
	enum class Actions { RIGHT = 1, Left = 2, UP = 3, DOWN = 4 };

	class State
	{
		EnvType **env;
		size_t envRowCount, envColCount;
		int row, column, remainingFood;
	public:
		State();
		State(int envRowCount, int envColCount);
		State(State &state);
		State(State &&state);
		~State();

		State &operator=(State &state);
		State &operator=(State &&state);

		bool operator==(State &state);

		int getRow() const;
		int getColumn() const;
		int getRemainingFood() const;
		size_t getEnvRowSize() const;
		size_t getEnvColSize() const;

		void setRow(int row);
		void setColumn(int col);
		void setRemainingFood(int remainingFood);

		EnvType &at(int i, int j);

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
		
		Problem(char *inputData, size_t len);

		bool result(State state, Actions action, State &result);
		bool isGoal(State state);
		State getInitialState();

	private:
		State deserializeInput(char *inputData, size_t len);

		bool isValidState(State state);
	};
}


