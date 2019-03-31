#ifndef LISTS_H
#define LISTS_H

#include <queue>

namespace search
{
	class TreeNode;
}

using namespace std;
using namespace search;

namespace lists
{


	class List
	{
	public:
		virtual void push(TreeNode *node) = 0;
		virtual TreeNode *pop() = 0;
		virtual bool empty() = 0;
	};

	class FifoList : public List
	{
		queue<TreeNode *> fifoList;

		virtual void push(TreeNode * node);

		virtual TreeNode * pop();

		virtual bool empty();

	};
}

#endif // !LISTS_H

