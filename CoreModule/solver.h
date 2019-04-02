#pragma once

_declspec(dllexport)
int solveProblem(const char *inputData, const size_t inputSize,
	char *&outputData, size_t &outputSize, int algorithm);