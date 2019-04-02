#ifndef LISTS_H
#define LISTS_H

#include <queue>
#include <stack>
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
		virtual ~List() = default;
	};

	class FifoList : public List
	{
		queue<TreeNode *> fifoList;

	public:
		virtual void push(TreeNode * node);

		virtual TreeNode * pop();

		virtual bool empty();

		virtual ~FifoList() = default;
	};

	class LifoList : public List
	{
		stack<TreeNode *> lifoList;

	public:
		// Inherited via List
		virtual void push(TreeNode * node) override;

		virtual TreeNode * pop() override;

		virtual bool empty() override;

		virtual ~LifoList() = default;
	};
}

#endif // !LISTS_H

