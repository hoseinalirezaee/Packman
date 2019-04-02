#pragma once

#include <vector>
#include <string>
#include <list>
#include "components.h"

namespace lists
{
	class List;
}

using namespace lists;
using namespace components;

namespace search
{
	class TreeNode
	{
		TreeNode *parent;
		State state;
		Actions action;

	public:
		TreeNode();

		TreeNode *getParent() const;
		void setParent(TreeNode *parent);

		Actions getAction() const;
		void setAction(Actions action);

		State getState();
		void setState(State state);

	};

	class ActionSequence
	{
		list<Actions> actionSequnce;

	public:
		void addFirst(Actions action);
		void addLast(Actions action);
		void serialize(char *&data, size_t &size) const;
		std::string toString() const;
	};

	class SearchAgent
	{
	
	public:
		template <class LT>
		static ActionSequence generalGraphSearch(const Problem &problem);
		static ActionSequence BFS(const Problem &problem);
		static ActionSequence DFS(const Problem &problem);

	private:
		static ActionSequence extractActionSequnce(TreeNode *node);
		static void expandNode(List *list, const Problem &problem, TreeNode *node, const LookupTable &exploredStates);
		static void addNode(List * list, State & state, Actions action, TreeNode * parent);
	};

}