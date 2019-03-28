#include "search.h"
#include <iostream>


using namespace core::search;
int main()
{

	TreeNode *root = new TreeNode();
	root->setRemainingSeed(2);
	root->setName("Root");

	TreeNode *L11 = new TreeNode();
	L11->setRemainingSeed(1);
	L11->setName("L11");
	root->addChild(L11);

	TreeNode *L12 = new TreeNode();
	L12->setRemainingSeed(0);
	L12->setName("L12");
	root->addChild(L12);


	TreeNode *L21 = new TreeNode();
	L21->setName("L21");
	L11->addChild(L21);

	std::cout << Solver::BFS(root)->getName();

}