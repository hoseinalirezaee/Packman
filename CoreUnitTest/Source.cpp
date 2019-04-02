#include <solver.h>
#include <fstream>

using namespace std;

int main()
{

	size_t inputSize, outputSize;
	char *inputData, *outputData;
	
	ifstream input("E:\\Daneshgah\\Programing\\C++\\C++ Test\\C++ Test\\in.txt");

	input.seekg(0, ios::end);
	inputSize = input.tellg();

	input.seekg(0, ios::beg);

	inputData = new char[inputSize];

	input.read(inputData, inputSize);
	input.close();


	solveProblem(inputData, inputSize, outputData, outputSize, 1);
	
}