using System.Text;

namespace Googler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static async Task<string> GoogleSearch(string query)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://www.google.com/");
                    var response = await client.GetAsync($"search?q={query}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return content;
                    }
                    else
                    {
                        throw new Exception($"Failed to retrieve search results. StatusCode={response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        static string parseResults(string results)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(results);
            var selectNodes = doc.DocumentNode.SelectNodes("//div[@class='g']");
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var node in selectNodes)
            {
                stringBuilder.Append(node.InnerText);
                stringBuilder.Append("\n");
            }
            return stringBuilder.ToString();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var results = await GoogleSearch(QueryBox.Text);
                resultBox.Text = results;
            }
            catch(Exception ex)
            {
                resultBox.Text = ex.Message;
            }
            
        }

        // Парсер пока не работает, потом допишу и обновлю
        private void parseButton_Click(object sender, EventArgs e)
        {
            string results = resultBox.Text;
            resultBox.Text=parseResults(results);
        }
    }
}