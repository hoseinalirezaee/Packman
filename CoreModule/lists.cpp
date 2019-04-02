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

void lists::LifoList::push(TreeNode * node)
{
	this->lifoList.push(node);
}

TreeNode * lists::LifoList::pop()
{
	auto temp = lifoList.top();
	this->lifoList.pop();
	return temp;
}

bool lists::LifoList::empty()
{
	return this->lifoList.empty();
}
