#include "search.h"
#include "lists.h"
#include <queue>

using namespace search;
using namespace components;

ActionSequence search::SearchAgent::generalSearch(Problem problem, List * list)
{
	TreeNode root;
	root.setState(problem.getInitialState());
	list->push(&root);

	while (!list->empty())
	{
		TreeNode *front = list->pop();

		if (problem.isGoal(front->getState()))
		{
			return extractActionSequnce(front);
		}
		else
		{
			expandNode(list, problem, front);
		}
	}
	return ActionSequence();
}

ActionSequence SearchAgent::BFS(Problem problem)
{
	
	List *fifoList = new FifoList();

	return generalSearch(problem, fifoList);
}
ActionSequence search::SearchAgent::extractActionSequnce(TreeNode * node)
{
	ActionSequence actionSec;
	do
	{
		actionSec.addFirst(node->getAction());
		node = node->getParent();
	} while (node->getParent() != nullptr);

	return actionSec;
}
void search::SearchAgent::expandNode(List *list, Problem &problem, TreeNode *node)
{
	State result;

	if (problem.result(node->getState(), Actions::Left, result))
	{
		addNode(list, result, Actions::Left, node);
	}

	if (problem.result(node->getState(), Actions::UP, result))
	{
		addNode(list, result, Actions::UP, node);
	}

	if (problem.result(node->getState(), Actions::RIGHT, result))
	{
		addNode(list, result, Actions::RIGHT, node);
	}

	if (problem.result(node->getState(), Actions::DOWN, result))
	{
		addNode(list, result, Actions::DOWN, node);
	}

	
}
void search::SearchAgent::addNode(List * list, State & state, Actions action, TreeNode * parent)
{
	TreeNode *expanndedNode = new TreeNode();
	expanndedNode->setAction(action);
	expanndedNode->setParent(parent);
	expanndedNode->setState(state);
	list->push(expanndedNode);
}
//
//TreeNode * search::Solver::DFS(TreeNode * root)
//{
//	std::stack<TreeNode *> lifoList;
//	lifoList.push(root);
//
//	while (!lifoList.empty())
//	{
//		TreeNode *front = lifoList.top();
//		lifoList.pop();
//		if (isGoal(front))
//		{
//			return front;
//		}
//		else
//		{
//			std::vector<TreeNode *> children = front->getChildren();
//			for (auto i = children.begin(); i != children.end(); i++)
//			{
//				lifoList.push(*i);
//			}
//		}
//	}
//}

search::TreeNode::TreeNode()
{
	setParent(nullptr);
}

void search::TreeNode::setParent(TreeNode * parent)
{
	this->parent = parent;
}

Actions search::TreeNode::getAction() const
{
	return this->action;
}

void search::TreeNode::setAction(Actions action)
{
	this->action = action;
}

State search::TreeNode::getState()
{
	return this->state;
}

void search::TreeNode::setState(State state)
{
	this->state = state;
}

TreeNode * search::TreeNode::getParent() const
{
	return this->parent;
}

void search::ActionSequence::addFirst(Actions action)
{
	this->actionSequnce.push_front(action);
}

void search::ActionSequence::addLast(Actions action)
{
	this->actionSequnce.push_back(action);
}

std::string search::ActionSequence::toString() const
{
	std::string str = "";

	for (auto i = this->actionSequnce.begin(); i != this->actionSequnce.end(); i++)
	{
		switch (*i)
		{
		case Actions::DOWN:
			str += "Down -> ";
			break;
		case Actions::Left:
			str += "Left -> ";
			break;
		case Actions::RIGHT:
			str += "Right -> ";
			break;
		case Actions::UP:
			str += "Up -> ";
			break;
		default:
			break;
		}
	}
	return str;
}
