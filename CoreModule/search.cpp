#include "search.h"
#include "lists.h"
#include <queue>

using namespace search;
using namespace components;

template ActionSequence SearchAgent::generalGraphSearch<LifoList>(const Problem &);
template ActionSequence SearchAgent::generalGraphSearch<FifoList>(const Problem &);
template<class LT>
ActionSequence SearchAgent::generalGraphSearch(const Problem &problem)
{
	List *list = new LT();

	LookupTable exploredStates(problem.getInitialState().getEnvRowSize(),
		problem.getInitialState().getEnvColSize());

	TreeNode root;
	root.setState(problem.getInitialState());
	list->push(&root);

	while (!list->empty())
	{
		TreeNode* front = list->pop();

		if (problem.isGoal(front->getState()))
		{
			auto temp = extractActionSequnce(front);
			delete list;
			return temp;
		}

		if (!exploredStates.has(front->getState()))
		{

			exploredStates.insert(front->getState());
			expandNode(list, problem, front, exploredStates);
		}
	}
	delete list;
	return ActionSequence();
}

ActionSequence SearchAgent::BFS(const Problem &problem)
{
	return generalGraphSearch<FifoList>(problem);
}

ActionSequence search::SearchAgent::DFS(const Problem &problem)
{
	return generalGraphSearch<LifoList>(problem);
}

ActionSequence search::SearchAgent::extractActionSequnce(TreeNode * node)
{
	ActionSequence actionSec;

	while (node != nullptr && node->getParent() != nullptr)
	{
		actionSec.addFirst(node->getAction());
		node = node->getParent();
	} 
	return actionSec;
}
void search::SearchAgent::expandNode(List *list, const Problem &problem, TreeNode *node,
	const LookupTable &exploredStates)
{
	State result;

	if (problem.result(node->getState(), Actions::LEFT, result))
	{
		addNode(list, result, Actions::LEFT, node);
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

void search::ActionSequence::serialize(char *& data, size_t & size) const
{
	size = (this->actionSequnce.size() + 4);
	data = new char[size];
	*((int *)data) = this->actionSequnce.size();
	
	int i = 0;
	char *temp = (data + 4);
	for (auto item = actionSequnce.begin(); item != actionSequnce.end(); item++)
	{
		temp[i] = static_cast<char>(*item);
		i++;
	}
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
		case Actions::LEFT:
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