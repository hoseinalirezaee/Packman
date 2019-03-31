#include "search.h"
#include <iostream>
#include <fstream>
#include "components.h"

using namespace components;
using namespace search;
using namespace std;

int main()
{
	
	ifstream input("E:\\Daneshgah\\Programing\\C++\\C++ Test\\C++ Test\\in.txt");
	
	size_t len = 0;
	char *data;

	input.seekg(0, ios::end);
	len = input.tellg();
	input.seekg(0, ios::beg);

	data = new char[len];

	input.read(data, len);

	input.close();


	Problem problem(data, len);

	ActionSequence actionSequenc = SearchAgent::BFS(problem);

	cout << actionSequenc.toString() << endl;


	
}