#ifndef SEARCH_H
#define SEARCH_H

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
		std::string toString() const;
	};

	class SearchAgent
	{
	
	public:
		static ActionSequence generalSearch(Problem problem, List *list);
		static ActionSequence BFS(Problem problem);
		//static ActionSequence DFS(Problem problem);

	private:
		static ActionSequence extractActionSequnce(TreeNode *node);
		static void expandNode(List *list,Problem &problem, TreeNode *node);
		static void addNode(List * list, State & state, Actions action, TreeNode * parent);
	};

}

#endif // !SEARCH_H