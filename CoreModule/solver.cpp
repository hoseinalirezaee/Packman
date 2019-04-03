#include "solver.h"
#include "components.h"
#include "search.h"

using namespace components;
using namespace search;

int nativeSolve(const char *inputData, size_t inputSize,
	char *&outputData, size_t &outputSize, int algorithm)
{
	Problem problem(inputData, inputSize);

	ActionSequence actionSequenc;
	switch (algorithm)
	{
	case 1:
		actionSequenc = SearchAgent::BFS(problem);
		break;
	case 2:
		actionSequenc = SearchAgent::DFS(problem);
		break;
	default:
		return -1;
	}
	actionSequenc.serialize(outputData, outputSize);

	return 0;
}
