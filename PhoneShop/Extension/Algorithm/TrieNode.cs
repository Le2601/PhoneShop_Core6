using PhoneShop.ModelViews;
namespace PhoneShop.Extension.Algorithm
{
    //lop dinh nghia Trie
    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; set; }
        public bool IsEndOfWord { get; set; }
        public List<Product_Search_Trie> Products { get; set; }

        public TrieNode()
        {
            Children = new Dictionary<char, TrieNode>();
            IsEndOfWord = false;
            Products = new List<Product_Search_Trie>();
        }

    }
}
