#pragma once
#include <vector>
#include <string>
namespace core
{
	namespace search
	{
		class TreeNode
		{
			int remainingSeed;
			std::vector<TreeNode *> children;
			TreeNode *parent;
			std::string name;

		public:
			TreeNode();
			
			void setName(std::string name);
			void setRemainingSeed(int seeds);
			void addChild(TreeNode *child);
			
			std::string getName() const;
			int getRemainingSeed() const;
			TreeNode *getParent() const;
			std::vector<TreeNode *> getChildren() const;

		private:
			void setParent(TreeNode *parent);
		};

		class Solver
		{
		private:
			static bool isGoal(TreeNode *node);
		public:
			
			static TreeNode* BFS(TreeNode *root);
			static TreeNode *DFS(TreeNode *root);
		};


	}
}