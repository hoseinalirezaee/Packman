#include "search.h"
#include <queue>
#include <stack>

using namespace core::search;

bool core::search::Solver::isGoal(TreeNode *node)
{
	if (node->getRemainingSeed() == 0)
	{
		return true;
	}
	return false;
}

TreeNode* Solver::BFS(TreeNode *root)
{
	std::queue<TreeNode *> fifoList;
	fifoList.push(root);

	while (!fifoList.empty())
	{
		TreeNode *front = fifoList.front();
		fifoList.pop();
		if (isGoal(front))
		{
			return front;
		}
		else
		{
			std::vector<TreeNode *> children = front->getChildren();
			for (auto i = children.begin(); i != children.end(); i++)
			{
				fifoList.push(*i);
			}
		}
	}
	return nullptr;
}

TreeNode * core::search::Solver::DFS(TreeNode * root)
{
	std::stack<TreeNode *> lifoList;
	lifoList.push(root);

	while (!lifoList.empty())
	{
		TreeNode *front = lifoList.top();
		lifoList.pop();
		if (isGoal(front))
		{
			return front;
		}
		else
		{
			std::vector<TreeNode *> children = front->getChildren();
			for (auto i = children.begin(); i != children.end(); i++)
			{
				lifoList.push(*i);
			}
		}
	}
}

core::search::TreeNode::TreeNode()
{
	setRemainingSeed(0);
	setParent(nullptr);
}

void core::search::TreeNode::setName(std::string name)
{
	this->name = name;
}

void core::search::TreeNode::setRemainingSeed(int seeds)
{
	this->remainingSeed = seeds;
}

void core::search::TreeNode::setParent(TreeNode * parent)
{
	this->parent = parent;
}

void core::search::TreeNode::addChild(TreeNode * child)
{
	this->children.push_back(child);
	child->setParent(this);
}

std::string core::search::TreeNode::getName() const
{
	return this->name;
}

int core::search::TreeNode::getRemainingSeed() const
{
	return this->remainingSeed;
}

TreeNode * core::search::TreeNode::getParent() const
{
	return this->parent;
}

std::vector<TreeNode*> core::search::TreeNode::getChildren() const
{
	return this->children;
}
