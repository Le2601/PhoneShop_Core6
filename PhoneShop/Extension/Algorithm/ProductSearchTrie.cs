using NuGet.Packaging;
using PhoneShop.ModelViews;

namespace PhoneShop.Extension.Algorithm
{
    // Lớp quản lý Trie
    public class ProductSearchTrie
    {
        private TrieNode root;

        public ProductSearchTrie()
        {
            root = new TrieNode();
        }

        public void Insert(ProductViewModel product)
        {

            TrieNode node = root;
            foreach (char c in product.Title.ToLower())
            {
                if (!node.Children.ContainsKey(c))
                {
                    node.Children[c] = new TrieNode();
                }
                node = node.Children[c];
            }
            node.IsEndOfWord = true;
            node.Products.Add(product);
        }

        public List<ProductViewModel> Search(string prefix)
        {
            TrieNode node = root;
            List<ProductViewModel> results = new List<ProductViewModel>();

            foreach (char c in prefix.ToLower())
            {
                if (!node.Children.ContainsKey(c))
                {
                    return results;
                }
                node = node.Children[c];
            }

            GetProductsFromNode(node, results);
            return results;
        }

        private void GetProductsFromNode(TrieNode node, List<ProductViewModel> results)
        {
            if (node.IsEndOfWord)
            {
                results.AddRange(node.Products);
            }

            foreach (var child in node.Children.Values)
            {
                GetProductsFromNode(child, results);
            }
        }
    }
}
