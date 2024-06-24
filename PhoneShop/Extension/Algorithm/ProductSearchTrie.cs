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
            //mac dinh ton tai
            //{
            //    Children = new Dictionary<char, TrieNode>(),
            //    IsEndOfWord = false,
            //    Products = new List<Product_Search_Trie>()
            //};
        }

        public void Insert(Product_Search_Trie product)
        {

            TrieNode node = root;
            //them cac ky tao vao cay trie
            foreach (char c in product.Title.ToLower())
            {
                if (!node.Children.ContainsKey(c))
                {
                    node.Children[c] = new TrieNode();
                }
                node = node.Children[c];
            }
            //danh dau la true neu duyet node o vi tri cuoi cung
            node.IsEndOfWord = true;
            node.Products.Add(product);
        }

        public List<Product_Search_Trie> Search(string prefix)
        {
            //can = c ca can

            TrieNode node = root;
            //doi tuong ds rong
            List<Product_Search_Trie> results = new List<Product_Search_Trie>();

            foreach (char c in prefix.ToLower())
            {
                //ko tont tai
                if (!node.Children.ContainsKey(c))
                {
                    return results;
                }
                //truy cao vao nut co ky tu key = c
                node = node.Children[c];
            }

            
            GetProductsFromNode(node, results);
            return results;
        }


        private void GetProductsFromNode(TrieNode node, List<Product_Search_Trie> results)
        {
            //kiem tra co phai node cuoi hay k
            if (node.IsEndOfWord)
            {
                results.AddRange(node.Products);
            }



            //de quy
            
            foreach (var child in node.Children.Values)
            {
                GetProductsFromNode(child, results);
            }
        }
    }
}
