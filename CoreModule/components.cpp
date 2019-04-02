#include "components.h"
#include <string>

using namespace components;

Problem::Problem(const char *inputData, size_t len)
{
	this->initialState = deserializeInput(inputData, len);
}

bool components::Problem::result(State state, Actions action, State &result) const
{
	state.at(state.getRow(), state.getColumn()) = EnvType::EMPTY;

	switch (action)
	{
	case Actions::LEFT:
		state.setColumn(state.getColumn() - 1);
		break;
	case Actions::RIGHT:
		state.setColumn(state.getColumn() + 1);
		break;
	case Actions::UP:
		state.setRow(state.getRow() - 1);
		break;
	case Actions::DOWN:
		state.setRow(state.getRow() + 1);
		break;
	default:
		throw "components::Problem::result";
		break;
	}

	if (isValidState(state))
	{
		if (state.at(state.getRow(), state.getColumn()) == EnvType::WALL)
			return false;
		if (state.at(state.getRow(), state.getColumn()) == EnvType::FOOD)
			state.setRemainingFood(state.getRemainingFood() - 1);
		state.at(state.getRow(), state.getColumn()) = EnvType::PACKMAN;
		result = state;
		return true;
	}
	return false;
}

bool components::Problem::isGoal(State state) const
{
	return state.getRemainingFood() == 0;
}

State components::Problem::getInitialState() const
{
	return initialState;
}

State components::Problem::deserializeInput(const char * inputData, size_t len)
{
	if (inputData == nullptr)
		throw "Cant be null:components::Problem::deserializeInput";

	int envRow = ((int *)inputData)[0];
	int envCol = ((int *)inputData)[1];

	State state(envRow, envCol);

	size_t foodCount = 0;

	const char *main = inputData + 8;

	for (int i = 0, k = 0; i < envRow; i++)
	{
		for (int j = 0; j < envCol; j++, k++)
		{
			switch (main[k])
			{
			case 1:
				state.at(i, j) = EnvType::WALL;
				break;
			case 2:
				state.at(i, j) = EnvType::EMPTY;
				break;
			case 3:
				state.at(i, j) = EnvType::FOOD;
				foodCount++;
				break;
			case 4:
				state.at(i, j) = EnvType::PACKMAN;
				state.setRow(i);
				state.setColumn(j);
				break;
			default:
				throw "components::Problme::deserializeInput.switch";
			}
		}
	}

	state.setRemainingFood(foodCount);

	return state;
}

bool components::Problem::isValidState(State state) const
{
	if (state.getRow() < 0 || state.getRow() >= state.getEnvRowSize())
		return false;
	if (state.getColumn() < 0 || state.getColumn() >= state.getEnvColSize())
		return false;
	if (state.getRemainingFood() < 0)
		return false;
	return true;
}

int components::State::getRow() const
{
	return this->row;
}

int components::State::getColumn() const
{
	return this->column;
}

int components::State::getRemainingFood() const
{
	return this->remainingFood;
}

size_t components::State::getEnvRowSize() const
{
	return this->envRowCount;
}

size_t components::State::getEnvColSize() const
{
	return this->envColCount;
}

void components::State::setRow(int row)
{
	this->row = row;
}

void components::State::setColumn(int col)
{
	this->column = col;
}

void components::State::setRemainingFood(int remainingFood)
{
	this->remainingFood = remainingFood;
}

EnvType & components::State::at(int i, int j) const
{
	return this->env[i][j];
}

components::State::State() : State(0, 0) {};

components::State::State(int envRowCount, int envColCount)
{
	setRow(0);
	setColumn(0);
	setRemainingFood(0);

	if (envRowCount > 0 && envColCount > 0)
	{
		this->envRowCount = envRowCount;
		this->envColCount = envColCount;
		env = new EnvType*[envRowCount];
		for (int i = 0; i < envRowCount; i++)
		{
			env[i] = new EnvType[envColCount];
		}
	}
	else
	{
		env = nullptr;
		this->envRowCount = 0;
		this->envColCount = 0;
	}
	
}

components::State::State(const State & state) : State(state.getEnvRowSize(), state.getEnvColSize())
{
	*this = state;
}

components::State::State(State && state) : State(state.getEnvRowSize(), state.getEnvColSize())
{
	*this = state;
}

components::State::~State()
{
	if (env != nullptr)
	{
		for (int i = 0; i < envRowCount; i++)
		{
			delete[] env[i];
		}
		delete[] env;
	}
}

State & components::State::operator=(const State & state)
{
	this->~State();

	this->row = state.getRow();
	this->column = state.getColumn();
	this->remainingFood = state.getRemainingFood();

	this->envRowCount = state.getEnvRowSize();
	this->envColCount = state.getEnvColSize();

	env = new EnvType*[this->envRowCount];
	for (int i = 0; i < this->envRowCount; i++)
	{
		env[i] = new EnvType[this->envColCount];
		for (int j = 0; j < this->envColCount; j++)
		{
			env[i][j] = state.at(i, j);
		}
	}

	return *this;
}

State & components::State::operator=(State && state)
{
	*this = state;
	return *this;
}

bool components::State::operator==(const State & state) const
{
	if (this->envRowCount != state.envRowCount)
		return false;
	if (this->envColCount != state.envColCount)
		return false;
	if (this->row != state.row || this->column != state.column)
		return false;
	if (this->remainingFood != state.remainingFood)
		return false;

	for (int i = 0; i < envRowCount; i++)
	{
		for (int j = 0; j < envColCount; j++)
		{
			if (env[i][j] != state.env[i][j])
			{
				return false;
			}
		}
	}
	return true;
}

components::LookupTable::LookupTable(size_t rowSize, size_t colSize)
{
	if (rowSize <= 0 || colSize <= 0)
	{
		this->table = nullptr;
		this->rowSize = 0;
		this->colSize = 0;
	}
	else
	{
		this->rowSize = rowSize;
		this->colSize = colSize;
		table = new vector<State> *[rowSize];
		for (int i = 0; i < rowSize; i++)
		{
			table[i] = new vector<State>[colSize];
		}
	}
}

components::LookupTable::~LookupTable()
{
	if (table != nullptr)
	{
		for (int i = 0; i < rowSize; i++)
		{
			delete[] table[i];
		}
		delete[] table;
	}
}

void components::LookupTable::insert(const State &state)
{
	table[state.getRow()][state.getColumn()].push_back(state);
}

bool components::LookupTable::has(const State & state) const
{
	auto list = table[state.getRow()][state.getColumn()];
	
	for (auto item : list)
	{
		if (item == state)
		{
			return true;
		}
	}
	return false;;
}
