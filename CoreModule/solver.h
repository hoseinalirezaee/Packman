#pragma once
#include <string>

extern "C" __declspec(dllexport)
int nativeSolve(const char *inputData, size_t inputSize,
	char *&outputData, size_t &outputSize, int algorithm);
