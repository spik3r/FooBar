namespace FooBar.Models;

public class ResponseModel(int id, string importantBusinessLogic, Post post)
{
    public int Id { get; set; } = id;
    public string ImportantBusinessLogic { get; set; } = importantBusinessLogic;
    public Post Post { get; set; } = post;
}