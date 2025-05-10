using System;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

using var db = new BloggingContext();

// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

var count = await db.Blogs.CountAsync();
Console.WriteLine($"Count Blogs: {count}");

// Create
Console.WriteLine("Inserting a new blog");
db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
await db.SaveChangesAsync();

count = await db.Blogs.CountAsync();
Console.WriteLine($"Count Blogs: {count}");

// Read
Console.WriteLine("Querying for a blog");
var blog = await db.Blogs
    .OrderBy(b => b.BlogId)
    .FirstAsync();
Console.WriteLine($"Blog found: {blog.Url}");
Console.WriteLine($"Does it have Post?: {blog.Posts.Count > 0}");

count = await db.Blogs.CountAsync();
Console.WriteLine($"Count Blogs: {count}");

// Update
Console.WriteLine("Updating the blog and adding a post");
blog.Url = "https://devblogs.microsoft.com/dotnet";
blog.Posts.Add(
    new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
await db.SaveChangesAsync();

Console.WriteLine($"Blog found: {blog.Url}");
Console.WriteLine($"Does it have Post?: {blog.Posts.Count > 0}");
Console.WriteLine($"Post found: {blog.Posts[0].Title}");
Console.WriteLine($"Post content: {blog.Posts[0].Content}");

count = await db.Blogs.CountAsync();
Console.WriteLine($"Count Blogs: {count}");

// Delete
Console.WriteLine("Delete the blog");
db.Remove(blog);
await db.SaveChangesAsync();

count = await db.Blogs.CountAsync();
Console.WriteLine($"Count Blogs: {count}");



public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public string DbPath { get; }
    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}