#pragma once
#include <vector>
namespace core
{
	namespace search
	{
		template <class T>
		class ITreeNode
		{

		public:
			virtual std::vector<ITreeNode> getChildren() = 0;
			virtual T getContent() = 0;
		};

		class Solver
		{
		public:
			template <class T>
			static ITreeNode<T> DFS(ITreeNode<T> root);
		};

		

		

		template<class T>
		inline ITreeNode<T> Solver::DFS(ITreeNode<T> root)
		{
			return ITreeNode<T>();
		}

	}
}