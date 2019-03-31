#include "lists.h"

using namespace search;

void lists::FifoList::push(TreeNode * node)
{
	this->fifoList.push(node);
}

search::TreeNode * lists::FifoList::pop()
{
	search::TreeNode *temp = this->fifoList.front();
	fifoList.pop();
	return temp;
}

bool lists::FifoList::empty()
{
	return this->fifoList.empty();
}
