using Newtonsoft.Json;

namespace BlazorApp2.Pages
{
    public partial class Counter
    {
        protected List<Posts> posts = [];

        protected override async Task OnInitializedAsync()
        {
            posts = await apiServices.GetApiData<Posts>("https://jsonplaceholder.typicode.com/posts");
        }
    }

    public class Posts
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
